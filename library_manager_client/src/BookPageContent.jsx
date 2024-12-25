import React from "react";
import "./style/App.css";
import { useState, useEffect } from "react";

function BookContent() {
    const [books, setBooks] = useState([]);

    useEffect(() => {
        fetch("https://localhost:7291/api/Books?limit=20&offset=0")
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

function BookModule({book}){
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
