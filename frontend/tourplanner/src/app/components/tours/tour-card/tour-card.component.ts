import { CommonModule } from '@angular/common';
import { Component, input } from '@angular/core';

@Component({
  selector: 'app-tour-card',
  imports: [CommonModule],
  templateUrl: './tour-card.component.html',
  styleUrl: './tour-card.component.css',
})
export class TourCardComponent {
  readonly tour = input.required<any>();
  readonly stats = input<any>();

  // readonly edit = output<Tour>();
  // readonly delete = output<Tour>();

  getTypeBadgeClasses(): string {
    const type = this.tour().transportType;
    const baseClasses = 'bg-opacity-20';
    const typeStyles: Record<string, string> = {
      'bike': `${baseClasses} bg-chart-1 text-chart-1`,
      'hike': `${baseClasses} bg-chart-2 text-chart-2`,
      'running': `${baseClasses} bg-chart-3 text-chart-3`,
      'vacation': `${baseClasses} bg-chart-4 text-chart-4`
    };
    return typeStyles[type] ?? `${baseClasses} bg-muted text-muted-foreground`;
  }

  getTypeIcon(): string {
    const type = this.tour().transportType;
    const icons: Record<string, string> = {
      'bike': '<svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><circle cx="5.5" cy="17.5" r="3.5"/><circle cx="18.5" cy="17.5" r="3.5"/><path d="M15 6a1 1 0 100-2 1 1 0 000 2zM12 17.5V14l-3-3 4-3 2 3h2"/></svg>',
      'hike': '<svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>',
      'running': '<svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>',
      'vacation': '<svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3.055 11H5a2 2 0 012 2v1a2 2 0 002 2 2 2 0 012 2v2.945M8 3.935V5.5A2.5 2.5 0 0010.5 8h.5a2 2 0 012 2 2 2 0 104 0 2 2 0 012-2h1.064M15 20.488V18a2 2 0 012-2h3.064M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>'
    };
    return icons[type] ?? '';
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

  // onEdit(event: Event): void {
  //   event.stopPropagation();
  //   this.edit.emit(this.tour());
  // }

  // onDelete(event: Event): void {
  //   event.stopPropagation();
  //   this.delete.emit(this.tour());
  // }
}
