import React, { useCallback, useEffect, useMemo, useState } from "react";
import Carousel from "react-material-ui-carousel";
import { useParams } from "react-router-dom";
import ArrowBackIosIcon from "@mui/icons-material/ArrowBackIos";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import { Button, Divider, TextField } from "@mui/material";
import _ from "lodash";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { Image } from "../../../components/Image";
import { PageLayout } from "../../../components/PageLayout";
import { AuctionDetails } from "../../../interfaces/AuctionInterfaces";
import { BidInput } from "../../../interfaces/BidsInterfaces";
import { useApiError } from "../../../hooks/useApiError";
import "./ViewAuctionDetails.scss";

export const ViewAuctionDetails = () => {
  const { id } = useParams();
  const userId = localStorage.getItem("userId");
  const { handleApiError } = useApiError();
  const [auction, setAuction] = useState<AuctionDetails>();
  const [bidAmount, setBidAmount] = useState<number>(0);
  const [bidAmountChanged, setBidAmountChanged] = useState<boolean>(false);
  const [status, setStatus] = useState<string>();

  const loadAuction = useCallback(async () => {
    Api.get<AuctionDetails>(`${ApiEndpoints.get_auctions}/${id}`)
      .then(({ data }) => {
        setAuction(data);
      })
      .catch((error) => {
        handleApiError(error);
      });
  }, [handleApiError, id]);

  useEffect(() => {
    loadAuction();
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
              width="770px"
              height="514px"
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
      .sort((a, b) => b.amount - a.amount)
      .map((bid) => {
        return (
          <div key={bid.id} className="bid">
            <div className="bidder-name">
              <strong>{bid.bidderName ?? "unknown"}</strong>
            </div>
            <div className="bid-amount">{bid.amount}</div>
          </div>
        );
      });
  }, [auction]);

  const bidTooSmall = useMemo(() => {
    if (!auction?.currentPrice) return false;

    return bidAmount <= auction?.currentPrice;
  }, [auction, bidAmount]);

  const bidButtonDisabled = useMemo(() => {
    return auction?.isFinished || auction?.sellerId === userId;
  }, [auction?.isFinished, auction?.sellerId, userId]);

  const handleBidSubmit = useCallback(() => {
    const bidInput: BidInput = {
      auctionId: id ?? "",
      bidAmount,
      date: new Date(),
    };

    Api.post<BidInput, number>(`${ApiEndpoints.make_bid}`, bidInput)
      .then(({ data }) => {
        console.log(data);
        loadAuction();
        setStatus("Bid successful");
        setBidAmountChanged(false);
      })
      .catch((error) => {
        handleApiError(error, setStatus);
      });
  }, [bidAmount, handleApiError, id, loadAuction]);

  return (
    <PageLayout>
      <div id="view-auction-details">
        <div className="details-container">
          <div className="base-details">
            <div className="title-price">
              <div className="title">{auction?.title}</div>
              <div className="price">
                <strong>
                  {auction?.isFinished ? "Final price" : "Current price"}:
                </strong>{" "}
                {auction?.currentPrice} €
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
          <Divider flexItem />
          <div className="make-bid-container">
            <TextField
              id="make-bid-amount"
              label="Amount"
              type="number"
              value={bidAmount}
              disabled={bidButtonDisabled}
              onChange={(e) => {
                const value = Number(e.target.value);
                if (value >= 0 && value <= 2147483647) {
                  setBidAmountChanged(true);
                  setBidAmount(value);
                  setStatus(undefined);
                }
              }}
            />
            {bidAmountChanged && bidTooSmall && !bidButtonDisabled && (
              <div className="error">
                Bid amount must be greater than current price
              </div>
            )}
            <Button
              variant="contained"
              color="primary"
              onClick={handleBidSubmit}
              disabled={bidTooSmall || bidButtonDisabled}
            >
              Make bid
            </Button>
            {status && <div className="status">{status}</div>}
          </div>
        </div>
      </div>
    </PageLayout>
  );
};
