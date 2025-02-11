import React, { Context, useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Cookies from 'js-cookie';
import { ApiRouteUrl, RouteUrl } from "../util/RouteUrl";
import { loginStateContext } from "../account/LoginStateContext";
import { Box, Button, Stack } from "@mui/material";




function Header(){
    const nav = useNavigate();
    const {LoggedIn, setLoggedIn} = useContext(loginStateContext);

    const logout = () => {
        const url = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/Logout`; 
        fetch(url, {method: 'POST'})
        .then(res => {
            switch (res.status){
                case 401 | 403:
                    setLoggedIn(false);
                    break;
                case 200:
                    Cookies.remove(".AspNetCore.Cookies")
                    setLoggedIn(false);
                    nav(RouteUrl.Home);
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
        <Stack 
            direction="row"
            justifyContent={"space-between"}
            alignItems={"center"}
            spacing={2}
            sx={{
                minHeight: "10vh",
                padding: "0 10px",
                }}>

            <h1>Library</h1>
            <Stack
                direction={"row"}
                spacing={{sm:1, md:2 }}
                    >
                {LoggedIn ? (
                        <>
                            <Button variant="contained" onClick={logout}>Logout</Button>
                            <Button variant="contained" onClick={() => nav(RouteUrl.Loans)}>Loans</Button>
                        </>
                    ) : (
                        <>
                            <Button variant="contained" onClick={() => nav(RouteUrl.Register)}>Register</Button>
                            <Button variant="contained" onClick={() => nav(RouteUrl.Login)}>Login</Button>
                        </>
                    )}    
            
                <Button variant="contained" onClick={() => nav(RouteUrl.Home)}>Home</Button>
                
            </Stack>
        </Stack> 
    );  
}

export default Header


