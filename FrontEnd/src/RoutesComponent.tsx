import React from "react"
import { Route, Routes } from "react-router-dom"
import { AddAuctionPage } from "./pages/auctions/AddAuction/AddAuctionPage";
import { ViewAuctionDetails } from "./pages/auctions/ViewAuctionDetails/ViewAuctionDetails";
import { AuctionsPage } from "./pages/auctions/ViewAuctions/AuctionsPage";
import { LoginPage } from "./pages/LoginPage";
import { MainPage } from "./pages/MainPage";
import { RegisterPage } from "./pages/RegisterPage";
import { UsersPage } from "./pages/UsersPage";

export const RoutesComponent = () => {
  return (
    <Routes>
      <Route path="/" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/users" element={<UsersPage />} />
      <Route path="/mainpage" element={<MainPage />} />
      <Route path="/auctions" element={<AuctionsPage />} />
      <Route path="/auctions/:id" element={<ViewAuctionDetails />} />
      <Route path="/add-auction" element={<AddAuctionPage />} />
    </Routes>
  );
};