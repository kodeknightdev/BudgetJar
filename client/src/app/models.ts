export type TransactionType = 'Income' | 'Expense';

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
  color: string;
}

export interface Transaction {
  id: number;
  amount: number;
  date: string;
  description: string;
  categoryId: number;
  categoryName: string;
  type: TransactionType;
}

export interface MonthlyTotal {
  year: number;
  month: number;
  income: number;
  expense: number;
  net: number;
}

export interface CategoryTotal {
  categoryId: number;
  name: string;
  color: string;
  total: number;
}

export interface AuthResponse {
  token: string;
  email: string;
  expiresAt: string;
}

export interface SaveTransaction {
  amount: number;
  date: string;
  description: string;
  categoryId: number;
}
