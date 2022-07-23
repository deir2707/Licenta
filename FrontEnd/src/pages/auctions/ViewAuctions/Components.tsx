import { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Divider } from "@mui/material";

import { NoImage } from "../../../components/NoImage";
import { Image } from "../../../components/Image";
import dateService from "../../../services/DateService";

export interface AuctionItemProps {
  id: number;
  title: string;
  price: number;
  description: string;
  image: string;
  endDate: Date;
  noOfBids: number;
  isFinished: boolean;
}

export const AuctionItem = (props: AuctionItemProps) => {
  const {
    id,
    title,
    price,
    description,
    image,
    endDate,
    noOfBids,
    isFinished,
  } = props;
  const navigate = useNavigate();

  const [timeLeft, setTimeLeft] = useState(dateService.getDuration(endDate));

  useEffect(() => {
    setTimeout(() => {
      setTimeLeft(dateService.getDuration(endDate));
    }, 1000);
  });

  const handleOnClick = useCallback(() => {
    navigate(`/auctions/${id}`);
  }, [id, navigate]);

  const isFinishedClass = useMemo(() => {
    return isFinished ? " finished" : "";
  }, [isFinished]);

  return (
    <div className={`auction-item${isFinishedClass}`} onClick={handleOnClick}>
      <div className="auction-item-image">
        {image ? <Image src={image} alt={title} /> : <NoImage />}
      </div>
      <Divider orientation="vertical" flexItem />
      <div className="details">
        <div className="title">{title.split(" ")[0]}</div>
        <div>{description}</div>
        <div>
          <strong>Bids:</strong> {noOfBids} |{" "}
          {noOfBids === 0 ? (
            <span>
              <strong>Starting price:</strong> {price} €
            </span>
          ) : (
            <span>
              <strong>Current Bid:</strong> {price} €
            </span>
          )}
        </div>
      </div>
      <Divider orientation="vertical" flexItem />
      <div className="expiration">
        {!isFinished ? (
          <>
            <strong>Ends in:</strong>
            {dateService.durationToString(timeLeft)}
          </>
        ) : (
          <span> Finished</span>
        )}
      </div>
    </div>
  );
};
