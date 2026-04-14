import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { BusStops } from '../models/stop.model';

@Injectable({ providedIn: 'root' })
export class StopService {
  private base = environment.apiBase;

  constructor(private http: HttpClient) {}

  getBusStops(busId: string): Observable<BusStops> {
    return this.http.get<BusStops>(`${this.base}/api/buses/${busId}/stops`);
  }
}
