import React, { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Book, Loan } from "../util/Types";
import { RouteUrl } from "../util/RouteUrl";
import { loginStateContext } from "../account/LoginStateContext";
import { Button, Stack } from "@mui/material";
import { scryRenderedDOMComponentsWithClass } from "react-dom/test-utils";


function Loans(){
    const [userLoans, setUserLoans] = useState<Loan[]>([]); 
    const {LoggedIn, setLoggedIn} = useContext(loginStateContext);
    const nav = useNavigate();

    const GetLoans = () => {
        const url:string = import.meta.env.VITE_SERVER_DOMAIN + "/api/loans"
        fetch(url)
        .then((res) => {
            console.log(res.status);
            if(res.status == 401){
                setLoggedIn(false);
                nav(RouteUrl.Login);
                return [];
            }
            return res.json()
        })
        .then((data) => setUserLoans(data))
        .catch((err) => console.log(err));
    }

    useEffect(() => {
        GetLoans();
    }, []);

    const HandleReturn = (id: number) => {
        const url:string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Loans/loan/${id}`;
        fetch(url, {method: "Delete"})
        .then((res) => {
            switch(res.status){
                case 200:
                        GetLoans();
                    break;
                case 400:
                    break;
                case 401:
                    setLoggedIn(false);
                    nav(RouteUrl.Login);
                    break;
                case 403:
                    break;
            }
        })
        .catch((err) => {
            console.log(err);
        })

    }

    return (
        <Stack
            direction={"column"}
            spacing={1}
            padding={1}
        >
            {userLoans.length !== 0 ? (
                userLoans.map((loan) => {
                    return (
                        <Stack
                        direction={"row"}
                        justifyContent={"space-between"}
                        border={1} borderRadius={2} padding={1}
                    >
                        <Stack spacing={0.2}>
                            <h3>{loan.book.title}</h3>
                            <p>{loan.book.author}</p>
                        </Stack>
                        <Stack spacing={2}>
                            <p>{new Date(loan.date).toUTCString()}</p>
                            <Button onClick={() => HandleReturn(loan.loanId)} variant="contained" >Return</Button>
                        </Stack>
                    </Stack>
                    )
                })
            ) : (
                <p>no loans</p>
            )}
        </Stack>
    );
}


export default Loans;