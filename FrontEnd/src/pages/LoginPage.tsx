import { Button, TextField } from "@mui/material"
import { FormikHelpers, useFormik } from "formik"
import { useContext } from "react"
import { useNavigate } from "react-router-dom"
import Api from "../Api"
import { ApiEndpoints } from "../ApiEndpoints"
import { UserContext } from "../context/UserContext"
import { useApiError } from "../hooks/useApiError"
import { LoginInput, UserDetails } from "../interfaces/UsersInterfaces";
import "./LoginPage.scss";

export const LoginPage = () => {
  const navigate = useNavigate();
  const { handleApiError } = useApiError();
  const { setUserId, setFullName, setEmail, setBalance } =
    useContext(UserContext);

  const onSubmit = (
    values: LoginInput,
    formikHelpers: FormikHelpers<LoginInput>
  ) => {
    const loginModel: LoginInput = {
      email: values.email,
      password: values.password,
    };

    Api.post<LoginInput, UserDetails>(`${ApiEndpoints.users}/login`, loginModel)
      .then(({ data: { id, fullname, email, balance } }) => {
        localStorage.setItem("userId", id.toString());
        localStorage.setItem("fullName", fullname);
        localStorage.setItem("email", email);
        localStorage.setItem("balance", balance.toString());
        setUserId(id);
        setFullName(fullname);
        setEmail(email);
        setBalance(balance);
        navigate("/mainpage");
      })
      .catch((error) => {
        handleApiError(error, formikHelpers.setStatus);
      });
  };

  const onRegisterClick = () => {
    navigate("/register");
  };

  const formik = useFormik({
    initialValues: {
      email: "",
      password: "",
    },
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
          <Button color="primary" variant="contained" fullWidth type="submit">
            Submit
          </Button>
          Do not have an account?
          <Button
            color="primary"
            variant="contained"
            fullWidth
            onClick={onRegisterClick}
          >
            Register
          </Button>
        </form>
      </div>
    </div>
  );
};
