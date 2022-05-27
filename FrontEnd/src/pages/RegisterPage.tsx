import { TextField, Button } from '@mui/material';
import { FormikHelpers, useFormik } from 'formik';
import React, { useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import Api from '../Api';
import { ApiEndpoints } from '../ApiEndpoints';
import { UserContext } from '../context/UserContext';
import { useApiError } from '../hooks/useApiError';
import { LoginInput, RegisterInput, UserDto } from '../interfaces/UsersInterfaces';

export const RegisterPage = () => {
    const navigate = useNavigate();
    const { handleApiError } = useApiError();
    const { setUserId, setFullName, setEmail, setBalance } = useContext(UserContext);

    const onSubmit = (values: RegisterInput, formikHelpers: FormikHelpers<RegisterInput>) => {

        const registerModel: RegisterInput = {
            email: values.email,
            password: values.password,
            fullname: values.fullname
        }

        Api.post<RegisterInput, UserDto>(`${ApiEndpoints.users}/register`, registerModel)
            .then(({ data }) => {
                setUserId(""+data.id)
                setFullName(data.fullname)
                setEmail(data.email)
                setBalance(""+data.balance)
                navigate("/mainpage")
            })
            .catch((error) => {
                // console.log(error)
                handleApiError(error, formikHelpers.setStatus)
            })
    }

    const onLoginClick = () => {
        navigate("/")
    }

    const formik = useFormik({
        initialValues: {
            email: "",
            password: "",
            fullname: ""
        },
        onSubmit: onSubmit,
    });

    return (
        <div className="loginPage">
            <div className="form-div">
                <form onSubmit={formik.handleSubmit}>

                    {formik.status &&
                        <div className="formError">{formik.status}</div>}
                    <TextField
                        fullWidth
                        id="email"
                        name="email"
                        label="Email"
                        value={formik.values.email}
                        onChange={formik.handleChange}
                    />

                    <TextField
                        fullWidth
                        id="password"
                        name="password"
                        label="Password"
                        type="password"
                        value={formik.values.password}
                        onChange={formik.handleChange}
                    />

                    <TextField
                        fullWidth
                        id="fullname"
                        name="fullname"
                        label="Full Name"
                        value={formik.values.fullname}
                        onChange={formik.handleChange}
                    />

                    <Button color="primary" variant="contained" fullWidth type="submit" >
                        Submit
                    </Button>
                    Already have an account?
                    <Button color="primary" variant="contained" fullWidth onClick={onLoginClick}>
                        Login
                    </Button>
                </form>
            </div>
        </div>
    )
}
