import React from 'react';
import './App.scss';
import { RoutesComponent } from './RoutesComponent';
import { BrowserRouter as Router } from 'react-router-dom';
import { UserContextProvider } from './context/UserContext';

function App() {
  return (
    <UserContextProvider>
      <Router>
        <RoutesComponent />
      </Router>
    </UserContextProvider>
  )
}

export default App;
