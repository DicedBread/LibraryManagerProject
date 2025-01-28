import React, { Context, createContext, ChangeEvent, FormEvent, HtmlHTMLAttributes, useContext, useState } from "react";
import "../style/App.css"
import "../style/Form.css"
import { redirect, useNavigate } from "react-router-dom";
import { loginStateContext } from "./LoginStateContext";



interface Login{
    email : string;
    password : string;
}

function Login(){
    const [inputs, setInputs] = useState<Login>({ email: "", password: "" });
    const [failedLogin, setFailedLogin] = useState<boolean>(false);
    const nav = useNavigate();
    const {LoggedIn, setLoggedIn} = useContext(loginStateContext);
    
    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setFailedLogin(false);
        const url:string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/login`;
        console.log(url)
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'email': inputs.email,
                'password': inputs.password,
            },
            credentials: "include"
        })
        .then(response => {
            if (response.ok) {
                setLoggedIn(true);
                nav(-1);
            } else {
                setLoggedIn(false);
                setFailedLogin(true);
            }
        })
        .catch((error) => {
            console.error('Error:', error);
        });
    }

    const handleChange = (event : ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target
        setInputs(prevInputs => ({...prevInputs, [name]: value}));
    } 

    return (
        <div className="formBase">
        <form className="" onSubmit={handleSubmit}>
            <label htmlFor="email" className="formlabel">
                Email <br />
                <input 
                    type="text" 
                    name="email"
                    value={inputs?.email || ""}
                    onChange={handleChange}
                    required
                />
            </label>

            <label htmlFor="password" className="formlabel">
                Password <br /> 
                <input 
                    type="password"
                    name="password"
                    value={inputs?.password || ""}
                    onChange={handleChange}    
                    required
                />
            </label>

            {failedLogin && (
                <p>Invalid email or password</p>
            )}
            <button type="submit">Login</button>
        </form>
        </div>
    );
}

export default Login