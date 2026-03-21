import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { TourCardComponent } from '../tour-card/tour-card.component';
import { TourFormComponent } from '../tour-form/tour-form.component';

interface Tour {
  id: number;
  name: string;
  transportType: string;
  description: string;
  from: string;
  to: string;
  distance: number;
  estimatedTime: number;
}

@Component({
  selector: 'app-tour-list',
  imports: [CommonModule, TourCardComponent, TourFormComponent],
  templateUrl: './tour-list.component.html',
  styleUrl: './tour-list.component.css',
})
export class TourListComponent {
  // fake data
  readonly tours = signal<Tour[]>([
    {
      id: 1,
      name: 'Wien Rundgang',
      transportType: 'hike',
      description: 'Schöner Spaziergang durch die Innenstadt',
      from: 'Stephansplatz',
      to: 'Schönbrunn',
      distance: 8.5,
      estimatedTime: 120
    },
    {
      id: 2,
      name: 'Donau Radweg',
      transportType: 'bike',
      description: 'Entlang der Donau',
      from: 'Krems',
      to: 'Melk',
      distance: 36,
      estimatedTime: 150
    },
    {
      id: 3,
      name: 'Alpen Tour',
      transportType: 'hike',
      description: 'Bergwanderung mit Aussicht',
      from: 'Talstation',
      to: 'Gipfel',
      distance: 12,
      estimatedTime: 300
    }
  ]);

  readonly filter = signal<'all' | string>('all');

  filteredTours = () => {
    if (this.filter() === 'all') return this.tours();
    return this.tours().filter(t => t.transportType === this.filter());
  };
}
