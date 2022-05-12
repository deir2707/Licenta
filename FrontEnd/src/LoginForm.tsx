import React, { useState } from "react"
import { Button, Divider, FormControl, Input } from "@mui/material";

export const LoginForm = () => {
    const onFormSubmit = () => {
        console.log("call api for login")
    }

    return (
        <FormControl>
            <Input placeholder="Email address" id="email" type="email" />

            <Input placeholder="Password" id="password" type="password" />

            <Divider></Divider>

            <Button variant="contained" onSubmit={onFormSubmit}>Login</Button>
        </FormControl>
    )
}