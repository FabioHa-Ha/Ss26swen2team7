import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { TourCardComponent } from '../tour-card/tour-card.component';
import { TourFormComponent } from '../tour-form/tour-form.component';
import { ButtonComponent } from '../../shared/ui/button/button.component';
import { TourService } from '../../../services/tour.service';
import { TourLogService } from '../../../services/tour-log.services';
import { ImageService } from '../../../services/image.service';
import { FilterService } from '../../../services/filter.service';

interface Tour {
  id: number;
  name: string;
  transportTypeName: string;
  description: string;
  fromLocation: string;
  toLocation: string;
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
  readonly showDeleteDialog = signal(false);
  readonly tourToDelete = signal<Tour | null>(null);

  constructor(private tourService: TourService, private tourLogService: TourLogService, private imageService: ImageService, public filterService: FilterService) {}

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

  onSaveTour(event: { data: any, pendingFiles: File[] }): void {
    const { data, pendingFiles } = event;
    const editing = this.editingTour();

    if (editing) {
      // UPDATE existing tour
      this.tourService.update(editing.id, data).subscribe({
        next: (updated) => {
          // upload any new images for existing tour
          this.uploadPendingFiles(pendingFiles, updated.id, () => {
            this.tours.update(tours => tours.map(t => t.id === updated.id ? updated : t));
            this.closeForm();
          })
        },
        error: (err) => console.error('Failed to update tour', err)
      });
    } 
    else 
    {
      // CREATE new tour
      this.tourService.create(data).subscribe({
        next: (created) => {
          this.uploadPendingFiles(pendingFiles, created.id, () => {
            this.tours.update(tours => [...tours, created]);
            this.closeForm();
          });
        },
        error: (err) => console.error('Failed to create tour', err)
      });
    }
  }

  private uploadPendingFiles(files: File[], tourId: number, onDone: () => void): void {
    if (!files.length) {
      onDone();
      return;
    }

    let completed = 0;
    files.forEach(file => {
      this.imageService.uploadImage(file, tourId).subscribe({
        next: () => {
          if (++completed === files.length) {
            onDone();
          }
        },
        error: (err) => {
          console.error('Image upload failed', err);
          if (++completed === files.length) {
            onDone();
          }
        }
      });
    });
  }

  onEditTour(tour: any): void {
    this.editingTour.set(tour);
    this.showForm.set(true);
  }

  onDeleteTour(tour: any): void {
    this.tourToDelete.set(tour);
    this.showDeleteDialog.set(true);
  }

  cancelDelete(): void {
    this.showDeleteDialog.set(false);
    this.tourToDelete.set(null);
  }

  confirmDelete(): void {
    const tour = this.tourToDelete();

    if (!tour) {
      return;
    }

    this.tourService.delete(tour.id).subscribe({
      next: () => {
        this.tours.update(tours => tours.filter(t => t.id !== tour.id));

        this.showDeleteDialog.set(false);
        this.tourToDelete.set(null);
      },
      error: (err) => console.error('Failed to delete tour', err)
    });
  }

  filteredTours = () => {
    const filter = this.filterService.selectedType();
    if (filter === 'all') {
      return this.tours();
    }
    return this.tours().filter(t => t.transportTypeName?.toLowerCase() === filter.toLowerCase());
  };
}
