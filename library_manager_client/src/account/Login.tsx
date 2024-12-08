import React, { ChangeEvent, HtmlHTMLAttributes, useState } from "react";
import "../style/App.css"

interface login{
    username : string;
    password : string;
}

function Login(){
    const [inputs, setInputs] = useState<login>();
    
    const handleSubmit = () => {
        alert("name entered " + inputs?.username);
    }

    const handleChange = (event : ChangeEvent<HTMLInputElement>) => {
        // const valueTarget = event.target  
        // const name = valueTarget.name;
        // const value = valueTarget.value;
        // setInputs(valueTarget => ({...valueTarget, [name]: value}));
        const {name, value} = event.currentTarget;
        setInputs({[name]: value } as Pick<login, keyof login>);
    } 

    return (
        <form className="formBase" onSubmit={handleSubmit}>
            <label htmlFor="username">
                Username
                <input 
                    type="text" 
                    name="username"
                    value={inputs?.username || ""}
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
    );
}

export default Login