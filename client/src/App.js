import './App.css';

import {NavigationBar} from "./components/navigationBar/NavigationBar";
import {Route, Routes} from "react-router-dom";
import {Cars} from "./pages/Cars";
import {Home} from "./pages/Home";
import {Add} from "./pages/Add";
import {RegistrationForm} from "./components/registrationForm/RegistrationForm";
import {LoginForm} from "./components/loginForm/LoginForm";
import {useState} from "react";

function App() {
    const [token, setToken] = useState("");

    const RegisterHandler  = (token) => {
        localStorage.setItem('token', token);
    }

    const LoginHandler = (token) => {
        localStorage.setItem("token", token);
        setToken(token);
    };

  return (
      <div className="App">
          {localStorage.getItem('token') ? (
              <>
              <NavigationBar/>
              <Routes>
                  <Route path='/' element={<Home/>}/>
                  <Route path='/cars' element={<Cars/>}/>
                  <Route path='/add' element={<Add/>}/>
              </Routes>
              </>
          ) : (
              <>
                <RegistrationForm onRegister={RegisterHandler}/>
                <LoginForm onLogin={LoginHandler}/>
              </>
              )};
      </div>
  );
}

export default App;
