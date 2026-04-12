import { CommonModule } from '@angular/common';
import { Component, effect, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-tour-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './tour-form.component.html',
  styleUrl: './tour-form.component.css',
})
export class TourFormComponent {
  readonly tour = input<any | null>(null);
  readonly save = output<Partial<any>>();
  readonly cancel = output<void>();

  submitted = false;

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
      } else {
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
      ...this.formData,
      distance: Math.round(Number(this.formData.distance)),
      estimatedTime: Math.round(Number(this.formData.estimatedTime))
    });
  }
}
