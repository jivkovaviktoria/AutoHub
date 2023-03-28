import './App.css';
import {Route, Routes} from "react-router-dom";
import {useEffect} from "react";

import {NavigationBar, Auth} from "./components";
import {Cars, Home, Add, Account} from "./pages";

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
