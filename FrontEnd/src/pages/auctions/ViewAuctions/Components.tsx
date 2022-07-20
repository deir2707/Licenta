import { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { NoImage } from "../../../components/NoImage";
import { Image } from "../../../components/Image";
import Moment from "react-moment";
import { Divider } from "@mui/material";
import dateService from "../../../services/DateService";

export interface AuctionItemProps {
  id: number;
  title: string;
  price: number;
  description: string;
  image: string;
  endDate: Date;
  noOfBids: number;
}

export const AuctionItem = (props: AuctionItemProps) => {
  const { id, title, price, description, image, endDate, noOfBids } = props;
  const navigate = useNavigate();

  const [timeLeft, setTimeLeft] = useState(dateService.getDuration(endDate));

  useEffect(() => {
    const timer = setTimeout(() => {
      setTimeLeft(dateService.getDuration(endDate));
    }, 1000);
  });

  const handleOnClick = useCallback(() => {
    navigate(`/auctions/${id}`);
  }, [id, navigate]);

  return (
    <div className="auction-item" onClick={handleOnClick}>
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
        <strong>Ends in:</strong>
        {dateService.durationToString(timeLeft)}
      </div>
    </div>
  );
};
