import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Book, Loan } from "./util/Types";
import { RouteUrl } from "./util/routeUrl";


function Loans(){
    const [userLoans, setUserLoans] = useState<Loan[]>([]); 
    const nav = useNavigate();

    useEffect(() => {
        const url:string = "https://" + import.meta.env.VITE_SERVER_DOMAIN + "/api/loans"
        fetch(url)
        .then((res) => {
            if(res.status == 401){
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
                    return <p>{loan.book.title}</p>
                })
            ) : (
                <p>no loans</p>
            )}
        </div>
    );
}

export default Loans;