import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Seat, HoldSeatsRequest, HoldSeatsResponse } from '../models/seat.model';

@Injectable({ providedIn: 'root' })
export class SeatService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  getBusSeats(busId: string, date: string): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.base}/api/buses/${busId}/seats?date=${date}`);
  }

  holdSeats(payload: HoldSeatsRequest): Observable<HoldSeatsResponse> {
    return this.http.post<HoldSeatsResponse>(`${this.base}/api/seats/hold`, payload);
  }
}
