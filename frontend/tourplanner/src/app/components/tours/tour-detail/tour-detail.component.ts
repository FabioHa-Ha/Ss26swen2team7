import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';

interface TourLog {
  id: string;
  date: Date;
  totalDistance: number;
  totalTime: number;
  difficulty: number;
  rating: number;
  comment: string;
}

interface Tour {
  id: string;
  name: string;
  from: string;
  to: string;
  description: string;
  routeInformation: string;
  transportType: string;
  distance: number;
  estimatedTime: number;
  createdAt: Date;
  updatedAt: Date;
  logs: TourLog[];
}

@Component({
  selector: 'app-tour-detail',
  imports: [CommonModule],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.css',
})
export class TourDetailComponent {
   // fake data (replace later with service)
  private readonly fakeTour = signal<Tour>({
    id: '1',
    name: 'Vienna City Ride',
    from: 'Vienna',
    to: 'Danube Island',
    description: 'A nice bike ride through Vienna.',
    routeInformation: 'Start in city center → follow Danube canal → reach island.',
    transportType: 'bike',
    distance: 12,
    estimatedTime: 90,
    createdAt: new Date(),
    updatedAt: new Date(),
    logs: [
      {
        id: 'l1',
        date: new Date(),
        totalDistance: 12,
        totalTime: 80,
        difficulty: 3,
        rating: 4,
        comment: 'Nice weather!'
      }
    ]
  });

  // mimic original API
  readonly tour = computed(() => this.fakeTour());

  readonly stats = computed(() => {
    const t = this.tour();
    if (!t) return null;

    return {
      popularity: 4,
      childFriendliness: 3,
      averageRating: 4,
      totalLogs: t.logs.length,
      totalDistance: t.logs.reduce((sum, l) => sum + l.totalDistance, 0),
      totalTime: t.logs.reduce((sum, l) => sum + l.totalTime, 0),
    };
  });

  // keep utility function
  formatTime(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    if (hours === 0) return `${mins}m`;
    if (mins === 0) return `${hours}h`;
    return `${hours}h ${mins}m`;
  }

  // dummy handlers (no logic yet)
  openLogForm() {}
  editLog(log: TourLog) {}
  closeLogForm() {}
  saveLog(data: Partial<TourLog>) {}
  deleteLog(log: TourLog) {}
}
