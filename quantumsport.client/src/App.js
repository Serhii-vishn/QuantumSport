import './App.scss';
import AppRoutes from "./AppRoutes";
import Header from './Components/Header/Header.jsx';
import Footer from './Components/Footer/Footer.jsx';

function App() {
  return (
    <div className="App">
      <Header/>
      <AppRoutes/>
      <Footer/>
    </div>
  );
}

export default App;
