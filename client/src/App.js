import './App.css';

import {NavigationBar} from "./components/navigationBar/NavigationBar";
import {Route, Routes} from "react-router-dom";
import {Cars} from "./pages/Cars";
import {Home} from "./pages/Home";
import {Add} from "./pages/Add";

function App() {
  return (
      <div className="App">
        <NavigationBar/>

        <Routes>
          <Route path='/' element={<Home/>}/>
          <Route path='/cars' element={<Cars/>}/>
          <Route path='/add' element={<Add/>}/>
        </Routes>
      </div>
  );
}

export default App;
