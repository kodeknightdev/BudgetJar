import { Component, OnInit } from '@angular/core';

import { Transaction } from '../models';
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

  constructor(private transactionService: TransactionService) {
    const now = new Date();
    this.year = now.getFullYear();
    this.month = now.getMonth() + 1;
  }

  get monthValue(): string {
    return `${this.year}-${String(this.month).padStart(2, '0')}`;
  }

  ngOnInit(): void {
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

  remove(t: Transaction): void {
    if (!confirm('Delete this transaction?')) {
      return;
    }
    this.transactionService.delete(t.id).subscribe(() => this.load());
  }
}
