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
      <Routes>
        <Route path='/' element={<Index />} />
      </Routes>
    </BrowserRouter>
  );
}

function Index(){
  return (
    <div className='app'>
      <Header />
      <BookContent />
    </div>
  );
}

export default App;
