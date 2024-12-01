import React, { ChangeEvent, ChangeEventHandler, useState } from "react";
import "../style/App.css"

interface Register {
    email: string,
    fName: string,
    lName: number,
    password: string
}

function Register() {
    const [inputs, setInputs] = useState<Register>();

    const handleSubmit = () => {
        alert(`register with values:
             ${inputs?.email}
        `);
    }

    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
        const target = event.target as typeof event.target & {
            value: Register
        }
        const email = target.value.email;
        const fName = target.value.fName;
        const lName = target.value.lName;
        const password = target.value.password;
    }

    return (
        <form onSubmit={handleSubmit}>
            <br />
            <FormElement<string> 
                name="Email"
                value={inputs?.email || ""}
                type="email" 
                onChange={handleChange}
            />
            <br />
            <FormElement 
                name="FisrtName"
                value={inputs?.fName || ""}
                type="text"
                onChange={handleChange}
            /> 
        </form>
    );
}

interface FormParams<T extends string | number> {
    name: string,
    value: T,
    type: React.HTMLInputTypeAttribute,
    onChange: ChangeEventHandler,
}

function FormElement<T extends string | number>({name, value, type, onChange}: FormParams<T>) {
    return (
        <label htmlFor={name} className="formElement">
            {name}
            <br />
            <input
                type="email"
                name="email"
                id="email"
                value={value}
                onChange={onChange}
            />
        </label>
    );
}



export default Register