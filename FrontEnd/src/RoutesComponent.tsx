import React from "react"
import { Route, Routes } from "react-router-dom"
import { AddAuctionPage } from "./pages/AddAuctionPage"
import { AuctionsPage } from "./pages/AuctionsPage"
import { LoginPage } from "./pages/LoginPage"
import { MainPage } from "./pages/MainPage"
import { RegisterPage } from "./pages/RegisterPage"
import { UsersPage } from "./pages/UsersPage"

export const RoutesComponent = () => {

    return (
        <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/users" element={<UsersPage />} />
            <Route path="/mainpage" element={<MainPage />} />
            <Route path="/auctions" element={<AuctionsPage />} />
            <Route path="/add-auction" element={<AddAuctionPage />} />
        </Routes>
    )
}