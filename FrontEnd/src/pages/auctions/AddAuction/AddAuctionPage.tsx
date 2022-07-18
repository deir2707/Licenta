import { Button, MenuItem, Select, TextField } from "@mui/material";
import { FormikHelpers, useFormik } from "formik";
import React, { useCallback, useMemo, useState } from "react";
import Api from "../../../Api";
import { ApiEndpoints } from "../../../ApiEndpoints";
import { PageLayout } from "../../../components/PageLayout";
import { useApiError } from "../../../hooks/useApiError";
import { CarInput } from "../../../interfaces/ItemsInterfaces";
import "./AddAuctionPage.scss";
interface ItemType {
  [key: string]: {
    label: string;
    type: string;
  }[];
}

export const AddAuctionPage = () => {
  const [files, setFiles] = useState<File[] | undefined>();

  const clearFiles = useCallback(() => {
    setFiles(undefined);
  }, []);

  const { handleApiError } = useApiError();

  const items: ItemType = {
    Car: [
      { label: "Make", type: "string" },
      { label: "Model", type: "string" },
      { label: "Year", type: "number" },
      { label: "Transmission", type: "string" },
      { label: "Engine Capacity", type: "number" },
      { label: "Mileage", type: "number" },
      { label: "Fuel Type", type: "string" },
    ],
    Antiquty: [
      { label: "Country of origin", type: "string" },
      { label: "Field 2", type: "string" },
      { label: "field 3", type: "string" },
      { label: "field 4", type: "string" },
      { label: "field 5", type: "string" },
    ],
    Painting: [
      { label: "Artist", type: "string" },
      { label: "Title", type: "string" },
      { label: "Year", type: "number" },
      { label: "Medium", type: "string" },
      { label: "Dimensions", type: "string" },
    ],
  };

  const onFileUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    const files: File[] = [];

    for (let i = 0; i < e.target.files!.length; i++) {
      const file = e.target.files?.[i];
      if (file) files.push(file);
    }

    setFiles(files);
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

    if (files) {
      for (let i = 0; i < files.length; i++) {
        formdata.append(`images`, files[i]);
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
    type: "Car",
  };

  const formik = useFormik({
    initialValues: initialValues,
    onSubmit: onSubmit,
  });

  const uploadButton = useMemo(() => {
    return (
      <div>
        <Button variant="contained" component="label">
          Upload File{" "}
          {files
            ? files.length === 1
              ? "1 file uploaded"
              : `${files.length} files uploaded`
            : "No files uploaded"}
          <input type="file" onChange={onFileUpload} multiple hidden />
        </Button>

        <Button onClick={clearFiles}>Clear</Button>
      </div>
    );
  }, [clearFiles, files]);

  return (
    <PageLayout>
      <div className="add-auction-container">
        <form onSubmit={formik.handleSubmit}>
          {formik.status && <div className="error">{formik.status}</div>}
          <TextField
            name="StartPrice"
            label="Start Price"
            type="number"
            onChange={formik.handleChange}
          />
          <TextField
            id="Description"
            name="Description"
            label="Description"
            multiline
            onChange={formik.handleChange}
          />
          <Select
            id="type"
            name="type"
            label="type"
            onChange={formik.handleChange}
            value={formik.values.type}
          >
            {Object.keys(items).map((key) => (
              <MenuItem key={key} value={key}>
                {key}
              </MenuItem>
            ))}
          </Select>

          {items[formik.values.type].map((field) => (
            <TextField
              id={field.label}
              name={field.label}
              label={field.label}
              key={field.label}
              type={field.type}
              onChange={formik.handleChange}
            />
          ))}

          {uploadButton}
          <Button variant="contained" color="primary" type="submit">
            Add auction
          </Button>
        </form>
      </div>
    </PageLayout>
  );
};
