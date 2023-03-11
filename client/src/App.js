import './App.css';

import {NavigationBar} from "./components/navigationBar/NavigationBar";
import {Route, Routes} from "react-router-dom";
import {Cars} from "./pages/Cars";

function App() {
  return (
      <div className="App">
        <NavigationBar/>

        <Routes>
          <Route path='/' element={<h1>Home</h1>}/>
          <Route path='/cars' element={<Cars/>}/>
        </Routes>
      </div>
  );
}

export default App;
