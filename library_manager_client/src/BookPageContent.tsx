import React from "react";
import { useState, useEffect } from "react";
import { Book } from "./util/Types";
import Grid from "@mui/material/Grid2";
import { Box, Stack } from "@mui/material";
import { useTheme } from '@mui/material/styles';



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
    return (
        <Grid 
            size={{ xs:12, sm:12, md:3 }} 
            sx={{
                backgroundColor: theme.palette.mode,
                borderColor: theme.palette.primary.main,
                borderRadius: 2
            }} 
            border={1}>
            
            <Stack padding={"10px"}>
                <Box alignContent={"center"} justifyContent={"center"} display={"flex"}>
                    <img src={book.imgUrl} alt={book.title + " book image"} />
                </Box>
                <Box>
                    <h4>{book.title}</h4>
                    <p>{book.authour}</p>
                </Box>
            </Stack>
        </Grid>
    )
}

export default BookContent;
