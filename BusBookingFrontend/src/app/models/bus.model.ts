import { BusOperator } from './bus-operator.model';
import { Seat } from './seat.model';
export interface BusTiming {
  dep: string;
  arr: string;
  dur: string;
  fare: number;
}

export interface Bus {
  id: string;
  name: string;
  type: string;
  rating: number;
  totalSeats: number;
  busType: BusType;
  operatorId: string;
  operator?: BusOperator;
  seats: number;
  seatDetails?: Seat[];
  tags: string[];
  timings: BusTiming[];
  selectedTimingIndex: number;
}

export type BusType = 'AC' | 'Sleeper';

export interface BusSearchParams {
  from: string;
  to: string;
  date: string;
  sort?: string;
  filter?: string;
}

export interface BusSearchParams {
  from: string;
  to: string;
  date: string;
  sort?: string;
  filter?: string;
}
