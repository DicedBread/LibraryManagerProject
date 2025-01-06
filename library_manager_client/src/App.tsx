import { BrowserRouter, Route, Routes } from 'react-router-dom'
import './style/App.css'
import BookContent from './BookPageContent'

function App() {

  return (
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<BookContent />} />
        {/* <Route path='/login' element={<Login />}/> */}
      </Routes>
    </BrowserRouter>
  )
}

export default App
