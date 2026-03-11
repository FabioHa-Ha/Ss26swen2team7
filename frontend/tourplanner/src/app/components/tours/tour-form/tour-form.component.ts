import { Component } from '@angular/core';

@Component({
  selector: 'app-tour-form',
  imports: [],
  templateUrl: './tour-form.component.html',
  styleUrl: './tour-form.component.css',
})
export class TourFormComponent {
  // readonly tour = input<Tour | null>(null);
  // readonly save = output<Partial<Tour>>();
  // readonly cancel = output<void>();

  // formData = {
  //   name: '',
  //   description: '',
  //   transportType: 'bike' as TransportType,
  //   from: '',
  //   to: '',
  //   distance: 0,
  //   estimatedTime: 0,
  //   routeInformation: ''
  // };

  // readonly transportTypes: { value: TransportType; label: string; icon: string }[] = [
  //   { 
  //     value: 'bike', 
  //     label: 'Bike', 
  //     icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><circle cx="5.5" cy="17.5" r="3.5"/><circle cx="18.5" cy="17.5" r="3.5"/><path d="M15 6a1 1 0 100-2 1 1 0 000 2zM12 17.5V14l-3-3 4-3 2 3h2"/></svg>'
  //   },
  //   { 
  //     value: 'hike', 
  //     label: 'Hike', 
  //     icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>'
  //   },
  //   { 
  //     value: 'running', 
  //     label: 'Running', 
  //     icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>'
  //   },
  //   { 
  //     value: 'vacation', 
  //     label: 'Vacation', 
  //     icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3.055 11H5a2 2 0 012 2v1a2 2 0 002 2 2 2 0 012 2v2.945M8 3.935V5.5A2.5 2.5 0 0010.5 8h.5a2 2 0 012 2 2 2 0 104 0 2 2 0 012-2h1.064M15 20.488V18a2 2 0 012-2h3.064M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>'
  //   }
  // ];

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

  // onSubmit(): void {
  //   if (this.isValid()) {
  //     this.save.emit(this.formData);
  //   }
  // }
}
