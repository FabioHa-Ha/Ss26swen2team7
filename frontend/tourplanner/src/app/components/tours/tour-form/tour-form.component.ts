import { CommonModule } from '@angular/common';
import { Component, effect, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ImageService } from '../../../services/image.service';
import { RouteService } from '../../../services/route-service';

@Component({
  selector: 'app-tour-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './tour-form.component.html',
  styleUrl: './tour-form.component.css',
})
export class TourFormComponent {
  private imageService = inject(ImageService);
  private routeService = inject(RouteService);

  routeLoading = false;
  routeError = false;

  readonly tour = input<any | null>(null);
  readonly save = output<{ data: Partial<any>, pendingFiles: File[] }>();
  readonly cancel = output<void>();

  submitted = false;
  pendingFiles: File[] = [];
  existingImageIds: number[] = [];
  imageUploading = false;

  readonly transportTypes = [
    { value: 1, label: 'Bike', icon: 'bike' },
    { value: 2, label: 'Hike', icon: 'hike' },
    { value: 3, label: 'Running', icon: 'running' },
    { value: 4, label: 'Vacation', icon: 'vacation' }
  ];

  formData = {
    name: '',
    description: '',
    transportTypeId: 1,
    fromLocation: '',
    toLocation: '',
    distance: 0,
    estimatedTime: 0,
    routeInformation: ''
  };

  constructor() {
    effect(() => {
      const tour = this.tour();
      if (tour) {
        this.existingImageIds = tour.imageIds ?? [];
        this.pendingFiles = [];
        this.formData = {
          name: tour.name,
          description: tour.description,
          transportTypeId: tour.transportTypeId,
          fromLocation: tour.fromLocation,
          toLocation: tour.toLocation,
          distance: tour.distance,
          estimatedTime: tour.estimatedTime,
          routeInformation: tour.routeInformation
        };
      } 
      else 
      {
        this.existingImageIds = [];
        this.pendingFiles = [];
        this.formData = {
          name: '',
          description: '',
          transportTypeId: 1,
          fromLocation: '',
          toLocation: '',
          distance: 0,
          estimatedTime: 0,
          routeInformation: ''
        };
      }
      this.submitted = false;
    });
  }

  fetchRoute(): void {
    const { fromLocation, toLocation, transportTypeId } = this.formData;
    if (!fromLocation?.trim() || !toLocation?.trim()) {
      return;
    }

    this.routeLoading = true;
    this.routeError = false;

    this.routeService.getRoute(fromLocation, toLocation, transportTypeId).subscribe({
      next: (info) => {
        this.formData.distance = info.distanceKm;
        this.formData.estimatedTime = info.durationMinutes;
        this.routeLoading = false;
      },
      error: () => {
        this.routeError = true;
        this.routeLoading = false;
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const files = Array.from(input.files ?? []);
    
    if (!files.length) {
      return;
    }

    this.pendingFiles = [...this.pendingFiles, ...files];
  }

  removePendingFile(index: number): void {
    this.pendingFiles = this.pendingFiles.filter((_, i) => i !== index);
  }

  isValid(): boolean {
    return !!(
      this.formData.name?.trim() &&
      this.formData.fromLocation?.trim() &&
      this.formData.toLocation?.trim() &&
      this.formData.distance > 0 &&
      this.formData.estimatedTime > 0
    );
  }

  onSubmit(): void {
    this.submitted = true;
    if (!this.isValid()) {
      return;
    }
    this.save.emit({
      data: {
        ...this.formData,
        imageIds: this.existingImageIds,
        distance: Math.round(Number(this.formData.distance)),
        estimatedTime: Math.round(Number(this.formData.estimatedTime))
      },
      pendingFiles: this.pendingFiles
    });
  }
}
