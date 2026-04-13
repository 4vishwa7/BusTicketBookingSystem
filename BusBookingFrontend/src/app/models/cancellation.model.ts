import { Booking } from './booking.model';

export interface Cancellation {
  id: string;
  bookingId: string;
  refundAmount: number;
  refundPercentage: number;
  cancelledAt: string;
  reason: string;
  booking?: Booking;
}

export interface CancellationPayload {
  bookingId: string;
  reason: string;
}
