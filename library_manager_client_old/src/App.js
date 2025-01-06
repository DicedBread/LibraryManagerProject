import React from 'react';
import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './style/App.css';
import BookContent from './BookPageContent';
import Header from './Header';
import Login from './account/Login';
import Register from './account/Register';

function App() {
  return (
    <BrowserRouter>
    <Header />
      <Routes>
        <Route path='/' element={<BookContent />} />
        <Route path='/login' element={<Login />}/>
      </Routes>
    </BrowserRouter>
  );
}


export default App;
