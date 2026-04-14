import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BusService } from '../services/bus-service';
import { Bus } from '../models/bus.model';
import { Seat } from '../models/seat.model';
import { BusStop as BoardingPoint } from '../models/stop.model';

@Component({
  selector: 'app-bus-booking',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './bus-booking.html',
  styleUrls: ['./bus-booking.css'],
})
export class BusBookingComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private busService = inject(BusService);

  // Stepper State
  currentStep = signal(1); // 1: Seats, 2: Points, 3: Info

  // Data Signals
  bus = signal<Bus | null>(null);
  selectedSeats = signal<Seat[]>([]);
  boardingPoint = signal<BoardingPoint | null>(null);
  droppingPoint = signal<BoardingPoint | null>(null);

  // Form Signals (Image 2)
  email = signal('vishwanathan.p2004@gmail.com');
  phone = signal('');
  pName = signal('');
  pAge = signal('');
  pGender = signal<'Male' | 'Female' | null>(null);

  // Computed Values
  totalFare = computed(() => this.selectedSeats().length * 700);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.busService.getBusDetails(id).subscribe((data) => this.bus.set(data));
    }
  }

  toggleSeat(seat: Seat) {
    if (seat.isSold) return;
    this.selectedSeats.update((prev) =>
      prev.find((s) => s.id === seat.id) ? prev.filter((s) => s.id !== seat.id) : [...prev, seat],
    );
  }

  nextStep() {
    this.currentStep.update((s) => s + 1);
    window.scrollTo(0, 0);
  }
}
