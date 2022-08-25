import { AuctionDetails } from "./AuctionInterfaces";

export interface UserStatistics {
  fullName: string;
  auctions: number;
  money: number;
}

export interface StatisticsOutput {
  mostActiveSeller: UserStatistics;
  mostActiveBuyer: UserStatistics;
  mostExpensiveItem: AuctionDetails;
  mostExpensiveItemSoldByCurrentUser: AuctionDetails;
  mostExpensiveItemBoughtByCurrentUser: AuctionDetails;
}
