import "./style/App.css";
import { useState, useEffect } from "react";

function BookContent() {
    const [books, setBooks] = useState([]);

    useEffect(() => {
        fetch("http://localhost:5255/api/Books?limit=3&offset=0")
            .then((res) => {return res.json();})
            .then((data) => {setBooks(data);})
            .catch((err) => {console.log("failed to access books ", err)});
    }, []);

    return (
        <div className="BookContent">
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
            <img src={book.imgUrl} alt={book.title + " book image"} />
            <div>
                <p>Title: {book.title}</p>
            </div>
        </div>
    )
}

export default BookContent;
