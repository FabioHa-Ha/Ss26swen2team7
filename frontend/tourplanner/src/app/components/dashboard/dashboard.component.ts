import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent {
  formatTime(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    if (hours === 0) return `${mins}m`;
    if (mins === 0) return `${hours}h`;
    return `${hours}h ${mins}m`;
  }

  // fake stats (replace later with service)
  private readonly fakeStats = signal({
    totalTours: 4,
    totalLogs: 12,
    totalDistance: 86,
    totalTime: 540,

    toursByType: {
      bike: 2,
      hike: 1,
      running: 1,
      vacation: 0
    },

    popularTours: [
      {
        tour: { id: '1', name: 'Vienna Ride' },
        stats: { totalLogs: 5, popularity: 4 }
      }
    ],

    recentLogs: [
      {
        tour: { name: 'Vienna Ride' },
        log: {
          id: 'l1',
          date: new Date(),
          comment: 'Nice trip',
          totalDistance: 12,
          totalTime: 80
        }
      }
    ]
  });

  // mimic service API
  stats = this.fakeStats;
}
