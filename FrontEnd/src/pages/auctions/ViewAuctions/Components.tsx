import { useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { NoImage } from "../../../components/NoImage";
import { Image } from "../../../components/Image";

export interface AuctionItemProps {
  id: number;
  name: string;
  price: number;
  description: string;
  images: string[];
}

export const AuctionItem = (props: AuctionItemProps) => {
  const { id, name, price, description, images } = props;
  const navigate = useNavigate();

  const handleOnClick = useCallback(() => {
    navigate(`/auctions/${id}`);
  }, [id, navigate]);

  return (
    <div className="auction-item" onClick={handleOnClick}>
      {images[0] ? (
        <Image src={images[0]} alt={name} className="thumbnail" />
      ) : (
        <NoImage className="thumbnail" />
      )}
      <div className="details-container">
        <div className="title">name: {name.split(" ")[0]} </div>
        <div className="body">
          <p>price: {price}</p>
          <p>description: {description}</p>
          <p>current price: {price}</p>
        </div>
      </div>
    </div>
  );
};
