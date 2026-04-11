import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';
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

  formData = {
    name: '',
    description: '',
    transportType: 'bike',
    from: '',
    to: '',
    distance: 0,
    estimatedTime: 0,
    routeInformation: ''
  };

  readonly transportTypes = [
    { 
      value: 'bike', 
      label: 'Bike'
    },
    { 
      value: 'hike', 
      label: 'Hike'
    },
    { 
      value: 'running', 
      label: 'Running'
    },
    { 
      value: 'vacation', 
      label: 'Vacation'
    }
  ];

  // constructor() {
  //   effect(() => {
  //     const tour = this.tour();
  //     if (tour) {
  //       this.formData = {
  //         name: tour.name,
  //         description: tour.description,
  //         transportType: tour.transportType,
  //         from: tour.from,
  //         to: tour.to,
  //         distance: tour.distance,
  //         estimatedTime: tour.estimatedTime,
  //         routeInformation: tour.routeInformation
  //       };
  //     } else {
  //       this.formData = {
  //         name: '',
  //         description: '',
  //         transportType: 'bike',
  //         from: '',
  //         to: '',
  //         distance: 0,
  //         estimatedTime: 0,
  //         routeInformation: ''
  //       };
  //     }
  //   });
  // }

  // isValid(): boolean {
  //   return !!(
  //     this.formData.name &&
  //     this.formData.from &&
  //     this.formData.to &&
  //     this.formData.distance > 0 &&
  //     this.formData.estimatedTime > 0
  //   );
  // }

  onSubmit(): void {
    console.log('Saved:', this.formData);
    this.save.emit(this.formData);
  }
}
