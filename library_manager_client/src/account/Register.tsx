import React, {
    ChangeEvent,
    ChangeEventHandler,
    FormEvent,
    useState,
} from "react";
import { useNavigate } from "react-router-dom";
import { Button, Stack, TextField } from "@mui/material";
import Grid from "@mui/material/Grid2";


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
        const url:string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Account/register`;
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
                        error={failedRegister}
                    />
                    <TextField
                        color={"primary"}
                        type={"userName"}
                        label={"username"} 
                        name={"userName"}
                        value={inputs?.userName || ""}
                        onChange={handleChange}
                        required
                        error={failedRegister}
                    />
                    <TextField 
                        type={"password"} 
                        label={"password"} 
                        name={"password"}
                        value={inputs?.password || ""} 
                        onChange={handleChange}
                        required
                        error={failedRegister}
                    />
                    <Button type="submit" variant="contained">Register</Button>
                </Stack>
            </form>
        </Grid>
    </Grid>
    );
}

export default Register;
