import { useState } from "react";
import styles from "./Auth.module.css";
import {useNavigate} from "react-router-dom";
import * as UserService from '../../services/UserService';

export const Auth = () => {
    const navigate = useNavigate();
    const [isSignIn, setIsSignIn] = useState(true);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [username, setUsername] = useState("");

    const handleSignIn = async () => {
        if (email && password) {
            const loginRequest = {
                email,
                password,
            };

            const data = await UserService.SignIn(loginRequest);

            sessionStorage.setItem('token', data.token);
            navigate('/');
            window.location.reload();
        }
    };

    const handleSignUp = async () => {
        if (email && password && username) {
            const registerRequest = {
                email,
                password,
                username,
            };

            const data = await UserService.SignUp(registerRequest);
        }
    };

    return (
        <div className={styles["Auth-container"]}>
            <div className={styles["Auth-card"]}>
                <h1 className={styles["Auth-header"]}>
                    {isSignIn ? "Sign In" : "Sign Up"}
                </h1>
                <form>
                    <input
                        type="email"
                        placeholder="Email"
                        className={styles["Auth-input"]}
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    {!isSignIn && (
                        <input
                            type="text"
                            placeholder="Username"
                            className={styles["Auth-input"]}
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                    )}
                    <input
                        type="password"
                        placeholder="Password"
                        className={styles["Auth-input"]}
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <button type="button" className={styles["Auth-button"]} onClick={isSignIn ? handleSignIn : handleSignUp}>
                        {isSignIn ? "Sign In" : "Sign Up"}
                    </button>
                </form>
                <p>
                    {isSignIn
                        ? "Don't have an account?"
                        : "Already have an account?"}{" "}
                    <button className={styles["Auth-switch"]} onClick={() => setIsSignIn(!isSignIn)}>
                        {isSignIn ? "Sign Up" : "Sign In"}
                    </button>
                </p>
            </div>
        </div>
    );
};
