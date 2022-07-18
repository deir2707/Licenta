export interface CarInput {
  Make: string;
  Model: string;
  Year: string;
  Transmission: string;
  EngineCapacity: string;
  Mileage: string;
  FuelType: string;
  Description: string;
  StartPrice: string;
  CarImageInput?: CarImageInput;
  type: string;
}
export interface CarImageInput {
  files: any[];
}