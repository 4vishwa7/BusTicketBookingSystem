import { Booking } from './booking.model';

export interface Payment {
  id: string;
  bookingId: string;
  amount: number;
  status: PaymentStatus;
  paymentMethod: string;
  transactionId: string;
  createdAt: string;
  booking?: Booking;
}

export type PaymentStatus = 'Pending' | 'Success' | 'Failed' | 'Refunded';

export interface PaymentPayload {
  bookingId: string;
  paymentMethod: string;
  amount: number;
}
