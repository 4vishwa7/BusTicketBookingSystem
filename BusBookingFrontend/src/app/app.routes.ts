import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { RegisterComponent } from './auth/register/register';
import { HomeComponent } from './pages/home/home';
import { BusSearchComponent } from './pages/bus-list/bus-list';
import { AuthGuard } from './guard/auth-guard-guard';

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
  { path: 'bus-list', component: BusSearchComponent, canActivate: [AuthGuard] },

  // { path: 'results',            component: SearchResultsComponent,   canActivate: [AuthGuard] },
  // { path: 'bus/:busId',         component: BusDetailComponent,       canActivate: [AuthGuard] },
  // { path: 'confirm/:bookingId', component: BookingConfirmComponent,  canActivate: [AuthGuard] },

  // --- Default ---
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' },
];
