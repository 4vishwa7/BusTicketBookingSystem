import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/User.model';

export interface UpdateProfileRequest {
  name?: string;
  email?: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  // Get logged-in user profile
  getUserProfile(): Observable<User> {
    return this.http.get<User>(`${this.base}/api/users/profile`);
  }

  // Update user profile
  updateUserProfile(payload: UpdateProfileRequest): Observable<User> {
    return this.http.put<User>(`${this.base}/api/users/profile`, payload);
  }
}
