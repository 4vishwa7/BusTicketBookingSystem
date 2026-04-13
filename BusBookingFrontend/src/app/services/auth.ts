import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/User.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = environment.apiBase;
  private currentUser: User | null = null;

  constructor(private http: HttpClient) {}

  isLoggedIn(): boolean {
    return !!this.currentUser;
  }

  getUser(): User | null {
    return this.currentUser;
  }

  logout(): void {
    this.currentUser = null;
  }

  getUserProfile(): Observable<User> {
    return this.http.get<User>(`${this.base}/api/users/profile`, {
      withCredentials: true,
    });
  }
}
