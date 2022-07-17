import { Stack } from "@mui/material";
import React, { useEffect } from "react";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { AuctionItem } from "./Components";
import { PageLayout } from "../../../components/PageLayout";
import { AuctionDetails } from "../../../interfaces/AuctionInterfaces";
import "./AuctionsPage.scss";

export const AuctionsPage = () => {
  const [auctions, setAuctions] = React.useState<AuctionDetails[]>([]);

  useEffect(() => {
    Api.get<AuctionDetails[]>(ApiEndpoints.get_auctions).then(({ data }) => {
      setAuctions(data);
    });
  }, []);

  return (
    <PageLayout>
      <Stack className="auction-container">
        {auctions.map((auction) => {
          return (
            <AuctionItem
              id={auction.id}
              name={auction.description}
              price={auction.startingPrice}
              description={auction.description}
              images={auction.images}
            />
          );
        })}
      </Stack>
    </PageLayout>
  );
};
