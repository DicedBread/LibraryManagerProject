import { BrowserRouter, Route, Routes } from 'react-router-dom'
import BookContent from './BookPageContent'
import Header from './Header';
import Login from './account/Login';
import Register from './account/Register';
import Loans from './loans/Loans'
import { RouteUrl } from './util/RouteUrl';
import { createContext, useState } from 'react';
import { loginStateContext } from './account/LoginStateContext';
import { createTheme, CssBaseline } from '@mui/material';
import { ThemeProvider } from '@emotion/react';
import "./style/index.css"


function App() {
  const [ls, setLs] = useState<boolean>(false);

  const darkTheme = createTheme({
    palette: {
      mode: 'dark',
    },
  });

  return (
    <ThemeProvider theme={darkTheme}>
      <CssBaseline />
      <BrowserRouter>
        <loginStateContext.Provider value={{ LoggedIn: ls, setLoggedIn: setLs }}>
          <Header />
          <Routes>
            <Route path={RouteUrl.Home} element={<BookContent />} />
            <Route path={RouteUrl.Login} element={<Login />} />
            <Route path={RouteUrl.Register} element={<Register />} />
            <Route path={RouteUrl.Loans} element={<Loans />} />
          </Routes>
        </loginStateContext.Provider>
      </BrowserRouter>
    </ThemeProvider>
  )
}

export default App
