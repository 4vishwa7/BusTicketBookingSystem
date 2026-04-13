import { BusOperator } from './bus-operator.model';
import { Seat } from './seat.model';
export interface Bus {
  id: string;
  busNumber: string;
  totalSeats: number;
  busType: BusType;
  operatorId: string;
  operator?: BusOperator;
  seats?: Seat[];
}

export type BusType = 'AC' | 'Sleeper';

export interface BusSearchParams {
  from: string;
  to: string;
  date: string;
  sort?: string;
  filter?: string;
}
