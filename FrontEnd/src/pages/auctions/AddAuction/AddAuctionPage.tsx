import { Button, MenuItem, TextField } from "@mui/material";
import { FormikHelpers, getIn, useFormik } from "formik";
import _ from "lodash";
import React, { useCallback, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import * as yup from "yup";

import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { PageLayout } from "../../../components/PageLayout";
import { useApiError } from "../../../hooks/useApiError";
import {
  AddAuctionInput,
  AuctionType,
} from "../../../interfaces/AuctionInterfaces";
import { CarInput } from "../../../interfaces/ItemsInterfaces";
import "./AddAuctionPage.scss";
interface ItemType {
  [AuctionType.Car]: {
    label: string;
    type: string;
  }[];
  [AuctionType.Antiquity]: {
    label: string;
    type: string;
  }[];
  [AuctionType.Painting]: {
    label: string;
    type: string;
  }[];
}

export const AddAuctionPage = () => {
  const [files, setFiles] = useState<File[] | undefined>();

  const navigate = useNavigate();

  const clearFiles = useCallback(() => {
    setFiles(undefined);
  }, []);

  const { handleApiError } = useApiError();

  const items: ItemType = useMemo(() => {
    return {
      [AuctionType.Car]: [
        { label: "Make", type: "string" },
        { label: "Model", type: "string" },
        { label: "Year", type: "string" },
        { label: "Transmission", type: "string" },
        { label: "EngineCapacity", type: "string" },
        { label: "Mileage", type: "string" },
        { label: "FuelType", type: "string" },
      ],
      [AuctionType.Painting]: [
        { label: "Artist", type: "string" },
        { label: "Year", type: "string" },
        { label: "Medium", type: "string" },
        { label: "Dimensions", type: "string" },
      ],
      [AuctionType.Antiquity]: [
        { label: "CountryOfOrigin", type: "string" },
        { label: "Field2", type: "string" },
        { label: "Field3", type: "string" },
        { label: "Field4", type: "string" },
        { label: "Field5", type: "string" },
      ],
    };
  }, []);

  const onFileUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files: File[] = [];

    for (let i = 0; i < e.target.files!.length; i++) {
      const file = e.target.files?.[i];

      if (file) {
        if (file.type.startsWith("image")) {
          files.push(file);
        }
      }
    }

    setFiles(files.length > 0 ? files : undefined);
  };

  const onSubmit = async (
    values: AddAuctionInput,
    formikHelpers: FormikHelpers<AddAuctionInput>
  ) => {
    const formdata = new FormData();

    formdata.append("Title", values.Title);
    formdata.append("StartPrice", values.StartPrice.toString());
    formdata.append("Type", values.Type.toString());
    formdata.append("Description", values.Description);

    if (values.Type.toString() === AuctionType.Car.toString()) {
      const otherDetails: CarInput = {
        Make: values.Make || "",
        Model: values.Model || "",
        Year: values.Year || "",
        Transmission: values.Transmission || "",
        EngineCapacity: values.EngineCapacity || "",
        Mileage: values.Mileage || "",
        FuelType: values.FuelType || "",
      };

      formdata.append("OtherDetails", JSON.stringify(otherDetails));
    } else if (values.Type.toString() === AuctionType.Antiquity.toString()) {
      const otherDetails: /*AntiquityInput*/ any = {
        CountryOfOrigin: values.CountryOfOrigin,
        Field2: values.Field2,
        Field3: values.Field3,
        Field4: values.Field4,
        Field5: values.Field5,
      };

      formdata.append("OtherDetails", JSON.stringify(otherDetails));
    } else if (values.Type.toString() === AuctionType.Painting.toString()) {
      const otherDetails: /*PaintingInput*/ any = {
        Artist: values.Artist,
        Year: values.Year,
        Medium: values.Medium,
        Dimensions: values.Dimensions,
      };

      formdata.append("OtherDetails", JSON.stringify(otherDetails));
    }

    if (files) {
      for (let i = 0; i < files.length; i++) {
        formdata.append(`images`, files[i]);
      }
    }

    Api.post<any, number>(`${ApiEndpoints.add_auction}`, formdata, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    })
      .then(({ data }) => {
        navigate(`/auctions/${data}`);
      })
      .catch((error) => {
        handleApiError(error, formikHelpers.setStatus);
      });
  };

  const initialValues: AddAuctionInput = {
    Title: "",
    StartPrice: 0,
    Description: "",
    Type: AuctionType.Car,

    Make: "",
    Model: "",
    Year: "",
    Transmission: "",
    EngineCapacity: "",
    Mileage: "",
    FuelType: "",

    CountryOfOrigin: "",
    Field2: "",
    Field3: "",
    Field4: "",
    Field5: "",

    Artist: "",
    Medium: "",
    Dimensions: "",
  };

  const validationSchema = yup.object({
    Title: yup.string().required("Title is required"),
    StartPrice: yup
      .number()
      .required("StartPrice is required")
      .moreThan(0, "StartPrice must be more than 0"),
    Description: yup.string().required("Description is required"),
  });

  const formik = useFormik({
    initialValues,
    validationSchema,
    validateOnBlur: false,
    validateOnChange: false,
    onSubmit: onSubmit,
  });

  const uploadButton = useMemo(() => {
    return (
      <div className="upload-container">
        <Button variant="contained" component="label">
          Upload photos
          <input type="file" onChange={onFileUpload} multiple hidden />
        </Button>

        <div>
          {files && (
            <Button onClick={clearFiles}>
              Clear {files.length === 1 ? "1 photo" : `${files.length} photos`}
            </Button>
          )}
        </div>
      </div>
    );
  }, [clearFiles, files]);

  const customFields = useMemo(() => {
    const value = formik.values.Type;
    return items[value].map((field) => {
      const error = getIn(formik.errors, field.label);
      const touched = getIn(formik.touched, field.label);

      return (
        <TextField
          id={field.label}
          name={field.label}
          label={_.startCase(field.label)}
          key={field.label}
          type={field.type}
          onChange={formik.handleChange}
          error={touched && Boolean(error)}
          helperText={touched && error}
        />
      );
    });
  }, [
    formik.errors,
    formik.handleChange,
    formik.touched,
    formik.values.Type,
    items,
  ]);

  return (
    <PageLayout>
      <div className="add-auction-container">
        <form onSubmit={formik.handleSubmit}>
          {formik.status && <div className="error">{formik.status}</div>}
          <div className="form">
            <TextField
              id="StartPrice"
              name="StartPrice"
              label="Start Price"
              type="number"
              value={formik.values.StartPrice}
              onChange={formik.handleChange}
              error={
                formik.touched.StartPrice && Boolean(formik.errors.StartPrice)
              }
              helperText={formik.touched.StartPrice && formik.errors.StartPrice}
            />
            <TextField
              id="Title"
              name="Title"
              label="Title"
              value={formik.values.Title}
              onChange={formik.handleChange}
              error={formik.touched.Title && Boolean(formik.errors.Title)}
              helperText={formik.touched.Title && formik.errors.Title}
            />
            <TextField
              id="Description"
              name="Description"
              label="Description"
              value={formik.values.Description}
              multiline
              onChange={formik.handleChange}
              error={
                formik.touched.Description && Boolean(formik.errors.Description)
              }
              helperText={
                formik.touched.Description && formik.errors.Description
              }
            />
            <TextField
              id="Type"
              name="Type"
              onChange={formik.handleChange}
              value={formik.values.Type}
              select
              label="Type"
            >
              {Object.keys(AuctionType)
                .filter((x) => Number(x) >= 0)
                .map((key) => (
                  <MenuItem key={key} value={key}>
                    {AuctionType[Number(key)]}
                  </MenuItem>
                ))}
            </TextField>
            {customFields}
            {uploadButton}
          </div>

          <Button variant="contained" color="primary" type="submit">
            Add auction
          </Button>
        </form>
      </div>
    </PageLayout>
  );
};
