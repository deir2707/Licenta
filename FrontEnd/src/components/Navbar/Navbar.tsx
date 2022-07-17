import { Button, Stack } from "@mui/material";
import { Link, useLocation } from "react-router-dom";
import "./Navbar.scss";

const menuItems = [
  {
    name: "Main page",
    path: "/mainpage",
  },
  {
    name: "Auctions",
    path: "/auctions",
  },
  {
    name: "Add auction",
    path: "/add-auction",
  },
  {
    name: "My Profile",
    path: "/my-profile",
  },
];

const getPath = (pathname: string) => {
  const path = pathname.split("/")[1];
  return path;
};

export const Navbar = () => {
  const location = useLocation();
  const pathName = getPath(location.pathname);
  return (
    <Stack direction="row" width="100%" height="100%">
      {menuItems.map((item) => {
        return (
          <Link
            key={item.name}
            to={item.path}
            className={
              "navbarItem " +
              (getPath(item.path) === pathName ? "selected" : "")
            }
          >
            {item.name}
          </Link>
        );
      })}
      <Button className="logoutButton">Logout</Button>
    </Stack>
  );
};
