export interface FareBreakdown {
  baseFare: number;
  taxes: number;
  discounts: number;
  totalFare: number;
  breakdown?: Array<{ label: string; amount: number }>;
}

export interface PaymentInitiateRequest {
  bookingId: string;
  paymentMethod: string;
  amount: number;
}

export interface PaymentInitiateResponse {
  paymentId: string;
  paymentUrl: string;
  expiresAt: string;
}

export interface CreateBookingRequest {
  tripId: string;
  seatIds: string[];
  passengerDetails: Array<{ name: string; age: number; gender: string; seatId: string }>;
  paymentMethod: string;
}

export interface CreateBookingResponse {
  bookingId: string;
  success: boolean;
}
