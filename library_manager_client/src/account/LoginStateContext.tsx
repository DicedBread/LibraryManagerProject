import { createContext } from "react";

interface LoginState{
    LoggedIn: boolean;
    setLoggedIn: (value: boolean) => void; 
  }
  
  export const loginStateContext = createContext<LoginState>({
    LoggedIn: false,
    setLoggedIn: (v: boolean) => {}
  });