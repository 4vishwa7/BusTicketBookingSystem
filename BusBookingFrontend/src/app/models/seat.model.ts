import { Bus } from './bus.model';

export interface Seat {
  id: string;
  busId: string;
  seatNumber: string;
  seatType: SeatType;
  bus?: Bus;
}

export type SeatType = 'Window' | 'Sleeper';

// Used in seat map UI
export interface SeatUI extends Seat {
  isBooked: boolean;
  isSelected: boolean;
  price: number;
}

export interface HoldSeatsRequest {
  busId: string;
  date: string;
  seatIds: string[];
}

export interface HoldSeatsResponse {
  success: boolean;
  reservedSeatIds: string[];
  expiresAt: string;
}
