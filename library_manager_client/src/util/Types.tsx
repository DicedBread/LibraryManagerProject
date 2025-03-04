export interface Book{
    isbn:string,
    title:string,
    author:string,
    publisher:string,
    imgUrl:string
}

export interface Loan{
    loanId:number,
    userId:number,
    book: Book,
    date:Date
}