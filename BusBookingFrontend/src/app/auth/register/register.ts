import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {
  firstName       = '';
  lastName        = '';
  email           = '';
  phone           = '';
  dob             = '';
  gender          = 'male';
  password        = '';
  confirmPassword = '';
  terms           = false;
  isLoading       = false;

  constructor(private router: Router) {}

  passwordsMatch(): boolean {
    return this.password === this.confirmPassword;
  }

  onRegister(form: any) {
    if (form.invalid || !this.passwordsMatch()) {
      form.form.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    // TODO: call AuthService.register({...})
    console.log('Register:', {
      firstName: this.firstName,
      lastName:  this.lastName,
      email:     this.email,
      phone:     this.phone,
      dob:       this.dob,
      gender:    this.gender,
      password:  this.password
    });
    setTimeout(() => {
      this.isLoading = false;
      this.router.navigate(['/login']);
    }, 1500);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}