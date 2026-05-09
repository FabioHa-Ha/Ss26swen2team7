import { CommonModule } from '@angular/common';
import { Component, signal, computed, OnInit, effect } from '@angular/core';
import { TourLogFormComponent } from '../tour-log-form/tour-log-form.component';
import { ActivatedRoute } from '@angular/router';
import { TourService } from '../../../services/tour.service';
import { TourLogService } from '../../../services/tour-log.services';
import { ImageService } from '../../../services/image.service';

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
export class TourDetailComponent implements OnInit {
  imageUrls: string[] = [];
  lightboxUrl: string | null = null;
  showLogForm = signal(false);
  editingLog = signal<any>(null);

  private readonly _tour = signal<any>(null);
  private readonly _logs = signal<any[]>([]);

  readonly tour = computed(() => {
    const t = this._tour();
    if (!t) {
      return null;
    }
    return { ...t, logs: this._logs() };
  });

  readonly stats = computed(() => {
    const logs = this._logs();
    if (!logs.length) {
      return null;
    }
    return {
      totalLogs: logs.length,
      totalDistance: logs.reduce((sum, l) => sum + (l.totalDistance ?? 0), 0),
      totalTime: logs.reduce((sum, l) => sum + (l.totalTime ?? 0), 0),
    };
  });

  readonly logCountLabel = computed(() => {
    const count = this._logs().length;
    if (count === 0) {
      return 'No logs yet';
    }

    if (count === 1) {
      return '1 log';
    }
    return `${count} logs`;
  })

  constructor(private route: ActivatedRoute, private tourService: TourService, private tourLogService: TourLogService, private imageService: ImageService) {
    effect(() => {
      const count = this._logs().length;
      console.log(`[Observer] Log count changed: ${count}`);
    });
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.tourService.getById(id).subscribe({
      next: (tour: any) => {
        this._tour.set(tour);
        this.imageUrls = (tour.imageIds ?? []).map((id: number) =>
          this.imageService.getImageUrl(id)
        );
      },
      error: (err) => console.error('Failed to load tour', err)
    });

    this.tourLogService.getByTour(id).subscribe({
      next: (logs) => this._logs.set(logs),
      error: (err) => console.error('Failed to load logs', err)
    });
  }

  openLightbox(url: string) {
    this.lightboxUrl = url;
  }

  closeLightbox() {
    this.lightboxUrl = null;
  }

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

  saveLog(data: any) {
    const editing = this.editingLog();
    const tourId = this._tour()?.id;

    if (editing) {
      this.tourLogService.update(editing.id, data).subscribe({
        next: (updated) => {
          this._logs.update(logs => logs.map(l => l.id === updated.id ? updated : l));
          this.closeLogForm();
        },
        error: (err) => console.error('Failed to update log', err)
      });
    } else {
      this.tourLogService.create({ ...data, tourId }).subscribe({
        next: (created) => {
          this._logs.update(logs => [...logs, created]);
          this.closeLogForm();
        },
        error: (err) => console.error('Failed to create log', err)
      });
    }
  }

  deleteLog(log: any) {
    if (!confirm('Delete this log?')) {
      return;
    }

    this.tourLogService.delete(log.id).subscribe({
      next: () => this._logs.update(logs => logs.filter(l => l.id !== log.id)),
      error: (err) => console.error('Failed to delete log', err)
    });
  }
}
