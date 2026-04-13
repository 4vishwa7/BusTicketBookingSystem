import { User } from './User.model';
import { Trip } from './trip.model';
import { BookingSeat } from './booking-seat.model';

import { Payment } from './payment.model';

export interface Booking {
  id: string;
  userId: string;
  tripId: string;
  totalAmount: number;
  status: BookingStatus;
  createdAt: string;
  confirmedAt?: string;
  user?: User;
  trip?: Trip;
  bookingSeats?: BookingSeat[];
  payment?: Payment;
}

export type BookingStatus = 'Pending' | 'Confirmed' | 'Cancelled';

// Payload sent to backend when creating a booking
export interface CreateBookingPayload {
  tripId: string;
  seatIds: string[];
  boardingPoint: string;
  droppingPoint: string;
  passengerDetails: PassengerDetail[];
}

export interface PassengerDetail {
  name: string;
  age: number;
  gender: Gender;
  seatId: string;
}

export type Gender = 'Male' | 'Female' | 'Other';

// Response after booking is created
export interface BookingResponse {
  booking: Booking;
  paymentId: string;
}

export type BookingDetail = Booking;

export interface NotificationRequest {
  bookingId: string;
  message: string;
}
