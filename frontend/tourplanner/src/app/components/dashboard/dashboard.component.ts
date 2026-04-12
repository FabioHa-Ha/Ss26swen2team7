import { CommonModule } from '@angular/common';
import { Component, signal, computed, OnInit } from '@angular/core';
import { TourService } from '../../services/tour.service';
import { TourLogService } from '../../services/tour-log.services';
import { map } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit{
  private tours: any[] = [];
  private logs: any[] = [];

  stats = signal({
    totalTours: 0,
    totalLogs: 0,
    totalDistance: 0,
    totalTime: 0,
    toursByType: { bike: 0, hike: 0, running: 0, vacation: 0},
    popularTours: [] as any[],
    recentLogs: [] as any[]
  });

  constructor(private tourService: TourService, private tourLogService: TourLogService) {}

  ngOnInit(): void {
    this.tourService.getMyTours().subscribe({
      next: (tours) => {
        this.tours = tours;
        this.tourLogService.getMyLogs().subscribe({
          next: (logs) => {
            this.logs = logs;
            this.computeStats();
          },
          error: (err) => console.error('Failed to load logs', err)
        });
      },
      error: (err) => console.error('Failed to load tours', err)
    });
  }

  private computeStats(): void {
    const toursByType = { bike: 0, hike: 0, running: 0, vacation: 0 };
    this.tours.forEach(t => {
      const type = t.transportType?.toLowerCase();
      if (type in toursByType) {
        toursByType[type as keyof typeof toursByType]++;
      }
    });

    // popular tours: tours sorted by how many logs they have
    const logCountByTour = new Map<number, number>();
    this.logs.forEach(l => {
      logCountByTour.set(l.tourId, (logCountByTour.get(l.tourId) ?? 0) + 1);
    });

    const popularTours = this.tours
      .map(tour => ({
        tour,
        stats: { totallogs: logCountByTour.get(tour.id) ?? 0 }
      }))
      .filter(x => x.stats.totallogs > 0)
      .sort((a, b) => b.stats.totallogs - a.stats.totallogs)
      .slice(0, 5);

      // recent logs: last 5 logs with their tour info
      const recentLogs = [...this.logs]
        .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
        .slice(0, 5)
        .map(log => ({
          log,
          tour: this.tours.find(t => t.id === log.tourId) ?? { name: 'Unknown' }
        }));

      this.stats.set({
        totalTours: this.tours.length,
        totalLogs: this.logs.length,
        totalDistance: this.logs.reduce((sum, l) => sum + (l.totalDistance ?? 0), 0),
        totalTime: this.logs.reduce((sum, l) => sum + (l.totalTime ?? 0), 0),
        toursByType,
        popularTours,
        recentLogs
      });
  }


  formatTime(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    if (hours === 0) return `${mins}m`;
    if (mins === 0) return `${hours}h`;
    return `${hours}h ${mins}m`;
  }
}
