import { Injectable, signal } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class FilterService {
    readonly selectedType = signal<string>('all');

    setFilter(type: string): void {
        this.selectedType.set(type);
    }
}