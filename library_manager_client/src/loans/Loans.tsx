import React, { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Book, Loan } from "../util/Types";
import { RouteUrl } from "../util/RouteUrl";
import { loginStateContext } from "../account/LoginStateContext";
import { Button, Stack } from "@mui/material";


function Loans(){
    const [userLoans, setUserLoans] = useState<Loan[]>([]); 
    const {LoggedIn, setLoggedIn} = useContext(loginStateContext);
    const nav = useNavigate();


    useEffect(() => {
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
    }, []);

    return (
        <Stack
            direction={"column"}
            spacing={1}
            padding={1}
        >
            {userLoans.length !== 0 ? (
                userLoans.map((loan) => {
                    return <LoanModule loan={loan} />
                })
            ) : (
                <p>no loans</p>
            )}
        </Stack>
    );
}

function LoanModule({loan}: { loan: Loan }){
    return (
        <Stack
            direction={"row"}
            justifyContent={"space-between"}
            border={1} borderRadius={2} padding={1}
        >
            <Stack  spacing={0.2}>
                <h3>{loan.book.title}</h3>
                <p>{loan.book.authour}</p>
            </Stack>
            <Stack spacing={2}>
                <p>{new Date(loan.date).toUTCString()}</p>
                <Button variant="contained" >Return</Button>
            </Stack>

        </Stack>
    ) 
    
}

export default Loans;