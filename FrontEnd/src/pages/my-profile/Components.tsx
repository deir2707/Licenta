import { TextField, Button } from "@mui/material";
import React, { useCallback, useEffect, useState } from "react";

import Api from "../../Api";
import { ApiEndpoints } from "../../ApiEndpoints";
import { AuctionsList } from "../../components/auctions-list/AuctionsList";
import { useApiError } from "../../hooks/useApiError";
import { AuctionOutput } from "../../interfaces/AuctionInterfaces";
import { AddBalanceInput, UserDetails } from "../../interfaces/UsersInterfaces";
import dateService from "../../services/DateService";

export const MyProfileDetails = () => {
  const id = localStorage.getItem("userId");

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

  return (
    <>
      User Details:
      {!userDetails && <div> loading</div>}
      {userDetails && (
        <div>
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
    </>
  );
};

export const MyAuctions = () => {
  const { handleApiError } = useApiError();
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>([]);

  const loadAuctions = useCallback(async () => {
    Api.get<AuctionOutput[]>(ApiEndpoints.get_my_auctions)
      .then(({ data }) => {
        const items = data.map((item) => ({
          ...item,
          endDate: dateService.convertUTCDateToLocalDate(
            new Date(item.endDate)
          ),
        }));
        setAuctions(items);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError]);

  useEffect(() => {
    loadAuctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return <AuctionsList auctions={auctions} />;
};

export const WonAuctions = () => {
  const { handleApiError } = useApiError();
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>([]);

  const loadAuctions = useCallback(async () => {
    Api.get<AuctionOutput[]>(ApiEndpoints.get_won_auctions)
      .then(({ data }) => {
        const items = data.map((item) => ({
          ...item,
          endDate: dateService.convertUTCDateToLocalDate(
            new Date(item.endDate)
          ),
        }));
        setAuctions(items);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError]);

  useEffect(() => {
    loadAuctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return <AuctionsList auctions={auctions} />;
};
