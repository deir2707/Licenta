import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import PubSub from "pubsub-js";
import { AuctionNotification } from "../../events/AuctionNotification";
import { NotificationEvents } from "../../events/NotificationEvents";
import { PubSubEvents } from "./PubSubEvents";

export class NotificationService {
  connection: HubConnection;
  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl("https://localhost:5001/hub/auctionHub")
      .configureLogging(LogLevel.Error)
      .withAutomaticReconnect()
      .build();

    this.connection.onclose(() => {
      console.log("Connection closed");
    });

    this.connection.on(
      "onPublishMessage",
      (notification: AuctionNotification) => {
        this.onNotificationReceived(notification);
      }
    );

    console.log("Connection started");
  }

  async start() {
    await this.connection.start();
  }

  async stop() {
    await this.connection.stop();
  }

  onNotificationReceived(notification: AuctionNotification) {
    switch (notification.event) {
      case NotificationEvents.AuctionBid:
        PubSub.publish(PubSubEvents.AuctionBid, notification);
        break;
      case NotificationEvents.AuctionFinished:
        PubSub.publish(PubSubEvents.AuctionFinished, notification);
        break;
    }
  }
}

// export const notificationService = new NotificationService();
// notificationService.start();
// export default notificationService;
