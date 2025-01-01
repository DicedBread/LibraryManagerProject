import React, { ChangeEvent, ChangeEventHandler, useState } from "react";
import FormElement from "./FormElement";
import "../style/App.css"
import "../style/Form.css"

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
        // const target = event.target as typeof event.target & {
        //     value: Register
        // }
        // const email = target.value.email;
        // const fName = target.value.fName;
        // const lName = target.value.lName;
        // const password = target.value.password;
        // setInputs(target.value => ({...target.value, [name]: value}));

        const {name, value} = event.currentTarget;
        // setInputs({[name]: value } as Pick<Register, keyof Register>);
    }

    return (
        <form onSubmit={handleSubmit} className="formBase">
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
            <br />
            <FormElement 
                name="LastName"
                value={inputs?.lName || ""}
                type="text"
                onChange={handleChange}
            />
            <br />
            <FormElement
                name="Password"
                value={inputs?.password || ""}
                type="password"
                onChange={handleChange}
            />
        </form>
    );
}





export default Register