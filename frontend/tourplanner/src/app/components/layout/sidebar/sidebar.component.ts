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

  // readonly tourService = inject(TourService);
  // private readonly authService = inject(AuthService);

  readonly transportTypes: { value: string; label: string; icon: string }[] = [
    { 
      value: 'bike', 
      label: 'Bike', 
      icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><circle cx="5.5" cy="17.5" r="3.5"/><circle cx="18.5" cy="17.5" r="3.5"/><path d="M15 6a1 1 0 100-2 1 1 0 000 2zM12 17.5V14l-3-3 4-3 2 3h2"/></svg>'
    },
    { 
      value: 'hike', 
      label: 'Hike', 
      icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>'
    },
    { 
      value: 'running', 
      label: 'Running', 
      icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/></svg>'
    },
    { 
      value: 'vacation', 
      label: 'Vacation', 
      icon: '<svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3.055 11H5a2 2 0 012 2v1a2 2 0 002 2 2 2 0 012 2v2.945M8 3.935V5.5A2.5 2.5 0 0010.5 8h.5a2 2 0 012 2 2 2 0 104 0 2 2 0 012-2h1.064M15 20.488V18a2 2 0 012-2h3.064M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>'
    }
  ];

  setFilter(type: string | 'all'): void {
    // this.tourService.setFilter(type);
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
