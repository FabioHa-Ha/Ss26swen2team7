import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  // readonly collapsed = input(false);
  // readonly toggleCollapse = output<void>();
  
  collapsed = false;

  toggleSidebar() {
    this.collapsed = !this.collapsed;
  }

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
