import pkg from "lodash";
const { range, sample, random } = pkg;
import fs from "fs";
import FormData from "form-data";
import axios from "axios";
import * as globals from "./globals.js";

const carMakes = ["Mercedes-Benz", "Audi", "Volvo", "BMW", "Toyota", "Renault"];

const carModels = ["C Class", "A4", "S90", "5 Series", "Yaris", "Megane", "A6"];

const transmissions = [
  "Automatic",
  "Manual",
  "Dual Clutch",
  "CVT",
  "Semi-Automatic",
];

const fuelTypes = ["Gasoline", "Diesel", "Hybrid"];

const years = range(2000, 2021);

export async function getAllAuctions() {
  const response = await axios.get(`${globals.auctions_endpoint}/1/9999`, {
    headers: globals.headers,
  });
  return response.data.items;
}

export async function getAuction(auctionId, userId) {
  const response = await axios.get(
    `${globals.auctions_endpoint}/${auctionId}`,
    {
      headers: {
        ...globals.headers,
        "User-Id": userId,
      },
    }
  );
  return response.data;
}

export async function addAuction(user) {
  const make = sample(carMakes);
  const model = sample(carModels);
  const year = sample(years);
  const transmission = sample(transmissions);
  const engineCapacity = random(900, 3000);
  const mileage = random(10000, 300000);
  const fuelType = sample(fuelTypes);

  const otherDetails = {
    make,
    model,
    year,
    transmission,
    engineCapacity,
    mileage,
    fuelType,
  };

  const title = `${make} ${model} ${random(0, 1) === 1 ? year : ""}`;
  const startPrice = random(500, 2000);
  const description = `${make} ${model} ${
    random(0, 1) == 1 ? year : ""
  } is a very good car`;
  const endDate = new Date(
    new Date().getTime() + random(1 * 60 * 60 * 1000, 7 * 24 * 60 * 60 * 1000)
  );
  const formData = new FormData();

  formData.append("Title", title);
  formData.append("StartPrice", startPrice.toString());
  formData.append("Type", "1");
  formData.append("Description", description);
  formData.append("OtherDetails", JSON.stringify(otherDetails));
  formData.append("EndDate", endDate.toJSON());

  formData.append("images", fs.createReadStream(`images/${model}.jpg`));

  const addAuctionHeaders = {
    ...globals.headers,
    "Content-Type": "multipart/form-data",
    "User-Id": user.id,
  };

  const response = await axios.post(
    `${globals.auctions_endpoint}/add-auction`,
    formData,
    { headers: addAuctionHeaders }
  );

  return response.data;
}

export async function makeBid(user, auctionId) {
  const auction = await getAuction(auctionId, user.id);
  const bid = random(auction.currentPrice + 1, auction.currentPrice + 100);

  try {
    const response = await axios.post(
      `${globals.auctions_endpoint}/make-bid`,
      {
        bidAmount: bid,
        auctionId: auction.id,
      },
      {
        headers: {
          ...globals.headers,
          "User-Id": user.id,
        },
      }
    );
    return response.data;
  } catch (error) {
    // console.log(error);
  }
}
