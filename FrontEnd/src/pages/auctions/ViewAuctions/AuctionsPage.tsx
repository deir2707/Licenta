import { Pagination } from "@mui/material";
import React, { ChangeEvent, useCallback, useEffect } from "react";
import PubSub from "pubsub-js";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { PageLayout } from "../../../components/PageLayout";
import { AuctionOutput } from "../../../interfaces/AuctionInterfaces";
import { ItemPagination } from "../../../interfaces/Pagination";
import "./AuctionsPage.scss";
import { useApiError } from "../../../hooks/useApiError";
import { PubSubEvents } from "../../../services/notifications/PubSubEvents";
import dateService from "../../../services/DateService";
import { AuctionFinishedNotification } from "../../../events/AuctionFinishedNotification";
import { AuctionNotification } from "../../../events/AuctionNotification";
import { AuctionsList } from "../../../components/auctions-list/AuctionsList";

export const AuctionsPage = () => {
  const { handleApiError } = useApiError();
  const [auctions, setAuctions] = React.useState<AuctionOutput[]>([]);
  const [currentPage, setCurrentPage] = React.useState<number>(1);
  const [totalItems, setTotalItems] = React.useState<number>(0);

  const pageSize = 10;

  const loadAuctions = useCallback(
    async (resetToFirstPage: boolean = true) => {
      Api.get<ItemPagination<AuctionOutput>>(
        `${ApiEndpoints.get_auctions}/${currentPage}/${pageSize}`
      )
        .then(({ data }) => {
          resetToFirstPage && setCurrentPage(1);
          const items = data.items.map((item) => ({
            ...item,
            startDate:
              // dateService.convertUTCDateToLocalDate(
              new Date(item.startDate),
            // )
            endDate:
              // dateService.convertUTCDateToLocalDate(
              new Date(item.endDate),
            // ),
          }));

          setAuctions(items);
          setTotalItems(data.totalItems);
        })
        .catch((error) => {
          handleApiError(error);
        });
    },
    [currentPage, handleApiError]
  );

  const handlePaginationChange = useCallback(
    (_event: ChangeEvent<unknown>, page: number) => {
      setCurrentPage(page);
    },
    []
  );

  const handleAuctionFinished = useCallback(
    (notification: AuctionNotification) => {
      const auctionFinished = notification.data as AuctionFinishedNotification;

      const auctionId = auctionFinished.auctionId;

      const newAuctions = auctions.filter(
        (auction) => auction.id !== auctionId
      );
      let finishedAuction = auctions.find(
        (auction) => auction.id === auctionId
      );

      if (finishedAuction) {
        finishedAuction = {
          ...finishedAuction,
          isFinished: true,
        };

        newAuctions.push(finishedAuction);
        newAuctions.sort((a, b) => {
          return a.endDate.getTime() - b.endDate.getTime();
        });
      }

      setAuctions(newAuctions);
    },
    [auctions]
  );

  useEffect(() => {
    loadAuctions(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [currentPage]);

  useEffect(() => {
    PubSub.subscribe(PubSubEvents.AuctionBid, async () => {
      await loadAuctions();
    });
    PubSub.subscribe(PubSubEvents.AuctionFinished, async (event, data) => {
      handleAuctionFinished(data as unknown as AuctionNotification);
    });

    return () => {
      PubSub.unsubscribe(PubSubEvents.AuctionBid);
      PubSub.unsubscribe(PubSubEvents.AuctionFinished);
    };
  }, [handleAuctionFinished, loadAuctions]);
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
        <AuctionsList auctions={auctions} />
      </div>
    </PageLayout>
  );
};
