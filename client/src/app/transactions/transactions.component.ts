import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { Category, Transaction, TransactionType } from '../models';
import { CategoryService } from '../services/category.service';
import { TransactionService } from '../services/transaction.service';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.css']
})
export class TransactionsComponent implements OnInit {
  year: number;
  month: number;
  transactions: Transaction[] = [];
  categories: Category[] = [];
  editingId: number | null = null;

  form = this.fb.group({
    amount: [null, [Validators.required, Validators.min(0.01)]],
    date: [this.today(), Validators.required],
    description: ['', Validators.maxLength(200)],
    categoryId: [null, Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private transactionService: TransactionService
  ) {
    const now = new Date();
    this.year = now.getFullYear();
    this.month = now.getMonth() + 1;
  }

  get monthValue(): string {
    return `${this.year}-${String(this.month).padStart(2, '0')}`;
  }

  get totalIncome(): number {
    return this.sum('Income');
  }

  get totalExpense(): number {
    return this.sum('Expense');
  }

  get net(): number {
    return this.totalIncome - this.totalExpense;
  }

  get incomeCategories(): Category[] {
    return this.categories.filter(c => c.type === 'Income');
  }

  get expenseCategories(): Category[] {
    return this.categories.filter(c => c.type === 'Expense');
  }

  ngOnInit(): void {
    this.categoryService.getAll().subscribe(categories => this.categories = categories);
    this.load();
  }

  onMonthChange(value: string): void {
    if (!value) {
      return;
    }
    const [year, month] = value.split('-').map(Number);
    this.year = year;
    this.month = month;
    this.load();
  }

  load(): void {
    this.transactionService.getForMonth(this.year, this.month)
      .subscribe(transactions => this.transactions = transactions);
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }
    const value = this.form.value;
    const request: Observable<unknown> = this.editingId
      ? this.transactionService.update(this.editingId, value)
      : this.transactionService.create(value);

    request.subscribe(() => {
      this.cancelEdit();
      this.load();
    });
  }

  edit(t: Transaction): void {
    this.editingId = t.id;
    this.form.setValue({
      amount: t.amount,
      date: t.date,
      description: t.description || '',
      categoryId: t.categoryId
    });
  }

  cancelEdit(): void {
    this.editingId = null;
    this.form.reset({ amount: null, date: this.today(), description: '', categoryId: null });
  }

  remove(t: Transaction): void {
    if (!confirm('Delete this transaction?')) {
      return;
    }
    this.transactionService.delete(t.id).subscribe(() => this.load());
  }

  private sum(type: TransactionType): number {
    return this.transactions
      .filter(t => t.type === type)
      .reduce((total, t) => total + t.amount, 0);
  }

  private today(): string {
    return new Date().toISOString().substring(0, 10);
  }
}
