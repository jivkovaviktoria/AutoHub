import { useState } from "react";

export const RegistrationForm = () => {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const registerUser = async () => {
        if (email && username && password) {
            const regRequest = {
                email,
                username,
                password,
            };

            const response = await fetch("https://localhost:7299/Auth/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(regRequest),
            });

            const data = await response.json();
            console.log(data);
        }
    };

    return (
        <div>
            <h1>Register</h1>
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
                <input
                    type="username"
                    name="username"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
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
                    <button
                        type="button"
                        onClick={registerUser}
                        style={{ marginTop: "10px" }}
                    >
                        Submit
                    </button>
                </div>
            </form>
        </div>
    );
};