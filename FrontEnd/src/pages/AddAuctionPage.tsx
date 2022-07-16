import { Button, TextField } from "@mui/material";
import React, { useContext, useState } from "react";
import { PageLayout } from "../components/PageLayout";
import "./AddAuctionPage.scss";
import { FormikHelpers, useFormik } from "formik";
import { ApiEndpoints } from "../ApiEndpoints";
import Api from "../Api";
import { CarInput } from "../interfaces/ItemsInterfaces";
import { useNavigate } from "react-router-dom";
import { useApiError } from "../hooks/useApiError";
import { UserContext } from "../context/UserContext";

interface ItemType {
  [key: string]: string[];
}

export const AddAuctionPage = () => {
  const [file, setFile] = useState<File[] | undefined>();

  const { userId, fullName } = useContext(UserContext);
  console.log(userId, fullName);

  const navigate = useNavigate();

  const { handleApiError } = useApiError();

  const items: ItemType = {
    Car: [
      "Make",
      "Model",
      "Year",
      "Mileage",
      "Fuel_Type",
      "Engine_capacity",
      "Transmission",
    ],
    Antiquty: ["Country of origin", "Field 2", "field 3", "field 4", "field 5"],
    Painting: [
      "field 8",
      "field 9",
      "field 10",
      "field 11",
      "field 12",
      "field 13",
      "field 14",
    ],
  };

  const onFileUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files: File[] = [];

    for (let i = 0; i < e.target.files!.length; i++) {
      const file = e.target.files?.[i];
      if (file) files.push(file);
    }

    setFile(files);
  };

  const onSubmit = async (
    values: CarInput,
    formikHelpers: FormikHelpers<CarInput>
  ) => {
    const formdata = new FormData();

    formdata.append("StartPrice", values.StartPrice);
    formdata.append("Make", values.Make);
    formdata.append("Model", values.Model);
    formdata.append("Year", values.Year);
    formdata.append("Transmission", values.Transmission);
    formdata.append("EngineCapacity", values.EngineCapacity);
    formdata.append("Mileage", values.Mileage);
    formdata.append("FuelType", values.FuelType);
    formdata.append("Description", values.Description);
    formdata.append("UserId", userId.toString());

    if (file) {
      for (let i = 0; i < file.length; i++) {
        formdata.append(`images`, file[i]);
      }
    }

    Api.post(`${ApiEndpoints.add_car}`, formdata, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
      .then(({ data }) => {
        console.log(data);
      })
      .catch((error) => {
        handleApiError(error, formikHelpers.setStatus);
      });
  };

  const initialValues: CarInput = {
    StartPrice: "",
    Make: "",
    Model: "",
    Year: "",
    Transmission: "",
    EngineCapacity: "",
    Mileage: "",
    FuelType: "",
    Description: "",
    UserId: "",
  };

  const formik = useFormik({
    initialValues: initialValues,
    onSubmit: onSubmit,
  });

  return (
    <PageLayout>
      <div>
        <form onSubmit={formik.handleSubmit}>
          {formik.status && <div className="error">{formik.status}</div>}
          <TextField
            name="StartPrice"
            label="Start Price"
            type="number"
            onChange={formik.handleChange}
          />
          <TextField
            id="Make"
            name="Make"
            label="Make"
            onChange={formik.handleChange}
          />
          <TextField
            id="Model"
            name="Model"
            label="Model"
            onChange={formik.handleChange}
          />
          <TextField
            id="Year"
            name="Year"
            label="Year"
            onChange={formik.handleChange}
          />
          <TextField
            id="Transmission"
            name="Transmission"
            label="Transmission"
            onChange={formik.handleChange}
          />
          <TextField
            id="EngineCapacity"
            name="EngineCapacity"
            label="Engine Capacity"
            type="number"
            onChange={formik.handleChange}
          />
          <TextField
            id="Mileage"
            name="Mileage"
            label="Mileage"
            type="number"
            onChange={formik.handleChange}
          />
          <TextField
            id="FuelType"
            name="FuelType"
            label="FuelType"
            onChange={formik.handleChange}
          />
          <TextField
            id="Description"
            name="Description"
            label="Description"
            multiline
            onChange={formik.handleChange}
          />
          <Button variant="contained" component="label">
            Upload File
            <input type="file" hidden onChange={onFileUpload} multiple />
          </Button>
          <Button variant="contained" color="primary" type="submit">
            Add auction
          </Button>
        </form>
      </div>
      {/* <div>
        <h1>Select the type of item you want to sell</h1>
        <div className="select-style">
          <Formik initialValues={initialValues} onSubmit={onSubmit}>
            {({ values, handleChange, handleSubmit, status }) => (
              // <Form encType="multipart/form-data" onSubmit={formik.handleSubmit}>
              <Form>
                {status && <div className="formError">{status}</div>}
                <select
                  className="modifySelect"
                  onChange={(e) => {
                    e.preventDefault();
                    setType(e.target.value);
                  }}
                >
                  <option label=""></option>
                  {Object.keys(items).map((key) => (
                    <option key={key} value={key}>
                      {key}
                    </option>
                  ))}
                </select>

                <div id="container">
                  <div id="left">
                    {type && (
                      <div>
                        <div className="div_for_fields">
                          <Field
                            component={TextField}
                            id="StartPrice"
                            name="StartPrice"
                            label="StartPrice"
                            // onChange={formik.handleChange}
                          />
                        </div>
                        {items[type].map((item) => (
                          <div key={item} className="div_for_fields">
                            <Field
                              component={TextField}
                              id={item}
                              name={item}
                              label={item}
                              //   onChange={formik.handleChange}
                            />
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                  <div id="right">
                    <Field
                      component={TextField}
                      id="Description"
                      name="Description"
                      //   onChange={formik.handleChange}
                      //   value={values.Description}
                      placeholder="Description"
                      multiline
                    />
                    <p className="">Select a photo</p>
                    <input
                      className="inputFile"
                      id="file"
                      name="file"
                      type="file"
                      onChange={(event) => {
                        // formik.setFieldValue("file", event.currentTarget.files![0]);
                        setFile(event.currentTarget.files?.[0]);
                      }}
                    />
                    <Button
                      className="AddAuction"
                      color="primary"
                      variant="contained"
                      fullWidth
                      type="submit"
                    >
                      Submit
                    </Button>
                  </div>
                </div>
              </Form>
            )}
          </Formik>
        </div>
      </div> */}
    </PageLayout>
  );
};
