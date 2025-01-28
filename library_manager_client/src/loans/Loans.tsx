import React, { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Book, Loan } from "../util/Types";
import { RouteUrl } from "../util/RouteUrl";
import { loginStateContext } from "../account/LoginStateContext";


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
        <div className="loanContent">
            {userLoans.length !== 0 ? (
                userLoans.map((loan) => {
                    return <LoanModule loan={loan} />
                })
            ) : (
                <p>no loans</p>
            )}
        </div>
    );
}

function LoanModule({loan}: { loan: Loan }){
    return (
        <div className="loanModule">
            <div>
                <p>{loan.book.title}</p>
                <p>{loan.book.authour}</p>
            </div>
            <div>
                <p>Date: {new Date(loan.date).toUTCString()}</p>
            </div>
        </div>
    ) 
    
}

export default Loans;