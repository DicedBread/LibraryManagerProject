import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './style/App.css'
import BookContent from './BookPageContent'
import Header from './Header';
import Login from './account/Login';
import Register from './account/Register';
import Loans from './Loans'
import { RouteUrl } from './util/RouteUrl';


function App() {

  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path={RouteUrl.Home} element={<BookContent />} />
        <Route path={RouteUrl.Login} element={<Login />}/>
        <Route path={RouteUrl.Register} element={<Register />}/>
        <Route path={RouteUrl.Loans} element={<Loans />}/>
      </Routes>
    </BrowserRouter>
  )
}

export default App
