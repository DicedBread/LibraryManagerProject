import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './style/App.css'
import BookContent from './BookPageContent'
import Header from './Header';


function App() {

  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path='/' element={<BookContent />} />
        {/* <Route path='/login' element={<Login />}/> */}
      </Routes>
    </BrowserRouter>
  )
}

export default App
