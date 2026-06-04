import { CommonModule } from '@angular/common';
import { Component, effect, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface TourLog {
  id: string;
  date: Date;
  totalDistance: number;
  totalTime: number;
  difficulty: number;
  rating: number;
  comment: string;
}

@Component({
  selector: 'app-tour-log-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './tour-log-form.component.html',
})
export class TourLogFormComponent {
  readonly log = input<TourLog | null>(null);
  readonly save = output<Partial<TourLog>>();
  readonly cancel = output<void>();

  submitted = false;

  formData = {
    date: new Date().toISOString().slice(0, 16),
    comment: '',
    difficulty: 1,
    totalDistance: 0,
    totalTime: 0,
    rating: 1,
  };

  readonly difficultyOptions = [
    { value: 1, label: 'Very easy' },
    { value: 2, label: 'Easy' },
    { value: 3, label: 'Medium' },
    { value: 4, label: 'Hard' },
    { value: 5, label: 'Very hard' },
  ];

  readonly ratingOptions = [
    { value: 1, label: 'Poor' },
    { value: 2, label: 'Fair' },
    { value: 3, label: 'Good' },
    { value: 4, label: 'Very good' },
    { value: 5, label: 'Excellent' },
  ];

  constructor() {
    effect(() => {
      const log = this.log();
      if (log) {
        this.formData = {
          date: new Date(log.date).toISOString().slice(0, 16),
          comment: log.comment,
          difficulty: log.difficulty,
          totalDistance: log.totalDistance,
          totalTime: log.totalTime,
          rating: log.rating,
        };
      } else {
        this.formData = {
          date: new Date().toISOString().slice(0, 16),
          comment: '',
          difficulty: 1,
          totalDistance: 0,
          totalTime: 0,
          rating: 1,
        };
      }
      this.submitted = false;
    });
  }

  isValid(): boolean {
    return !!(
      this.formData.date &&
      this.formData.difficulty >= 1 && this.formData.difficulty <= 5 &&
      Number.isFinite(this.formData.totalDistance) && this.formData.totalDistance > 0 &&
      Number.isFinite(this.formData.totalTime) && this.formData.totalTime > 0 &&
      this.formData.rating >= 1 && this.formData.rating <= 5
    );
  }

  onSubmit(): void {
    this.submitted = true;
    if (!this.isValid()) return;
    this.save.emit({
      date: new Date(this.formData.date),
      comment: this.formData.comment,
      difficulty: this.formData.difficulty,
      totalDistance: this.formData.totalDistance,
      totalTime: this.formData.totalTime,
      rating: this.formData.rating,
    });
  }
}
