import pkg from "lodash";
const { range, sample, random } = pkg;
import axios from "axios";
import * as globals from "./globals.js";
import { uniqueNamesGenerator, names, starWars } from "unique-names-generator";

export async function getAllUsers() {
  const response = await axios.get(`${globals.users_endpoint}/get-all`, {
    headers: globals.headers,
  });

  return response.data.filter(
    (u) =>
      u["id"] !== globals.fake_user1_id && u["id"] !== globals.fake_user2_id
  );
}

export async function addDefaultUsers() {
  const user1 = {
    Id: "00000000-0000-0000-0000-000000000001",
    Email: "user1@email.com",
    Password: "password",
    FullName: "User1",
  };
  const user2 = {
    Id: "00000000-0000-0000-0000-000000000002",
    Email: "user2@email.com",
    Password: "password2",
    FullName: "User2",
  };
  try {
    const response = await axios.post(
      `${globals.users_endpoint}/register`,
      user1,
      {
        headers: globals.headers,
      }
    );

    console.log(response.data);
  } catch (e) {
    console.log(e)
  }
  try {
    const response2 = await axios.post(
      `${globals.users_endpoint}/register`,
      user2,
      {
        headers: globals.headers,
      }
    );
    console.log(response2.data);
  } catch (e) {
    console.log(e)
  }
}

export async function createUser() {
  const fullName = uniqueNamesGenerator({
    dictionaries: [names, names],
    length: 2,
    separator: " ",
  });
  const password = generatePassword();
  const email = `${fullName.toLowerCase().replace(" ", "_")}@${sample([
    "gmail.com",
    "yahoo.com",
    "outlook.com",
  ])}`;

  const response = await axios.post(`${globals.users_endpoint}/register`, {
    fullName,
    email,
    password,
  });

  return response.data;
}

export async function addBalance(userId) {
  const BalanceToAdd = random(50000, 100000);
  const response = await axios.post(
    `${globals.users_endpoint}/add-balance`,
    {
      BalanceToAdd,
    },
    {
      headers: {
        ...globals.headers,
        "User-Id": userId,
      },
    }
  );
}

function generatePassword() {
  var length = 8,
    charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
    retVal = "";
  for (var i = 0, n = charset.length; i < length; ++i) {
    retVal += charset.charAt(Math.floor(Math.random() * n));
  }
  return retVal;
}
