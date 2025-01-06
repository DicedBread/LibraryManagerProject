import React from "react";
import "./style/App.css";
import { useState, useEffect } from "react";

interface Book{
    id:string,
    title:string,
    authour:string,
    publisher:string,
    imgUrl:string
}

function BookContent() {
    const [books, setBooks] = useState<Book[]>([]);

    useEffect(() => {
        const url:string = "https://" + import.meta.env.VITE_SERVER_DOMAIN + "/api/Books?limit=20&offset=0"
        fetch(url)
            .then((res) => {return res.json();})
            .then((data) => {setBooks(data);})
            .catch((err) => {console.log("failed to access books ", err)});
    }, []);

    return (
        <div className="bookContent">
            {books.length !== 0 ? (
                books.map((book) => {
                    return <BookModule book={book} />;
                })
            ) : (
                <p>No books available</p>
            )}
        </div>
    );
}

function BookModule({book}: {book: Book}){
    return (
        <div className="bookModule">
            <div className="bookImg">
                <img src={book.imgUrl} alt={book.title + " book image"} />
            </div>
            <div>
                <h4>Title:</h4>
                <p>{book.title}</p>
                <br />
                <h5>Authour:</h5>
                <p>{book.authour}</p>
            </div>
        </div>
    )
}

export default BookContent;
