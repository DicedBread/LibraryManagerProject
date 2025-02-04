import React, { Context, createContext, ChangeEvent, FormEvent, HtmlHTMLAttributes, useContext, useState } from "react";
import "../style/App.css"
import "../style/Form.css"
import { redirect, useNavigate } from "react-router-dom";
import { loginStateContext } from "./LoginStateContext";
import { Box, Button, Stack, TextField } from "@mui/material";
import Grid from "@mui/material/Grid2";
import { blue } from "@mui/material/colors";


interface Login {
    email: string;
    password: string;
}

function Login() {
    const [inputs, setInputs] = useState<Login>({ email: "", password: "" });
    const [failedLogin, setFailedLogin] = useState<boolean>(false);
    const nav = useNavigate();
    const { LoggedIn, setLoggedIn } = useContext(loginStateContext);

    const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setFailedLogin(false);
        const url: string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/login`;
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

    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target
        setInputs(prevInputs => ({ ...prevInputs, [name]: value }));
    }

    return (
        <Grid
            container
            direction={"column"}
            justifyContent={"center"}
            padding={20}
            justifyItems={"center"}
            alignItems={"center"}
        >
            <Grid>
                <form onSubmit={handleSubmit}>
                    <Stack
                        border={1}
                        color={"secondary"}
                        padding={5}
                        borderRadius={2}
                        spacing={1}
                    >
                        <TextField
                            color={"primary"}
                            type={"email"}
                            label={"email"} 
                            name={"email"}
                            value={inputs?.email || ""}
                            onChange={handleChange}
                            required
                        />
                        <TextField 
                            type={"password"} 
                            label={"password"} 
                            name={"password"}
                            value={inputs?.password || ""} 
                            onChange={handleChange}
                            required
                        />
                        <Button type="submit" variant="contained">Login</Button>
                    </Stack>

                </form>
            </Grid>
        </Grid>
    );
}

export default Login