import { Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { UserService, UserProfile } from '../services/user.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class ProfileComponent implements OnInit {
  private userService = inject(UserService);
  private router = inject(Router);

  // Initialize with null or an empty object
  user = signal<UserProfile | null>(null);
  isLoading = signal(true);

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    this.isLoading.set(true);
    this.userService.getProfile().subscribe({
      next: (data) => {
        this.user.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load profile', err);
        this.isLoading.set(false);
        // Optional: Redirect to login if unauthorized
      }
    });
  }

  logout() {
    localStorage.removeItem('token'); // Clear your session
    this.router.navigate(['/login']);
  }
}