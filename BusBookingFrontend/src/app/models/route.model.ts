import { Trip } from './trip.model';

export interface Route {
  id: string;
  source: string;
  destination: string;
  distanceKm: number;
  trips?: Trip[];
}
