import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { City } from '../../models/city.model';
import { CityService } from '../../services/city-service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
})
export class HomeComponent implements OnInit {
  fromCityId: number | null = null;
  toCityId: number | null = null;
  travelDate: string = '';

  cities: City[] = [];

  constructor(
    private router: Router,
    private cityService: CityService,
  ) {}

  ngOnInit(): void {
    this.getCities();
  }

  getCities() {
    this.cityService.getAllCities().subscribe({
      next: (res: City[]) => {
        this.cities = res;
      },
      error: (err: any) => {
        console.error('Error fetching cities', err);
      },
    });
  }

  searchBuses() {
    if (!this.fromCityId || !this.toCityId || !this.travelDate) {
      alert('Please fill all fields');
      return;
    }

    if (this.fromCityId === this.toCityId) {
      alert('From and To cannot be same');
      return;
    }

    const fromCity = this.cities.find((c) => c.id === this.fromCityId)?.name;
    const toCity = this.cities.find((c) => c.id === this.toCityId)?.name;

    this.router.navigate(['/bus-list'], {
      queryParams: {
        from: fromCity,
        to: toCity,
        date: this.travelDate,
      },
    });
  }
}
