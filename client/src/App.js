import './App.css';

import {NavigationBar} from "./components/navigationBar/NavigationBar";
import {Route, Routes} from "react-router-dom";
import {Cars} from "./pages/Cars";
import {Home} from "./pages/Home";
import {Add} from "./pages/Add";
import {RegistrationForm} from "./components/registrationForm/RegistrationForm";
import {LoginForm} from "./components/loginForm/LoginForm";
import {useState} from "react";
import {Account} from "./pages/Account";

function App() {
    const [token, setToken] = useState("");

    const RegisterHandler  = (token) => {
        sessionStorage.setItem('token', token);
    }

    const LoginHandler = (token) => {
        sessionStorage.setItem('token', token);
        setToken(token);
    };

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
              </Routes>
              </>
          ) : (
              <>
                <RegistrationForm onRegister={RegisterHandler}/>
                <LoginForm onLogin={LoginHandler}/>
              </>
              )}
      </div>
  );
}

export default App;
