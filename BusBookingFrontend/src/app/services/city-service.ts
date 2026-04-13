import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { City } from '../models/city.model';

@Injectable({ providedIn: 'root' })
export class CityService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  getAllCities(): Observable<City[]> {
    return this.http.get<City[]>(`${this.base}/api/cities`);
  }
}
