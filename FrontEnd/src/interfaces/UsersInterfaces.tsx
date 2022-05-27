export interface UserDto {
    id: number
    fullname: string
    email: string
    balance: number
}

export interface LoginInput {
    email: string
    password: string
}

export interface RegisterInput {
    email: string
    password: string
    fullname: string
}

