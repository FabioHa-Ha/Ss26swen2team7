import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { FilterService } from '../../../services/filter.service';
import { ImportExportService } from '../../../services/import-export.service';

@Component({
  selector: 'app-sidebar',
  imports: [RouterModule, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  importSuccess = false;
  importError = false;


  constructor(private authService: AuthService, private router: Router, private filterService: FilterService, private importExportService: ImportExportService) {}
  
  someTours = [
    {
      id: '1',
      name: 'Vienna City Ride',
      transportType: 'bike',
      distance: 18,
      duration: 95,
    },
    {
      id: '2',
      name: 'Danube Hiking Trail',
      transportType: 'hike',
      distance: 12,
      duration: 180,
    },
    {
      id: '3',
      name: 'Prater Park Run',
      transportType: 'running',
      distance: 5,
      duration: 28,
    },
    {
      id: '4',
      name: 'Alpine Weekend Trip',
      transportType: 'vacation',
      distance: 240,
      duration: 600,
    }
  ];

  collapsed = false;
  isMobileOpen = false;

  openMobile() {
    this.isMobileOpen = true;
  }

  closeMobile() {
    this.isMobileOpen = false;
  }

  toggleSidebar() {
    this.collapsed = !this.collapsed; // desktop only
  }

  readonly transportTypes: { value: string; label: string; icon: string }[] = [
    { 
      value: 'bike', 
      label: 'Bike', 
      icon: 'bike'
    },
    { 
      value: 'hike', 
      label: 'Hike', 
      icon: 'hike'
    },
    { 
      value: 'running', 
      label: 'Running', 
      icon: 'running'
    },
    { 
      value: 'vacation', 
      label: 'Vacation', 
      icon: 'vacation'
    }
  ];

  get selectedType() {
    return this.filterService.selectedType();
  }

  setFilter(type: string): void {
    this.filterService.setFilter(type);
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

  exportData(): void {
    this.importExportService.export().subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'tour-planner-export.zip';
        a.click();
        URL.revokeObjectURL(url);
      },
      error: (err) => console.error('Export failed', err)
    });
  }

  importData(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) {
      return;
    }

    this.importExportService.import(file).subscribe({
      next: () => {
        this.importSuccess = true;
        setTimeout(() => this.importSuccess = false, 3000);
        input.value = '';
      },
      error: (err) => {
        console.error('Import failed', err);
        this.importError = true;
        setTimeout(() => this.importError = false, 3000);
      }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
