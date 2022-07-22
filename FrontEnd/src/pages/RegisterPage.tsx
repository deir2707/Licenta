import { TextField, Button } from '@mui/material';
import { FormikHelpers, useFormik } from 'formik';
import React, { useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import * as yup from "yup";

import Api from "../Api";
import { ApiEndpoints } from "../ApiEndpoints";
import { UserContext } from "../context/UserContext";
import { useApiError } from "../hooks/useApiError";
import { RegisterInput, UserDetails } from "../interfaces/UsersInterfaces";

export const RegisterPage = () => {
  const navigate = useNavigate();
  const { handleApiError } = useApiError();
  const { setUserId, setFullName, setEmail, setBalance } =
    useContext(UserContext);

  const onSubmit = (
    values: RegisterInput,
    formikHelpers: FormikHelpers<RegisterInput>
  ) => {
    const registerModel: RegisterInput = {
      email: values.email,
      password: values.password,
      fullName: values.fullName,
    };

    Api.post<RegisterInput, UserDetails>(
      `${ApiEndpoints.users}/register`,
      registerModel
    )
      .then(({ data }) => {
        localStorage.setItem("userId", data.id.toString());
        localStorage.setItem("fullName", data.fullName);
        localStorage.setItem("email", data.email);
        localStorage.setItem("balance", data.balance.toString());
        setUserId(data.id);
        setFullName(data.fullName);
        setEmail(data.email);
        setBalance(data.balance);
        navigate("/mainpage");
      })
      .catch((error) => {
        handleApiError(error, formikHelpers.setStatus);
      });
  };

  const onLoginClick = () => {
    navigate("/");
  };

  const validationSchema = yup.object({
    email: yup
      .string()
      .email("Enter a valid email")
      .required("Email is required"),
    password: yup
      .string()
      .min(8, "Password should be of minimum 8 characters length")
      .required("Password is required"),
    fullName: yup.string().required("Full name is required"),
  });

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
      fullName: "",
    },
    validationSchema: validationSchema,
    onSubmit: onSubmit,
  });

  return (
    <div className="loginPage">
      <div className="form-div">
        <form onSubmit={formik.handleSubmit}>
          {formik.status && <div className="formError">{formik.status}</div>}
          <TextField
            fullWidth
            id="email"
            name="email"
            label="Email"
            value={formik.values.email}
            onChange={formik.handleChange}
            error={formik.touched.email && Boolean(formik.errors.email)}
            helperText={formik.touched.email && formik.errors.email}
          />
          <TextField
            fullWidth
            id="password"
            name="password"
            label="Password"
            type="password"
            value={formik.values.password}
            onChange={formik.handleChange}
            error={formik.touched.password && Boolean(formik.errors.password)}
            helperText={formik.touched.password && formik.errors.password}
          />
          <TextField
            fullWidth
            id="fullName"
            name="fullName"
            label="Full Name"
            value={formik.values.fullName}
            onChange={formik.handleChange}
            error={formik.touched.fullName && Boolean(formik.errors.fullName)}
            helperText={formik.touched.fullName && formik.errors.fullName}
          />
          <Button color="primary" variant="contained" fullWidth type="submit">
            Submit
          </Button>
          Already have an account?
          <Button
            color="primary"
            variant="contained"
            fullWidth
            onClick={onLoginClick}
          >
            Login
          </Button>
        </form>
      </div>
    </div>
  );
};
