import pkg from "lodash";
const { sample, random, range } = pkg;
import { getAllAuctions, addAuction, makeBid } from "./auctions.js";
import { getAllUsers, createUser, addBalance } from "./users.js";

process.env["NODE_TLS_REJECT_UNAUTHORIZED"] = "0";

async function main() {
  await addUsers();
  const users = await getAllUsers();
  await addAuctions(users);
  const auctions = await getAllAuctions();
  await addBids(users, auctions);
}

async function addUsers() {
  for (let index = 0; index < random(500, 1000); index++) {
    const user = await createUser();
    await addBalance(user.id);
  }
}

async function addAuctions(users) {
  for (let index = 0; index < random(1000, 3000); index++) {
    await addAuction(sample(users));
  }
}

async function addBids(users, auctions) {
  for (const auction in auctions) {
    for (let index = 0; index < random(10, 30); index++) {
      await makeBid(sample(users), auctions[auction].id);
    }
  }
}

main();
