export interface BidDetails {
  id: number;
  amount: number;
  date: Date;
  bidderName: string;
}

export interface BidInput {
  auctionId: number;
  bidAmount: number;
  date: Date;
}