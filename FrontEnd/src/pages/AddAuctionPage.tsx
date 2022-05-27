import { Button, TextField } from '@mui/material';
import Downshift from 'downshift'
import React, { useContext, useState } from 'react'
import { PageLayout } from '../components/PageLayout'
import "./AddAuctionPage.scss"
import { Field, FormikHelpers, useFormik } from "formik"
import { ApiEndpoints } from '../ApiEndpoints';
import Api from '../Api';
import { CarInput } from "../interfaces/ItemsInterfaces"
import { useNavigate } from 'react-router-dom';
import { useApiError } from '../hooks/useApiError';
import { UserContext } from '../context/UserContext';

interface ItemType {
    [key: string]: string[];
}

export const AddAuctionPage = () => {

    const [file, setFile] = useState<File | undefined>();

    const { userId, fullName, email, balance } = useContext(UserContext);
    console.log(userId, fullName)
    const navigate = useNavigate();
    const { handleApiError } = useApiError();
    const items: ItemType = {
        Car: ["Make", "Model", "Year", "Mileage", "Fuel_Type", "Engine_capacity", "Transmission"],
        Antiquty: ["Country of origin", "Field 2", "field 3", "field 4", "field 5"],
        Painting: ["field 8", "field 9", "field 10", "field 11", "field 12", "field 13", "field 14"],
    };

    const onSubmit = (values: CarInput, formikHelpers: FormikHelpers<CarInput>) => {


        const carModel: CarInput = {
            StartPrice: values.StartPrice,
            Make: values.Make,
            Model: values.Model,
            Year: values.Year,
            Transmission: values.Transmission,
            Engine_capacity: values.Engine_capacity,
            Mileage: values.Mileage,
            Fuel_Type: values.Fuel_Type,
            FileWrapper:{file:file},
            Description: values.Description,
            UserId: userId



        }

        const formData=new FormData();
        formData.append("CarInput",JSON.stringify(carModel))
        Api.post(`${ApiEndpoints.add_car}`, formData,{

            headers:{
                "Content-Type":"multipart/form-data"
            }
        })
            .then(({ data }) => {
                console.log(carModel)

            })
            .catch((error) => {
                handleApiError(error, formikHelpers.setStatus)
            })
        console.log({
            carModel

        })
    }

    const [type, setType] = useState("");
    const initialValues: CarInput = {
        StartPrice: "",
        Make: "",
        Model: "",
        Year: "",
        Transmission: "",
        Engine_capacity: "",
        Mileage: "",
        Fuel_Type: "",
        FileWrapper: {file:undefined},
        Description: "",
        UserId: ""


    }
    const formik = useFormik({
        initialValues: initialValues,
        onSubmit: onSubmit,

    });
    return (
        <PageLayout>
            <div>
                <h1>
                    Select the type of item you want to sell
                </h1>
                <div className="select-style">
                    <form encType='multipart/form-data' onSubmit={formik.handleSubmit}>
                        {formik.status &&
                            <div className="formError">{formik.status}</div>}
                        <select className="modifySelect"
                            onChange={(e) => {
                                e.preventDefault();
                                setType(e.target.value);
                            }}
                        >
                            <option label="" ></option>
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
                                            <TextField id="StartPrice"
                                                name="StartPrice"
                                                label="StartPrice"
                                                onChange={formik.handleChange}
                                            ></TextField>
                                        </div>
                                        {items[type].map((item) => (
                                            <div key={item}
                                                className="div_for_fields">
                                                <TextField
                                                    id={item}
                                                    name={item}
                                                    label={item}
                                                    onChange={formik.handleChange}
                                                />

                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>
                            <div id="right">
                                <textarea
                                    id="Description"
                                    name="Description"
                                    onChange={formik.handleChange}
                                    value={formik.values.Description} placeholder='Description'
                                />
                                <p className=''>Select a photo</p>
                                <input className='inputFile' id="file" name="file" type="file" onChange={(event) => {
                                    // formik.setFieldValue("file", event.currentTarget.files![0]);
                                    setFile(event.currentTarget.files?.[0]);
                                }} />
                                <Button className="AddAuction" color="primary" variant="contained" fullWidth type="submit" >
                                    Submit
                                </Button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </PageLayout>
    );
}
