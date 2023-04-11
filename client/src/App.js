import './App.css';
import {Route, Routes} from "react-router-dom";
import {useEffect} from "react";

import {NavigationBar, Auth} from "./components";
import {Cars, Home, Add, Account} from "./pages/index";
import {Car} from "./pages/car/Car";

function App() {
    useEffect(() => {
        const token = sessionStorage.getItem('token');
        if (token) {
            const decodedToken = JSON.parse(atob(token.split('.')[1]));
            if (decodedToken.exp * 1000 < new Date().getTime()) sessionStorage.removeItem('token');
        }
    });

  return (
      <div className="App">
          {sessionStorage.getItem('token') ? (
              <>
              <NavigationBar/>
              <Routes>
                  <Route path='/' element={<Home/>}/>
                  <Route path='cars/Cars' element={<Cars/>}/>
                  <Route path='add/Add' element={<Add/>}/>
                  <Route path='account/Account' element={<Account/>}/>
                  <Route path='auth/Auth' element={<Auth/>}/>
              </Routes>
              </>
          ) : (
              <div>
                <Auth />
              </div>
              )}
      </div>
  );
}

export default App;
