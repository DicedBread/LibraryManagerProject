import React from 'react';
import './style/App.css';
import BookContent from './BookPageContent';
import Header from './Header';
import Login from './account/Login';
import Register from './account/Register';

function App() {
  return (
    <div className="App">
      <Header />
      <Login />
      {/* <Register /> */}
      {/* <BookContent/> */}
    </div>
  );
}

export default App;
