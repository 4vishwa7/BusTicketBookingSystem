import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { HomeComponent } from './pages/home/home';
import { BusList } from './pages/bus-list/bus-list';
import { AuthGuard } from './guard/auth-guard-guard';
import { ProfileComponent } from './profile/profile';

// TODO: create these components
// ng generate component pages/search-results
// ng generate component pages/bus-detail
// ng generate component pages/booking-confirm

export const routes: Routes = [
  // --- Public Routes (no guard) ---
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // --- Protected Routes ---
  { path: 'home', component: HomeComponent },
  { path: 'bus-list', component: BusList, canActivate: [AuthGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard] },

  // { path: 'results',            component: SearchResultsComponent,   canActivate: [AuthGuard] },
  // { path: 'bus/:busId',         component: BusDetailComponent,       canActivate: [AuthGuard] },
  // { path: 'confirm/:bookingId', component: BookingConfirmComponent,  canActivate: [AuthGuard] },

  // --- Default ---
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' },
];
