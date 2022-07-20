import React, { useEffect, useMemo, useState } from "react";
import Carousel from "react-material-ui-carousel";
import { useParams } from "react-router-dom";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import { Divider } from "@mui/material";
import _ from "lodash";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { Image } from "../../../components/Image";
import { PageLayout } from "../../../components/PageLayout";
import { AuctionDetails } from "../../../interfaces/AuctionInterfaces";
import "./ViewAuctionDetails.scss";

export const ViewAuctionDetails = () => {
  const { id } = useParams();
  const [auction, setAuction] = useState<AuctionDetails>();

  useEffect(() => {
    Api.get<AuctionDetails>(`${ApiEndpoints.get_auctions}/${id}`).then(
      ({ data }) => {
        setAuction(data);
      }
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const images = useMemo(() => {
    return (
      <Carousel
        NextIcon={<ArrowForwardIosIcon />}
        PrevIcon={<ArrowBackIosIcon />}
      >
        {auction?.images?.map((image) => {
          return (
            <Image
              width="800px"
              height="600px"
              key={image}
              src={image}
              alt={auction?.title}
            />
          );
        })}
      </Carousel>
    );
  }, [auction]);

  const otherDetails = useMemo(() => {
    if (!auction?.otherDetails) return;

    return Object.keys(auction?.otherDetails).map((key) => {
      return (
        <div key={key} className="line">
          <div className="col">
            <strong>{_.startCase(key)}</strong>
          </div>
          <div className="col">{auction?.otherDetails[key]}</div>
        </div>
      );
    });
  }, [auction]);

  const bids = useMemo(() => {
    if (!auction?.bids || auction?.bids.length === 0)
      return <div>No bids yet</div>;

    return auction.bids
      .sort((a, b) => b.bidAmount - a.bidAmount)
      .map((bid) => {
        return (
          <div key={bid.id} className="bid">
            <div className="bidder-name">
              <strong>{bid.bidderName ?? "unknown"}: </strong>
            </div>
            <div className="bid-amount">{bid.bidAmount}</div>
          </div>
        );
      });
  }, [auction]);

  return (
    <PageLayout>
      <div id="view-auction-details">
        <div className="details-container">
          <div className="base-details">
            <div className="title-price">
              <div className="title">{auction?.title}</div>
              <div className="price">
                <strong>Current price:</strong> {auction?.currentPrice} €
              </div>
            </div>
            {auction?.images.length !== 0 && (
              <div className="images-container">{images}</div>
            )}
            <div className="description">{auction?.description}</div>
          </div>
          <div className="other-details">
            <div className="title">Other Details</div>
            <Divider flexItem />
            {otherDetails}
          </div>
        </div>
        <div className="bids-container">
          <div className="title">Bids</div>
          <Divider flexItem />
          <div className="bids">{bids}</div>
        </div>
      </div>
    </PageLayout>
  );
};
