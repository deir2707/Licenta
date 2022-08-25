import { useCallback, useMemo, useState } from "react";
import { Button, Stack } from "@mui/material";
import { useNavigate } from "react-router-dom";

import { PageLayout } from "../../components/PageLayout";
import { MyAuctions, MyProfileDetails, WonAuctions } from "./Components";
import "./MyProfilePage.scss";

export const MyProfilePage = () => {
  const id = localStorage.getItem("userId");

  const navigate = useNavigate();
  const [selectedSubMenu, setSelectedSubMenu] = useState("details");

  const handleSubmenuClick = useCallback((e: any) => {
    setSelectedSubMenu(e.currentTarget.id);
  }, []);

  const isFinishedClass = useCallback(
    (name: string) => {
      return selectedSubMenu === name ? " selected" : "";
    },
    [selectedSubMenu]
  );

  const pageContent = useMemo(() => {
    switch (selectedSubMenu) {
      case "details":
        return <MyProfileDetails />;

      case "my-auctions":
        return <MyAuctions />;

      case "won-auctions":
        return <WonAuctions />;
    }
  }, [selectedSubMenu]);

  if (!id) {
    return (
      <div>
        <div>You are not logged in</div>
        <Button
          variant="contained"
          color="primary"
          onClick={() => {
            navigate("/");
          }}
        >
          Go to Login Page
        </Button>
      </div>
    );
  }

  return (
    <PageLayout>
      <div id="my-profile-page">
        <Stack className="sub-menu" direction="row" width="100%">
          <div
            id={"details"}
            onClick={handleSubmenuClick}
            className={`option${isFinishedClass("details")}`}
          >
            My details
          </div>
          <div
            id={"my-auctions"}
            onClick={handleSubmenuClick}
            className={`option${isFinishedClass("my-auctions")}`}
          >
            My Auctions
          </div>
          <div
            id={"won-auctions"}
            onClick={handleSubmenuClick}
            className={`option${isFinishedClass("won-auctions")}`}
          >
            Won Auctions
          </div>
        </Stack>
        {pageContent}
      </div>
    </PageLayout>
  );
};
