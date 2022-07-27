export interface BidDetails {
  id: string;
  amount: number;
  date: Date;
  bidderName: string;
}

export interface BidInput {
  auctionId: string;
  bidAmount: number;
  date: Date;
}