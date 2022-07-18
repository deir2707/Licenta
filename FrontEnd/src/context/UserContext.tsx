import React, { createContext, ReactNode, useState } from "react";

export interface ProviderProps {
  children: ReactNode[] | ReactNode;
}

interface UserContextModel {
  userId: number;
  fullName: string;
  email: string;
  balance: number;
  setUserId: (value: number) => void;
  setFullName: (value: string) => void;
  setEmail: (value: string) => void;
  setBalance: (value: number) => void;
}

export const UserContext = createContext<UserContextModel>({
  userId: 0,
  fullName: "",
  email: "",
  balance: 0,
  setUserId: null as unknown as (value: number) => void,
  setFullName: null as unknown as (value: string) => void,
  setEmail: null as unknown as (value: string) => void,
  setBalance: null as unknown as (value: number) => void,
});

export const UserContextProvider = ({ children }: ProviderProps) => {
  const [userId, setUserId] = useState(
    localStorage.getItem("user") as unknown as number
  );
  const [fullName, setFullName] = useState(
    localStorage.getItem("fullName") as string
  );
  const [email, setEmail] = useState(localStorage.getItem("email") as string);
  const [balance, setBalance] = useState(
    localStorage.getItem("balance") as unknown as number
  );

  const data = {
    userId,
    setUserId,
    fullName,
    setFullName,
    email,
    setEmail,
    balance,
    setBalance,
  };

  return <UserContext.Provider value={data}>{children}</UserContext.Provider>;
};
