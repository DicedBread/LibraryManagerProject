import React, { ChangeEvent, HtmlHTMLAttributes, useState } from "react";
import "../style/App.css"

interface Login{
    email : string;
    password : string;
}

function Login(){
    const [inputs, setInputs] = useState<Login>({ email: "", password: "" });
    
    const handleSubmit = () => {
        // let url : string = process.env.REACT_APP_SERVER_URL + "/api/login'"  
        // fetch(url, {
        //     method: 'POST',
        //     headers: {
        //         'Content-Type': 'application/json',
        //         'email': inputs.email,
        //         'password': inputs.password,
        //     }
        // })
        // .then(response => response.json())
        // .then(data => {
        //     console.log('Success:', data);
        // })
        // .catch((error) => {
        //     console.error('Error:', error);
        // });
    }

    const handleChange = (event : ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target
        setInputs(prevInputs => ({...prevInputs, [name]: value}));
    } 

    return (
        <div className="formBase">
        <form className="" onSubmit={handleSubmit}>
            <label htmlFor="email">
                Username
                <input 
                    type="text" 
                    name="email"
                    value={inputs?.email || ""}
                    onChange={handleChange}
                />
            </label>

            <label htmlFor="password">
                Password
                <input 
                    type="password"
                    name="password"
                    value={inputs?.password || ""}
                    onChange={handleChange}    
                />
            </label>
            <button type="submit">Login</button>
        </form>
        </div>
    );
}

export default Login