import React, { useContext } from "react";
import { useState, useEffect } from "react";
import { Book, Loan } from "./util/Types";
import Grid from "@mui/material/Grid2";
import { Box, Button, Stack } from "@mui/material";
import { useTheme } from '@mui/material/styles';
import { loginStateContext } from "./account/LoginStateContext";
import { RouteUrl } from "./util/RouteUrl";
import { useNavigate } from "react-router-dom";



function BookContent() {
    const [books, setBooks] = useState<Book[]>([]);

    useEffect(() => {
        const url:string = import.meta.env.VITE_SERVER_DOMAIN + "/api/Books?limit=20&offset=0"
        fetch(url)
            .then((res) => {return res.json();})
            .then((data) => {setBooks(data);})
            .catch((err) => {console.log("failed to access books ", err)});
    }, []);

    return (
        <Grid container spacing={1} padding={"10px"}>
            {books.length !== 0 ? (
                books.map((book) => {
                    return <BookModule book={book} />;
                })
            ) : (
                <p>No books available</p>
            )}
        </Grid>
    );
}

function BookModule({book}: {book: Book}){
    const theme = useTheme();
    const { LoggedIn, setLoggedIn } = useContext(loginStateContext);
    const nav = useNavigate();

    const HandleLoan = (isbn: string) => {
        if(!LoggedIn) {
            nav(RouteUrl.Login); 
            return;
        }

        const url:string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Loans/loan/${book.id}`;
        fetch(url, {method: "POST"})
        .then((res) => {
            switch (res.status){
                case 200:
                    res.json()
                    break;
                case 403:
                    // already loaned
                    break;
                case 401:
                    setLoggedIn(false);
                    nav(RouteUrl.Login);
                    break;
            }
            
        })
    }

    return (
        <Grid size={{xs:12, sm: 12, md: 6, lg: 4, xl: 3}}>
            <Stack 
                direction={"row"}
                padding={2}
                border={1}
                borderRadius={2}
                spacing={2}
            >
                <Box 
                    sx={{
                        height: "100xp",
                        width: "100px",
                        display: "flex",
                        justifyContent: "center",
                        alignContent: "center",
                        flexShrink: 0,   
                        img: {
                            height: "100px",
                            width: "auto"
                        }
                    }} 
                
                >
                    <img src={book.imgUrl} alt={book.title + " book image"} />
                </Box>
                <Stack flexGrow={1} alignItems={"flex-start"} justifyContent={"space-between"}>
                    <Box 
                        display={"inline-grid"}
                        sx={{
                            h4: {
                                overflow: "hidden",
                                textOverflow: "ellipsis",
                                whiteSpace: "nowrap"
                            },
                        }} 
                        >
                        <h4>{book.title}</h4>
                        <p>{book.authour}</p>
                    </Box>
                    <Stack alignSelf={"flex-end"}>
                        <Button onClick={() => HandleLoan(book.id)} variant={"contained"} sx={{width: "100px"}}>Loan</Button>
                    </Stack>
                </Stack>
            </Stack>
        </Grid>
    )
}



export default BookContent;

