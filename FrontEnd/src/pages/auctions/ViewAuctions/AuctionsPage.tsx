import { Pagination, Stack } from "@mui/material";
import React, {
  ChangeEvent,
  useCallback,
  useEffect,
  useMemo,
  useState,
} from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { AuctionItem } from "./Components";
import { PageLayout } from "../../../components/PageLayout";
import { AuctionOutput } from "../../../interfaces/AuctionInterfaces";
import { NotificationEvents } from "../../../events/NotificationEvents";
import { Notification } from "../../../events/Notification";
import { ItemPagination } from "../../../interfaces/Pagination";
import "./AuctionsPage.scss";

export const AuctionsPage = () => {
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>([]);
  const [currentPage, setCurrentPage] = React.useState<number>(1);
  // const [pageSize, setPageSize] = React.useState<number>(10);
  const [totalItems, setTotalItems] = React.useState<number>(0);

  const pageSize = 10;

  const loadAuctions = useCallback(async () => {
    Api.get<ItemPagination<AuctionOutput>>(
      `${ApiEndpoints.get_auctions}/${currentPage}/${pageSize}`
    ).then(({ data }) => {
      setAuctions(data.items);
      setTotalItems(data.totalItems);
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handlePaginationChange = useCallback(
    (_event: ChangeEvent<unknown>, page: number) => {
      console.log(page);
      setCurrentPage(page);
    },
    []
  );

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
    Api.get<ItemPagination<AuctionOutput>>(
      `${ApiEndpoints.get_auctions}/${currentPage}/${pageSize}`
    ).then(({ data }) => {
      setAuctions(data.items);
      setTotalItems(data.totalItems);
    });
  }, [currentPage, pageSize]);

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
          title={auction.title}
          price={auction.currentPrice}
          description={auction.description}
          image={auction.image}
          endDate={auction.endDate}
          noOfBids={auction.noOfBids}
        />
      );
    });
  }, [auctions]);

  return (
    <PageLayout>
      <div id="view-auctions">
        <Pagination
          className="auctions-pagination"
          count={Math.ceil(totalItems / pageSize)}
          shape="rounded"
          variant="outlined"
          onChange={handlePaginationChange}
        />
        <Stack className="auction-container">{auctionsShow}</Stack>
      </div>
    </PageLayout>
  );
};
