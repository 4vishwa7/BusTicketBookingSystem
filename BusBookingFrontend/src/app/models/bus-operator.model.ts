import { Bus } from './bus.model';

export interface BusOperator {
  id: string;
  name: string;
  contactEmail: string;
  phone: string;
  isActive: boolean;
  buses?: Bus[];
}
