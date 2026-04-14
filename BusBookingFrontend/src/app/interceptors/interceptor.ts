import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // 1. Get token from localStorage
    const token = localStorage.getItem('token');

    // 2. If token exists, attach it to every request header
    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    // 3. Handle errors globally
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Token expired or invalid — redirect to login
          localStorage.removeItem('token');
          this.router.navigate(['/login']);
        }

        if (error.status === 403) {
          // No permission
          console.error('Access denied');
        }

        if (error.status === 500) {
          // Server error
          console.error('Server error, please try again later');
        }

        return throwError(() => error);
      }),
    );
  }
}
