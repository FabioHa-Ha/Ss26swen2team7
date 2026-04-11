import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';
import { TourLogFormComponent } from '../tour-log-form/tour-log-form.component';

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
  imports: [CommonModule, TourLogFormComponent],
  templateUrl: './tour-detail.component.html',
  styleUrl: './tour-detail.component.css',
})
export class TourDetailComponent {
  showLogForm = signal(false);
  editingLog = signal<TourLog | null>(null);

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
    const tour = this.tour();
    if (!tour) {
      return null;
    }

    return {
      popularity: 4,
      childFriendliness: 3,
      averageRating: 4,
      totalLogs: tour.logs.length,
      totalDistance: tour.logs.reduce((sum, l) => sum + l.totalDistance, 0),
      totalTime: tour.logs.reduce((sum, l) => sum + l.totalTime, 0),
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

  openLogForm() {
    this.editingLog.set(null);
    this.showLogForm.set(true);
  }

  editLog(log: TourLog) {
    this.editingLog.set(log);
    this.showLogForm.set(true);
  }

  closeLogForm() {
    this.showLogForm.set(false);
    this.editingLog.set(null);
  }

  saveLog(data: Partial<TourLog>) {
    const editing = this.editingLog();
    if (editing) {
      this.fakeTour.update(t => ({
        ...t,
        logs: t.logs.map(l => l.id === editing.id ? { ...l, ...data } as TourLog : l)
      }));
    } else {
      const newLog: TourLog = {
        id: Date.now().toString(),
        date: data.date ?? new Date(),
        totalDistance: data.totalDistance ?? 0,
        totalTime: data.totalTime ?? 0,
        difficulty: data.difficulty ?? 1,
        rating: data.rating ?? 1,
        comment: data.comment ?? '',
      };
      this.fakeTour.update(t => ({ ...t, logs: [...t.logs, newLog] }));
    }
    this.closeLogForm();
  }

  deleteLog(log: TourLog) {
    this.fakeTour.update(t => ({ ...t, logs: t.logs.filter(l => l.id !== log.id) }));
  }
}
