import React, { useState } from "react";
import "../style/App.css"

function Login(){
    const [inputs, setInputs] = useState({});
    
    const handleSubmit = () => {
        alert("name entered " + inputs.username);
    }

    const handleChange = (event) => {
        const name = event.target.name;
        const value = event.target.value;
        setInputs(values => ({...values, [name]: value}));
    } 

    return (
        <form className="loginForm" onSubmit={handleSubmit}>
            <label htmlFor="username">
                Username
                <input 
                    type="text" 
                    name="username"
                    value={inputs.username || ""}
                    onChange={handleChange}
                />
            </label>

            <label htmlFor="password">
                Password
                <input 
                    type="password"
                    name="password"
                    value={inputs.password || ""}
                    onChange={handleChange}    
                />
            </label>
            <button type="submit">Login</button>
        </form>
    );
}

export default Login