import { Button, TextField } from "@mui/material";
import React, { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Api from "../Api";
import { ApiEndpoints } from "../ApiEndpoints";
import { PageLayout } from "../components/PageLayout";
import { useApiError } from "../hooks/useApiError";
import { AddBalanceInput, UserDetails } from "../interfaces/UsersInterfaces";

export const MyProfilePage = () => {
  const id = Number(localStorage.getItem("userId"));

  const navigate = useNavigate();
  const { handleApiError } = useApiError();
  const [userDetails, setUserDetails] = useState<UserDetails>();
  const [balanceToAdd, setBalanceToAdd] = useState(0);

  const loadDetails = useCallback(() => {
    Api.get<UserDetails>(`${ApiEndpoints.users}/${id}`)
      .then(({ data }) => {
        setUserDetails(data);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError, id]);

  useEffect(() => {
    loadDetails();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleAddBalance = useCallback(() => {
    const addBalanceInput: AddBalanceInput = {
      balanceToAdd: balanceToAdd,
    };

    Api.post<AddBalanceInput, number>(
      `${ApiEndpoints.users}/add-balance`,
      addBalanceInput
    )
      .then(({ data }) => {
        loadDetails();
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [balanceToAdd, handleApiError, loadDetails]);

  if (!id) {
    return (
      <div>
        <div>You are not logged in</div>
        <Button
          variant="contained"
          color="primary"
          onClick={() => {
            navigate("/");
          }}
        >
          Go to Login Page
        </Button>
      </div>
    );
  }

  return (
    <PageLayout>
      User Details:
      {!userDetails && <div> loading</div>}
      {userDetails && (
        <div>
          <div>Id: {userDetails?.id}</div>
          <div>Email: {userDetails?.email}</div>
          <div>Full Name: {userDetails?.fullName}</div>
          <div>Balance: {userDetails?.balance}</div>
          <div>
            <TextField
              id="add-balance"
              label="balance"
              type="number"
              value={balanceToAdd}
              onChange={(e) => setBalanceToAdd(Number(e.target.value))}
            />
            <Button
              variant="contained"
              color="primary"
              onClick={handleAddBalance}
            >
              Add balance
            </Button>
          </div>
        </div>
      )}
    </PageLayout>
  );
};
