import { Bus } from './bus.model';
import { Route } from './route.model';

export interface Trip {
  id: string;
  busId: string;
  routeId: string;
  departureTime: string;
  arrivalTime: string;
  basePrice: number;
  availableSeats: number;
  bus?: Bus;
  route?: Route;
}

// Used in search results UI
export interface TripSearchResult {
  trip: Trip;
  operatorName: string;
  busType: string;
  busNumber: string;
  source: string;
  destination: string;
  departureTime: string;
  arrivalTime: string;
  basePrice: number;
  availableSeats: number;
  durationMinutes: number;
}

// Payload to search trips
export interface TripSearchPayload {
  source: string;
  destination: string;
  date: string; // yyyy-MM-dd
}
