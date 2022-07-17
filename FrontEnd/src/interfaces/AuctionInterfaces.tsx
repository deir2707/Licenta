export enum AuctionType {
  Car = 1,
  Painting = 2,
  Vase = 3,
}

export interface AuctionDetails {
  id: number;
  description: string;
  startingPrice: number;
  type: AuctionType;
  otherDetails: { [key: string]: string };
  images: string[];
  startDate: Date;
  endDate: Date;
}
