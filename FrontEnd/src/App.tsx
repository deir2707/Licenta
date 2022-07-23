import React, { useEffect, useState } from "react";
import "./App.scss";
import { RoutesComponent } from "./RoutesComponent";
import { BrowserRouter as Router } from "react-router-dom";
import { UserContextProvider } from "./context/UserContext";
import { NotificationService } from "./services/notifications/NotificationService";

function App() {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [notificationService, setNotificationService] =
    useState<NotificationService>();

  useEffect(() => {
    const startNotificationService = async () => {
      const notificationService = new NotificationService();
      await notificationService.start();
      setNotificationService(notificationService);

      return () => {
        notificationService.stop();
      };
    };

    startNotificationService();
  }, []);

  return (
    <UserContextProvider>
      <Router>
        <RoutesComponent />
      </Router>
    </UserContextProvider>
  );
}

export default App;
