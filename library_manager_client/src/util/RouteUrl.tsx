
export const RouteUrl ={
    Home : "/",
    Login : "/login",
    Register : "/register",
    Loans : "/loans",
} 

export const ApiRouteUrl = {
    Loans : "/api/loans",
    Loan : (id = ":id") => `api/loans${id}`,
}