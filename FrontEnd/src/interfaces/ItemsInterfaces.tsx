export interface CarInput {
  Make: string;
  Model: string;
  Year: string;
  Transmission: string;
  EngineCapacity: string;
  Mileage: string;
  FuelType: string;
  // FileWrapper?: FileWrapper;
  UserId: string;
  Description: string;
  StartPrice: string;
  CarImageInput?: CarImageInput;
}
export interface CarImageInput {
  files: any[];
}