import React, { Context, useContext, useEffect, useState } from "react";
import "./style/App.css"
import { useNavigate } from "react-router-dom";
import Cookies from 'js-cookie';
import { ApiRouteUrl, RouteUrl } from "./util/RouteUrl";
import { loginStateContext } from "./account/LoginStateContext";




function Header(){
    const nav = useNavigate();
    const {LoggedIn, setLoggedIn} = useContext(loginStateContext);

    const logout = () => {
        const url = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/Logout`; 
        fetch(url, {method: 'POST'})
        .then(res => {
            switch (res.status){
                case 401:
                    setLoggedIn(false);
                    break;
                case 200:
                    Cookies.remove(".AspNetCore.Cookies")
                    setLoggedIn(false);
                    break;
                default:
                    break;
            }
        })
        .catch(error => {
            console.log(error);
        })
    }

    return (
        <div className="header">
            <h1>Library</h1>

            <div className="nav">
                {LoggedIn ? (
                    <>
                        <button onClick={logout}>Logout</button>
                        <button onClick={() => nav(RouteUrl.Loans)}>Loans</button>
                    </>
                ) : (
                    <button onClick={() => nav(RouteUrl.Login)}>Login</button>
                )}
                <button onClick={() => nav(RouteUrl.Home)}>Home</button>
            </div>
        </div>
    );  
}

export default Header


