import React, { useState } from "react";
import "./style/App.css"
import { useNavigate } from "react-router-dom";
import { useCookies } from "react-cookie";

function Header(){
    const [cookies] = useCookies();
    // const [isLoggedIn, setIsloggedIn] = useState<boolean>(!!cookies['.AspNetCore.Cookies']);
    const nav = useNavigate();
    
    return (
        <div className="header">
            <h1>Library</h1>
            {/* <p>{cookies[".AspNetCore.Cookies"] || "sadklajnsdklm" }</p> */}

            

            <div className="nav">
                <button onClick={() => nav("/")}>Home</button>
                {/* {isLoggedIn && (
                    <p>yeo</p>
                )} */}
                <button onClick={() => nav("/login")}>Login</button>
            </div>
        </div>
    );
}

export default Header