import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { PathLike } from "fs";
import qs from "qs";

const axiosConfig = {
  returnRejectPromiseOnError: true,
  timeout: 30000,
  baseURL: "https://localhost:5001",
  headers: {
    common: {
      "Cache-Control": "no-cache, no-store",
      Pragma: "no-cache",
      "Content-Type": "application/json",
      Accept: "application/json",
    },
  },
  paramsSerializer: (params: PathLike) =>
    qs.stringify(params, { indices: false }),
};

class ApiService {
  private _api: AxiosInstance;

  constructor() {
    this._api = axios.create(axiosConfig);
  }

  public get<ResponseDataType, Response = AxiosResponse<ResponseDataType>>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<Response> {
    return this._api.get(url, this.getConfigWithUserId());
  }

  public post<
    RequestDataType,
    ResponseDataType,
    Response = AxiosResponse<ResponseDataType>
  >(
    url: string,
    data: RequestDataType,
    config?: AxiosRequestConfig
  ): Promise<Response> {
    return this._api.post(url, data, this.getConfigWithUserId());
  }

  public put<
    RequestDataType,
    ResponseDataType,
    Response = AxiosResponse<ResponseDataType>
  >(
    url: string,
    data: RequestDataType,
    config?: AxiosRequestConfig
  ): Promise<Response> {
    return this._api.put(url, data, this.getConfigWithUserId());
  }

  public delete<ResponseDataType, Response = AxiosResponse<ResponseDataType>>(
    url: string,
    config?: AxiosRequestConfig
  ): Promise<Response> {
    return this._api.delete(url, this.getConfigWithUserId());
  }

  private getConfigWithUserId(): AxiosRequestConfig {
    const config: AxiosRequestConfig = {
      headers: {
        "User-Id": localStorage.getItem("userId") || "-1",
      },
    };
    return config;
  }
}

export default new ApiService();