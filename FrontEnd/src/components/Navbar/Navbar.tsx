import { Button, Stack } from "@mui/material";
import { useCallback } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
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
  const navigate = useNavigate();

  const handleLogout = useCallback(() => {
    localStorage.removeItem("balance");
    localStorage.removeItem("email");
    localStorage.removeItem("firstName");
    localStorage.removeItem("fullName");
    localStorage.removeItem("lastName");
    localStorage.removeItem("userId");

    navigate("/");
  }, [navigate]);

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
      <Button className="logoutButton" onClick={handleLogout}>
        Logout
      </Button>
    </Stack>
  );
};
