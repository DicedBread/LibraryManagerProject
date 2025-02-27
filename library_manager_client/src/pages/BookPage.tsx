import React, { ChangeEvent, useContext } from "react";
import { useState, useEffect } from "react";
import { Book, Loan } from "../util/Types";
import Grid from "@mui/material/Grid2";
import { Box, Button, IconButton, Stack, TextField } from "@mui/material";
import { useTheme } from '@mui/material/styles';
import { loginStateContext } from "../account/LoginStateContext";
import { RouteUrl } from "../util/RouteUrl";
import { useNavigate } from "react-router-dom";
import ClearIcon from '@mui/icons-material/Clear';
import CancelIcon from '@mui/icons-material/Cancel';

function BookContent() {
    const [books, setBooks] = useState<Book[]>([]);
    const [searchRes, setSearchRes] = useState<Book[]>([]);
    const [searchInput, setSearchInput] = useState<string>("");
    const [displaySearch, setDisplaySearch] = useState<boolean>(false);

    useEffect(() => {
        const url: string = import.meta.env.VITE_SERVER_DOMAIN + "/api/Books?limit=20&offset=0"
        fetch(url)
            .then((res) => { return res.json(); })
            .then((data) => { setBooks(data); })
            .catch((err) => { console.log("failed to access books ", err) });
    }, []);

    const HandleSearch = (event: ChangeEvent<HTMLInputElement>) => {
        const search = event.currentTarget.value;
        setSearchInput(search);
        if(search.length <= 0) {
            setDisplaySearch(false);
            return
        };
        setDisplaySearch(true);
        // const searchCleaned = search.replace(" ", "+");
        const url: string = encodeURI(import.meta.env.VITE_SERVER_DOMAIN + `/api/Books?search=${search}`)
        fetch(url)
        .then(res => {
            if(res.status == 200){
                return res.json();
            }
        })
        .then(data => {
            setSearchRes(data);
        })
        .catch(err => {console.log(err)});
    }

    const ClearSearch = () => {
        setDisplaySearch(false);
        setSearchInput("");
    }

    return (
        <>
            <Stack direction={"row"} paddingLeft={1}>
                <TextField
                    onChange={HandleSearch}
                    type={"text"}
                    name={"search"}
                    value={searchInput}
                    label={"Search"}
                    sx={{
                        ".css-5j1080-MuiInputBase-root-MuiOutlinedInput-root": {
                            borderTopRightRadius: 0,
                            borderBottomRightRadius: 0,
                        }
                    }}
                />
                <Button 
                    sx={{
                        borderTopLeftRadius: 0,
                        borderBottomLeftRadius: 0,
                    }}
                    variant="outlined"
                    onClick={ClearSearch}
                >
                    < CancelIcon/>
                </Button>
            </Stack>
            <Grid container spacing={1} padding={"10px"}>
                {displaySearch ? (
                    searchRes.map((book) => {
                        return <BookModule book={book} />;
                    })
                ) : (
                    books.map((book) => {
                        return <BookModule book={book} />;
                    })                    
                )}
            </Grid>
        </>
    );
}

function BookModule({ book }: { book: Book }) {
    const theme = useTheme();
    const { LoggedIn, setLoggedIn } = useContext(loginStateContext);
    const nav = useNavigate();

    const HandleLoan = (isbn: string) => {
        if (!LoggedIn) {
            nav(RouteUrl.Login);
            return;
        }

        const url: string = `${import.meta.env.VITE_SERVER_DOMAIN}/api/Loans/loan/${book.isbn}`;
        fetch(url, { method: "POST" })
            .then((res) => {
                switch (res.status) {
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
        <Grid size={{ xs: 12, sm: 12, md: 6, lg: 4, xl: 3 }}>
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
                        <Button onClick={() => HandleLoan(book.isbn)} variant={"contained"} sx={{ width: "100px" }}>Loan</Button>
                    </Stack>
                </Stack>
            </Stack>
        </Grid>
    )
}



export default BookContent;

