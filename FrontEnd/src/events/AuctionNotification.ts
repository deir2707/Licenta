import { AuctionFinishedNotification } from "./AuctionFinishedNotification";
import { BidNotification } from "./BidNotification";
import { NotificationEvents } from "./NotificationEvents";

export interface AuctionNotification {
  id: string;
  event: NotificationEvents;
  message: string;
  data: AuctionFinishedNotification | BidNotification;
}
