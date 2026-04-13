export interface BusStop {
  id: string;
  name: string;
  location: string;
  order: number;
}

export interface BusStops {
  busId: string;
  stops: BusStop[];
}
