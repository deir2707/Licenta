import React, { useEffect, useState } from "react"
import Api from "../Api"
import { ApiEndpoints } from "../ApiEndpoints"
import { LoginInput, UserDto } from "../interfaces/UsersInterfaces"



export const UsersPage = () => {
    const [userDetails, setUserDetails] = useState<UserDto>()

    useEffect(() => {
        Api.get<UserDto>(`${ApiEndpoints.users}/1`)
            .then(({ data }) => {
                setUserDetails(data)
            })
            .catch((error) => {
                // alert(error)
                console.log(error)
            })
        const loginModel: LoginInput = {
            email: "edo@email.com",
            password: "edoPassword"
        }

        Api.post<LoginInput, UserDto>(`${ApiEndpoints.users}/login`, loginModel)
            .then(({ data }) => {
                //redirect to homepage
            })
            .catch((error) => {
                console.log(error)
            })
    }, [])

    return (
        <>
            User Details:
            {!userDetails && <p> loading</p>}
            {userDetails && <div>
                <p>Id: {userDetails?.id}</p>
                <p>Email: {userDetails?.email}</p>
            </div>}
        </>
    )
}