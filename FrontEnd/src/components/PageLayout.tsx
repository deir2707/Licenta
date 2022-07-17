import { Container, Grid } from "@mui/material";
import React from "react";
import { Navbar } from "./Navbar/Navbar";

interface PageLayoutProps {
  children?: any;
  isLoading?: boolean;
}

export const PageLayout = (props: PageLayoutProps) => {
  return (
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <Navbar />
      </Grid>
      <Grid item xs={12}>
        <Container >{props.children}</Container>
      </Grid>
    </Grid>
  );
};
