export type TransactionType = 'Income' | 'Expense';

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
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

export interface SaveTransaction {
  amount: number;
  date: string;
  description: string;
  categoryId: number;
}
