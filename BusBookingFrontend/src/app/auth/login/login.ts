import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  email    = '';
  password = '';
  isLoading = false;

  constructor(private router: Router) {}

  onLogin(form: any) {
    if (form.invalid) return;
    this.isLoading = true;
    // TODO: call AuthService.login({ email, password })
    console.log('Login:', { email: this.email, password: this.password });
    setTimeout(() => { this.isLoading = false; }, 1500);
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}