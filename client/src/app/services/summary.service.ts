import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { CategoryTotal, MonthlyTotal } from '../models';

@Injectable({
  providedIn: 'root'
})
export class SummaryService {
  private readonly url = `${environment.apiUrl}/summary`;

  constructor(private http: HttpClient) { }

  monthly(year: number): Observable<MonthlyTotal[]> {
    const params = new HttpParams().set('year', String(year));
    return this.http.get<MonthlyTotal[]>(`${this.url}/monthly`, { params });
  }

  byCategory(year: number, month: number): Observable<CategoryTotal[]> {
    const params = new HttpParams()
      .set('year', String(year))
      .set('month', String(month));
    return this.http.get<CategoryTotal[]>(`${this.url}/categories`, { params });
  }
}
