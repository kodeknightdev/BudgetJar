import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { SaveTransaction, Transaction } from '../models';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly url = `${environment.apiUrl}/transactions`;

  constructor(private http: HttpClient) { }

  getForMonth(year: number, month: number): Observable<Transaction[]> {
    const params = new HttpParams()
      .set('year', String(year))
      .set('month', String(month));
    return this.http.get<Transaction[]>(this.url, { params });
  }

  create(transaction: SaveTransaction): Observable<Transaction> {
    return this.http.post<Transaction>(this.url, transaction);
  }

  update(id: number, transaction: SaveTransaction): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, transaction);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
