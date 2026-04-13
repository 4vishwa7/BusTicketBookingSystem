import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { HomeComponent } from './pages/home/home';
// import { ProfileComponent } from './profile/profile';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: HomeComponent },
  // { path: 'profile', component: ProfileComponent },

  { path: '', redirectTo: '/login', pathMatch: 'full' },
];
