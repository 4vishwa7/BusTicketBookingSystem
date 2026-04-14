import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Bus, BusSearchParams } from '../models/bus.model';

@Injectable({ providedIn: 'root' })
export class BusService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  searchBuses(params: BusSearchParams): Observable<Bus[]> {
    return this.http.get<Bus[]>(
      `${this.base}/api/buses/search?from=${params.from}&to=${params.to}&date=${params.date}&sort=${params.sort || ''}&filter=${params.filter || ''}`,
    );
  }

  getBusDetails(busId: string): Observable<Bus> {
    return this.http.get<Bus>(`${this.base}/api/buses/${busId}`);
  }
}
