import './App.css';

import {NavigationBar} from "./components/navigationBar/NavigationBar";
import {Route, Routes} from "react-router-dom";
import {Cars} from "./pages/Cars";
import {Home} from "./pages/Home";
import {Add} from "./pages/Add";
import {Account} from "./pages/Account";
import {Auth} from "./components/auth/Auth";
import {useEffect} from "react";

function App() {
    useEffect(() => {
        const token = sessionStorage.getItem('token');
        if (token) {
            const decodedToken = JSON.parse(atob(token.split('.')[1]));
            if (decodedToken.exp * 1000 < new Date().getTime()) {
                sessionStorage.removeItem('token');
            }
        }
    })

  return (
      <div className="App">
          {sessionStorage.getItem('token') ? (
              <>
              <NavigationBar/>
              <Routes>
                  <Route path='/' element={<Home/>}/>
                  <Route path='/cars' element={<Cars/>}/>
                  <Route path='/add' element={<Add/>}/>
                  <Route path='/account' element={<Account/>}/>
                  <Route path='/auth' element={<Auth/>}/>
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
