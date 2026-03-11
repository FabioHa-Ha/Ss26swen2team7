import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TourCardComponent } from '../tour-card/tour-card.component';
import { TourFormComponent } from '../tour-form/tour-form.component';

@Component({
  selector: 'app-tour-list',
  imports: [CommonModule, TourCardComponent, TourFormComponent],
  templateUrl: './tour-list.component.html',
  styleUrl: './tour-list.component.css',
})
export class TourListComponent {

}
