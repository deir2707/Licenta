import React from "react";
import { AuctionOutput } from "../../interfaces/AuctionInterfaces";
import { AuctionItem } from "../../pages/auctions/ViewAuctions/Components";
import "./AuctionsList.scss";

export interface AuctionsListProps {
  auctions: AuctionOutput[];
}

export const AuctionsList = (props: AuctionsListProps) => {
  const { auctions } = props;

  if (auctions.length === 0) {
    return <div>There are no auctions!</div>;
  }

  return (
    <div className="auctions-list">
      {auctions.map((auction) => {
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
            isFinished={auction.isFinished}
          />
        );
      })}
    </div>
  );
};
