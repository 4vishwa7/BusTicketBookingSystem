import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BusList } from './pages/bus-list/bus-list';
import { ProfileComponent } from './profile/profile';
import { BusBookingComponent } from './bus-booking/bus-booking';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BusList, ProfileComponent, BusBookingComponent],
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
})
export class App {
  protected readonly title = signal('BusBookingFrontend');
}
