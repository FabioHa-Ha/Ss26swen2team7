import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [RouterModule, CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  // readonly collapsed = input(false);
  // readonly toggleCollapse = output<void>();
  
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

  // toggleSidebar() {
  //   this.collapsed = !this.collapsed;
  // }

  // readonly tourService = inject(TourService);
  // private readonly authService = inject(AuthService);

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

  selectedType: string | 'all' = 'all';

  setFilter(type: string | 'all'): void {
    this.selectedType = type;
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
    // const data = this.tourService.exportToJson();
    // const blob = new Blob([data], { type: 'application/json' });
    // const url = URL.createObjectURL(blob);
    // const a = document.createElement('a');
    // a.href = url;
    // a.download = 'tour-planner-export.json';
    // a.click();
    // URL.revokeObjectURL(url);
  }

  importData(event: Event): void {
    // const input = event.target as HTMLInputElement;
    // const file = input.files?.[0];
    // if (file) {
    //   const reader = new FileReader();
    //   reader.onload = () => {
    //     this.tourService.importFromJson(reader.result as string);
    //   };
    //   reader.readAsText(file);
    // }
  }

  logout(): void {
    // this.authService.logout();
  }
}
