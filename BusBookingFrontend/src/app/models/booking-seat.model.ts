import { Seat } from './seat.model';

export interface BookingSeat {
  id: string;
  bookingId: string;
  seatId: string;
  price: number;
  seat?: Seat;
}
