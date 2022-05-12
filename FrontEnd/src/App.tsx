import React from 'react';
import logo from './logo.svg';
import './App.css';
import { LoginForm as LoginForm } from './LoginForm';
import { Box, Container, Grid } from '@mui/material';

function App() {
  return (
    <div className="App">
      <Grid
        container
        spacing={0}
        direction="column"
        alignItems="center"
        justifyContent="center"
        style={{ minHeight: '100vh' }}
      >
        <Grid item xs={3}>
          <LoginForm />
        </Grid>
      </Grid>
    </div>
  )
}

export default App;
