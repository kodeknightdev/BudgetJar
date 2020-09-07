import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { AuthResponse } from '../models';

const TOKEN_KEY = 'budgetjar.token';
const EMAIL_KEY = 'budgetjar.email';
const EXPIRES_KEY = 'budgetjar.expires';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly url = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) { }

  get token(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  get email(): string | null {
    return localStorage.getItem(EMAIL_KEY);
  }

  isLoggedIn(): boolean {
    const expires = localStorage.getItem(EXPIRES_KEY);
    return !!this.token && !!expires && new Date(expires) > new Date();
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.url}/login`, { email, password })
      .pipe(tap(res => this.store(res)));
  }

  register(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.url}/register`, { email, password })
      .pipe(tap(res => this.store(res)));
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
    localStorage.removeItem(EXPIRES_KEY);
  }

  private store(res: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    localStorage.setItem(EMAIL_KEY, res.email);
    localStorage.setItem(EXPIRES_KEY, res.expiresAt);
  }
}
