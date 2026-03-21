import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';

@Component({
  selector: 'app-search-panel',
  imports: [CommonModule],
  templateUrl: './search-panel.component.html',
  styleUrl: './search-panel.component.css',
})
export class SearchPanelComponent {
  // fake input state
  readonly searchQuery = signal('');

  // fake results (replace later with service)
  readonly results = signal({
    tours: [
      {
        id: '1',
        name: 'Vienna Ride',
        transportType: 'bike',
        description: 'Nice ride',
        from: 'Vienna',
        to: 'Danube',
        distance: 12
      }
    ],
    logs: [
      {
        id: 'l1',
        tourId: '1',
        date: new Date(),
        comment: 'Great ride',
        totalDistance: 12,
        totalTime: 80,
        difficulty: 3,
        rating: 4
      }
    ],
    matchedFields: new Map()
  });

  // update query (fake search for now)
  onSearch(value: string) {
    this.searchQuery.set(value);
  }

  clearSearch() {
    this.searchQuery.set('');
  }

  formatFieldName(field: string): string {
    const names: Record<string, string> = {
      'name': 'Name',
      'description': 'Description',
      'from': 'From',
      'to': 'To',
      'transportType': 'Type',
      'routeInformation': 'Route Info',
      'popularity': 'Popular',
      'childFriendliness': 'Child-Friendly',
      'log-comment': 'Log Comment'
    };
    return names[field] || field;
  }

  formatTime(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    if (hours === 0) return `${mins}m`;
    if (mins === 0) return `${hours}h`;
    return `${hours}h ${mins}m`;
  }
}
