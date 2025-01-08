import React, {
    ChangeEvent,
    ChangeEventHandler,
    FormEvent,
    useState,
} from "react";
import "../style/App.css";
import "../style/Form.css";
import { useNavigate } from "react-router-dom";

interface Register {
    email: string;
    userName: string;
    password: string;
}

function Register() {
    const [inputs, setInputs] = useState<Register>({ email: "", userName: "", password: "" });
    const [failedRegister, setFailedRegister] = useState<boolean>(false);
    const nav = useNavigate();

    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        const url:string = `https://${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/register`;
        console.log(url)
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'email': inputs.email,
                'password': inputs.password,
                'username': inputs.userName
            },
            credentials: "include"
        })
        .then(response => {
            if (response.ok) {
                nav("/login");
            } else {
                setFailedRegister(true);
            }
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    };

    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target
        setInputs(prevInputs => ({...prevInputs, [name]: value}));
    };

    return (
        <div className="formBase">
            <form onSubmit={handleSubmit} className="">
                <label>
                    Email:
                    <br />
                    <input
                        type="email"
                        name="email"
                        value={inputs?.email || ""}
                        onChange={handleChange}
                        required
                    />
                </label>
                <label>
                    Username:
                    <br />
                    <input
                        type="text"
                        name="userName"
                        value={inputs?.userName || ""}
                        onChange={handleChange}
                        required
                    />
                </label>
                <label>
                    Password:
                    <br />
                    <input
                        type="password"
                        name="password"
                        value={inputs?.password || ""}
                        onChange={handleChange}
                        required
                    />
                </label>
                <button type="submit">Register</button>
            </form>
        </div>
    );
}

export default Register;
