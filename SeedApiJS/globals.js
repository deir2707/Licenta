export const url = "https://localhost:5001";

export const fake_user2_id = "00000000-0000-0000-0000-000000000002";
export const fake_user1_id = "00000000-0000-0000-0000-000000000001";

export const headers = {
  "Cache-Control": "no-cache, no-store",
  Pragma: "no-cache",
  "Content-Type": "application/json",
  Accept: "application/json",
  "User-Id": fake_user1_id,
};

export const auctions_endpoint = `${url}/auctions`;
export const users_endpoint = `${url}/users`;
