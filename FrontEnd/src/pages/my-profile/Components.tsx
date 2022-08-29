import { TextField, Button, Grid } from "@mui/material";
import React, { useCallback, useEffect, useState } from "react";

import Api from "../../Api";
import { ApiEndpoints } from "../../ApiEndpoints";
import { AuctionsList } from "../../components/auctions-list/AuctionsList";
import { useApiError } from "../../hooks/useApiError";
import { AuctionOutput } from "../../interfaces/AuctionInterfaces";
import { AddBalanceInput, UserDetails } from "../../interfaces/UsersInterfaces";

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
        setBalanceToAdd(0);
        loadDetails();
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [balanceToAdd, handleApiError, loadDetails]);

  if (!userDetails) {
    return <div>Loading...</div>;
  }

  return (
    <div className="user-details">
      <p>User Details:</p>
      <Grid container spacing={2}>
        <Grid item xs={6}>
          <TextField
            className="detail"
            id="email"
            name="email"
            label="Email address"
            type="string"
            value={userDetails?.email}
            InputProps={{
              readOnly: true,
            }}
          />
        </Grid>
        <Grid item xs={6}>
          <TextField
            className="detail"
            id="fullName"
            name="fullName"
            label="Full Name"
            type="string"
            value={userDetails?.fullName}
            InputProps={{
              readOnly: true,
            }}
          />
        </Grid>
        <Grid item xs={6}>
          <TextField
            className="detail"
            id="currentBalance"
            name="currentBalance"
            label="Current Balance"
            type="string"
            value={userDetails?.balance}
            InputProps={{
              readOnly: true,
            }}
          />
        </Grid>
        <Grid item xs={6} className="detail">
          <TextField
            id="addBalance"
            label="Add Balance"
            type="number"
            value={balanceToAdd}
            onChange={(e) => {
              const value = Number(e.target.value);
              if (value >= 0) {
                setBalanceToAdd(value);
              }
            }}
          />
          <Button
            id="addBalanceButton"
            variant="contained"
            color="primary"
            onClick={handleAddBalance}
          >
            Add balance
          </Button>
        </Grid>
      </Grid>
    </div>
  );
};

export const MyAuctions = () => {
  const { handleApiError } = useApiError();
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>();

  const loadAuctions = useCallback(async () => {
    Api.get<AuctionOutput[]>(ApiEndpoints.get_my_auctions)
      .then(({ data }) => {
        setAuctions(data);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError]);

  useEffect(() => {
    loadAuctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!auctions) {
    return <div>Loading...</div>;
  }

  return <AuctionsList auctions={auctions} />;
};

export const WonAuctions = () => {
  const { handleApiError } = useApiError();
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>([]);

  const loadAuctions = useCallback(async () => {
    Api.get<AuctionOutput[]>(ApiEndpoints.get_won_auctions)
      .then(({ data }) => {
        setAuctions(data);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError]);

  useEffect(() => {
    loadAuctions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  if (!auctions) {
    return <div>Loading...</div>;
  }

  return <AuctionsList auctions={auctions} />;
};
