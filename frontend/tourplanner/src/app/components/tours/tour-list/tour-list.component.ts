import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { TourCardComponent } from '../tour-card/tour-card.component';
import { TourFormComponent } from '../tour-form/tour-form.component';
import { ButtonComponent } from '../../shared/ui/button/button.component';
import { TourService } from '../../../services/tour.service';
import { TourLogService } from '../../../services/tour-log.services';

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
  imports: [CommonModule, TourCardComponent, TourFormComponent, ButtonComponent],
  templateUrl: './tour-list.component.html',
  styleUrl: './tour-list.component.css',
})
export class TourListComponent implements OnInit {
  readonly showForm = signal(false);
  readonly editingTour = signal<Tour | null>(null);
  readonly tours = signal<Tour[]>([]);

  constructor(private tourService: TourService, private tourLogService: TourLogService) {}

  ngOnInit(): void {
    this.tourService.getMyTours().subscribe({
      next: (data) => this.tours.set(data),
      error: (err) => console.error(err)
    });
  }

  openCreateForm(): void {
    this.editingTour.set(null);
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
  }

  onSaveTour(formData: any): void {
    const editing = this.editingTour();

    if (editing) {
      // UPDATE existing tour
      this.tourService.update(editing.id, formData).subscribe({
        next: (updated) => {
          this.tours.update(tours => tours.map(t => t.id === updated.id ? updated : t));
          this.closeForm();
        },
        error: (err) => console.error('Failed to update tour', err)
      });
    } else {
      // CREATE new tour
      this.tourService.create(formData).subscribe({
        next: (created) => {
          this.tours.update(tours => [...tours, created]);
          this.closeForm();
        },
        error: (err) => console.error('Failed to create tour', err)
      });
    }
  }

  onEditTour(tour: any): void {
    this.editingTour.set(tour);
    this.showForm.set(true);
  }

  onDeleteTour(tour: any): void {
    if (!confirm(`Delete "${tour.name}"?`)) {
      return;
    }

    this.tourService.delete(tour.id).subscribe({
      next: () => this.tours.update(tours => tours.filter(t => t.id !== tour.id)),
      error: (err) => console.error('Failed to delete tour', err)
    });
  }

  readonly filter = signal<'all' | string>('all');

  filteredTours = () => {
    if (this.filter() === 'all') {
      return this.tours();
    }
    return this.tours().filter(t => t.transportType === this.filter());
  };
}
