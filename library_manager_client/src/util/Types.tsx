export interface Book{
    isbn:string,
    title:string,
    authour:string,
    publisher:string,
    imgUrl:string
}

export interface Loan{
    loanId:number,
    userId:number,
    book: Book,
    date:Date
}