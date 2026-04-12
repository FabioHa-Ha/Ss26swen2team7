import { CommonModule } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-tour-card',
  imports: [CommonModule, RouterModule],
  templateUrl: './tour-card.component.html',
  styleUrl: './tour-card.component.css',
})
export class TourCardComponent {
  readonly tour = input.required<any>();
  readonly stats = input<any>();

  readonly edit = output<any>();
  readonly delete = output<any>();

  getTypeBadgeClasses(): string {
    const type = this.tour().transportTypeName?.toLowerCase();
    const baseClasses = 'bg-opacity-20';
    const typeStyles: Record<string, string> = {
      'bike': `${baseClasses} bg-chart-1 text-chart-1`,
      'hike': `${baseClasses} bg-chart-2 text-chart-2`,
      'running': `${baseClasses} bg-chart-3 text-chart-3`,
      'vacation': `${baseClasses} bg-chart-4 text-chart-4`
    };
    return typeStyles[type] ?? `${baseClasses} bg-muted text-muted-foreground`;
  }

  formatTime(minutes: number): string {
    if (minutes == null || isNaN(Number(minutes))) {
      return '-';
    }

    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;

    if (hours === 0) {
      return `${mins}m`;
    }

    if (mins === 0) {
      return `${hours}h`;
    }
    return `${hours}h ${mins}m`;
  }

  onEdit(event: Event): void {
    event.stopPropagation();
    this.edit.emit(this.tour());
  }

  onDelete(event: Event): void {
    event.stopPropagation();
    this.delete.emit(this.tour());
  }
}
