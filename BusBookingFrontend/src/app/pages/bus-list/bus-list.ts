import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BusService } from '../../services/bus-service';
import { Bus } from '../../models/bus.model';

@Component({
  selector: 'app-bus-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './bus-list.html',
  styleUrls: ['./bus-list.css'],
})
export class BusSearchComponent implements OnInit {
  private busService = inject(BusService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  // Signals for state management
  from = signal('Chennai');
  to = signal('Coimbatore');
  date = signal('Mon, 14 Apr 2026');
  buses = signal<Bus[]>([]);
  isLoading = signal(true);

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      if (params['from']) this.from.set(params['from']);
      if (params['to']) this.to.set(params['to']);
      if (params['date']) this.date.set(params['date']);
      this.loadBuses();
    });
  }

  loadBuses() {
    this.isLoading.set(true);
    this.busService
      .searchBuses({
        from: this.from(),
        to: this.to(),
        date: this.date(),
      })
      .subscribe({
        next: (data: Bus[]) => {
          const mappedData = data.map((bus: Bus) => ({
            ...bus,
            timings: bus.timings ?? [],
            selectedTimingIndex: 0,
          }));
          this.buses.set(mappedData);
          this.isLoading.set(false);
        },
        error: () => this.isLoading.set(false),
      });
  }

  getTagClass(tag: string): string {
    const map: Record<string, string> = {
      AC: 'tag-ac',
      Sleeper: 'tag-sleeper',
      WiFi: 'tag-wifi',
    };
    return map[tag] || 'tag-ac';
  }

  viewBus(bus: Bus): void {
    const selectedIndex = bus.selectedTimingIndex ?? 0;
    const timing = bus.timings?.[selectedIndex];
    if (!timing) {
      return;
    }

    this.router.navigate(['/bus', bus.id], {
      queryParams: {
        dep: timing.dep,
        arr: timing.arr,
        fare: timing.fare,
        from: this.from(),
        to: this.to(),
      },
    });
  }
}
