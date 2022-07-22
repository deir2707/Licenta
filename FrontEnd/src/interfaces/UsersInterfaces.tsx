export interface UserDetails {
  id: number;
  fullName: string;
  email: string;
  balance: number;
}

export interface LoginInput {
  email: string;
  password: string;
}

export interface RegisterInput {
  email: string;
  password: string;
  fullName: string;
}

export interface AddBalanceInput {
  balanceToAdd: number;
}
