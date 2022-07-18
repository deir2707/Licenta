import { NotificationEvents } from "./NotificationEvents";

export interface Notification {
  id: string;
  event: NotificationEvents;
  message: string;
  data: any;
}