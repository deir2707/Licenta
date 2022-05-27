export interface CarInput {
    Make: string
    Model: string
    Year: string
    Transmission: string
    Engine_capacity: string
    Mileage:string
    Fuel_Type:string
    FileWrapper:FileWrapper
    UserId:string
    Description: string
    StartPrice:string
}

export interface FileWrapper
{
    file:File|undefined
}