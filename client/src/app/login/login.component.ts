import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  mode: 'login' | 'register' = 'login';
  error = '';
  busy = false;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  toggleMode(): void {
    this.mode = this.mode === 'login' ? 'register' : 'login';
    this.error = '';
  }

  submit(): void {
    if (this.form.invalid) {
      return;
    }
    const { email, password } = this.form.value;
    const request = this.mode === 'login'
      ? this.auth.login(email, password)
      : this.auth.register(email, password);

    this.busy = true;
    this.error = '';
    request.subscribe(
      () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/';
        this.router.navigateByUrl(returnUrl);
      },
      (err: HttpErrorResponse) => {
        this.busy = false;
        if (err.status === 401) {
          this.error = 'Wrong email or password.';
        } else if (err.status === 409) {
          this.error = 'An account with that email already exists.';
        } else {
          this.error = 'Something went wrong. Is the API running?';
        }
      }
    );
  }
}
