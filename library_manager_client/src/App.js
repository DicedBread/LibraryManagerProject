import './style/App.css';
import BookContent from './BookPageContent';
import Header from './Header';
import Login from './login/Login';

function App() {
  return (
    <div className="App">
      <Header/>
      <Login />
      {/* <BookContent/> */}
    </div>
  );
}

export default App;
