import { useState } from "react";

export const LoginForm = ({onLogin}) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async () => {
        if (email && password) {
            const loginRequest = {
                email,
                password,
            };

            const response = await fetch("https://localhost:7299/Auth/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(loginRequest),
            });

            const data = await response.json();
            onLogin(data.token);
        }
    };

        return (
            <div>
                <h1>Login</h1>
                <form action="#">
                    <div>
                        <input
                            type="email"
                            name="email"
                            placeholder="Email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>
                    <div>
                        <input
                            type="password"
                            name="password"
                            placeholder="Password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>
                    <div>
                        <button type="button" onClick={handleLogin} style={{marginTop: "10px"}}>
                            Login
                        </button>
                    </div>
                </form>
            </div>
        );
}
