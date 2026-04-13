import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BookingDetail, NotificationRequest } from '../models/booking.model';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  getBookingById(bookingId: string): Observable<BookingDetail> {
    return this.http.get<BookingDetail>(`${this.base}/api/bookings/${bookingId}`);
  }

  sendNotification(payload: NotificationRequest): Observable<{ success: boolean }> {
    return this.http.post<{ success: boolean }>(`${this.base}/api/notifications/send`, payload);
  }
}
