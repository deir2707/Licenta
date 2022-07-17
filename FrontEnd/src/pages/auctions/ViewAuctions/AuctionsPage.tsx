import { Stack } from "@mui/material";
import React, { useEffect, useState } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

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

  const [connection, setConnection] = useState<HubConnection | null>(null);

  const listenToBids = async () => {
    try {
      const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:5001/hub/auctionHub")
        .configureLogging(LogLevel.Error)
        .withAutomaticReconnect()
        .build();

      connection.on("onPublishMessage", (notification: Notification) => {
        console.log(notification.data);
      });

      await connection.start();
      setConnection(connection);
    } catch (e) {
      console.error(e);
    }
  };

  useEffect(() => {
    listenToBids();

    return () => {
      if (connection) {
        connection.stop();
      }
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <PageLayout>
      <Stack className="auction-container">
        {auctions.map((auction) => {
          return (
            <AuctionItem
              key={auction.id}
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
