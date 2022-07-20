import { BidDetails } from "./BidsInterfaces";

export enum AuctionType {
  Car = 1,
  Painting = 2,
  Antiquity = 3,
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

export interface AddAuctionInput {
  Title: string;
  StartPrice: number;
  Description: string;
  Type: AuctionType;

  Make?: string;
  Model?: string;
  Year?: string;
  Transmission?: string;
  EngineCapacity?: string;
  Mileage?: string;
  FuelType?: string;

  CountryOfOrigin?: string;
  Field2?: string;
  Field3?: string;
  Field4?: string;
  Field5?: string;

  Artist?: string;
  Medium?: string;
  Dimensions?: string;
}