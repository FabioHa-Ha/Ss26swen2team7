import { CommonModule } from '@angular/common';
import { Component, signal, computed } from '@angular/core';
import { SearchResults, SearchService } from '../../../services/search.service';
import { debounceTime, distinctUntilChanged, EMPTY, Subject, switchMap } from 'rxjs';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-search-panel',
  imports: [CommonModule, RouterModule],
  templateUrl: './search-panel.component.html',
  styleUrl: './search-panel.component.css',
})
export class SearchPanelComponent {
  readonly searchQuery = signal('');
  readonly results = signal<SearchResults>({ tours: [], logs: [] });
  readonly loading = signal(false);

  private searchObject = new Subject<string>();

  constructor(private searchService: SearchService) {
    this.searchObject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(q => {
        const term = q.trim();

        if (!term) {
          this.results.set({ tours: [], logs: [] });
          this.loading.set(false);
          return EMPTY;
        }

        this.loading.set(true);
        return this.searchService.search(term);
      })
    ).subscribe({
      next: (res) => {
        this.results.set(res);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onSearch(value: string) {
    this.searchQuery.set(value);
    this.searchObject.next(value);
  }

  clearSearch() {
    this.searchQuery.set('');
    // this.results.set({ tours: [], logs: [] });
    this.searchObject.next('');
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
