import React, { ChangeEvent, ChangeEventHandler, useState } from "react";



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

export default FormElement