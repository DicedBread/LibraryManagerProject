import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './style/App.css'
import BookContent from './BookPageContent'
import Header from './Header';
import Login from './account/Login';
import Register from './account/Register';
import Loans from './loans/Loans'
import { RouteUrl } from './util/RouteUrl';
import { createContext, useState } from 'react';
import { loginStateContext } from './account/LoginStateContext';



function App() {
  const [ls, setLs] = useState<boolean>(false);

  return (    
    <BrowserRouter>
      <loginStateContext.Provider value={{ LoggedIn: ls, setLoggedIn: setLs }}>
        <Header />
        <Routes>
          <Route path={RouteUrl.Home} element={<BookContent />} />
          <Route path={RouteUrl.Login} element={<Login />}/>
          <Route path={RouteUrl.Register} element={<Register />}/>
          <Route path={RouteUrl.Loans} element={<Loans />}/>
        </Routes>
      </loginStateContext.Provider>
    </BrowserRouter>
  )
}

export default App
