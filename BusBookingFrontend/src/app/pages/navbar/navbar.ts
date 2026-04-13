import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css'],
})
export class NavbarComponent implements OnInit {
  isLoggedIn: boolean = false;
  userName: string = '';

  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  checkLoginStatus() {
    this.isLoggedIn = this.authService.isLoggedIn();

    if (this.isLoggedIn) {
      const user = this.authService.getUser();
      this.userName = user?.name || '';
    }
  }

  goHome() {
    this.router.navigate(['/home']);
  }

  goBooking() {
    this.router.navigate(['/booking']);
  }

  handleUserClick() {
    if (this.isLoggedIn) {
      this.router.navigate(['/profile']);
    } else {
      this.router.navigate(['/login']);
    }
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    this.userName = '';
    this.router.navigate(['/login']);
  }
}
