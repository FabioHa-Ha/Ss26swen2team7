import { Component } from '@angular/core';

@Component({
  selector: 'app-search-panel',
  imports: [],
  templateUrl: './search-panel.component.html',
  styleUrl: './search-panel.component.css',
})
export class SearchPanelComponent {
  // private readonly tourService = inject(TourService);

  // readonly searchQuery = signal('');
  // readonly results = signal<SearchResult>({ tours: [], logs: [], matchedFields: new Map() });

  // onSearch(): void {
  //   const query = this.searchQuery();
  //   if (query.trim()) {
  //     this.results.set(this.tourService.search(query));
  //   } else {
  //     this.results.set({ tours: [], logs: [], matchedFields: new Map() });
  //   }
  // }

  // clearSearch(): void {
  //   this.searchQuery.set('');
  //   this.results.set({ tours: [], logs: [], matchedFields: new Map() });
  // }

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
