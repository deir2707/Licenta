import { Stack } from "@mui/material";
import React, { useEffect, useMemo, useState } from "react";
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
import { NotificationEvents } from "../../../events/NotificationEvents";
import { Notification } from "../../../events/Notification";

export const AuctionsPage = () => {
  const [auctions, setAuctions] = React.useState<AuctionDetails[]>([]);

  const loadAuctions = async () => {
    Api.get<AuctionDetails[]>(ApiEndpoints.get_auctions).then(({ data }) => {
      setAuctions(data);
    });
  };

  useEffect(() => {
    loadAuctions();
  }, []);

  const [connection, setConnection] = useState<HubConnection | null>(null);

  const listenToBids = async () => {
    try {
      const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:5001/hub/auctionHub")
        .configureLogging(LogLevel.Error)
        .withAutomaticReconnect()
        .build();

      connection.onclose(() => {
        console.log("Connection closed");
      });

      connection.on("onPublishMessage", (notification: Notification) => {
        console.log(notification.data);
        switch (notification.event) {
          case NotificationEvents.AuctionBid:
            loadAuctions();
          // debugger;
          // const data = notification.data as BidNotification;
          // const auction = auctions.find(
          //   (auction) => auction.id === data.auctionId
          // );

          // if (auction) {
          //   const index = auctions.indexOf(auction);

          //   auctions.splice(index, 1);

          //   auction.currentPrice = data.bidAmount;
          //   const newAuctions = [...auctions, auction];
          //   debugger;
          //   setAuctions(newAuctions);
          // }
        }
      });

      await connection.start();
      console.log("Connection started");
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

  const auctionsShow = useMemo(() => {
    return auctions.map((auction) => {
      return (
        <AuctionItem
          key={auction.id}
          id={auction.id}
          name={auction.description}
          price={auction.currentPrice}
          description={auction.description}
          images={auction.images}
        />
      );
    });
  }, [auctions]);

  return (
    <PageLayout>
      <Stack className="auction-container">{auctionsShow}</Stack>
    </PageLayout>
  );
};
