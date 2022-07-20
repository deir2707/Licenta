import { BidDetails } from "./BidsInterfaces";

export enum AuctionType {
  Car = 1,
  Painting = 2,
  Vase = 3,
}

export interface AuctionOutput {
  id: number;
  title: string;
  description: string;
  startingPrice: number;
  type: AuctionType;
  image: string;
  startDate: Date;
  endDate: Date;
  noOfBids: number;
  currentPrice: number;
}

export interface AuctionDetails {
  id: number;
  title: string;
  description: string;
  startingPrice: number;
  type: AuctionType;
  otherDetails: { [key: string]: string };
  images: string[];
  startDate: Date;
  endDate: Date;
  currentPrice: number;
  noOfBids: number;
  bids: BidDetails[];
}
