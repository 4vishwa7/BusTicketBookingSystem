import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
  FareBreakdown,
  PaymentInitiateRequest,
  PaymentInitiateResponse,
  CreateBookingRequest,
  CreateBookingResponse,
} from '../models/fare.model';

@Injectable({ providedIn: 'root' })
export class FareService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  calculateFare(seatIds: string[]): Observable<FareBreakdown> {
    return this.http.post<FareBreakdown>(`${this.base}/api/fare/calculate`, { seatIds });
  }

  initiatePayment(payload: PaymentInitiateRequest): Observable<PaymentInitiateResponse> {
    return this.http.post<PaymentInitiateResponse>(`${this.base}/api/payments/initiate`, payload);
  }

  createBooking(payload: CreateBookingRequest): Observable<CreateBookingResponse> {
    return this.http.post<CreateBookingResponse>(`${this.base}/api/bookings`, payload);
  }
}
