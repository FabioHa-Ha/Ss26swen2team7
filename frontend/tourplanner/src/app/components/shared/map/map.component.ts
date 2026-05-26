import { Component, Input, AfterViewInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import * as L from 'leaflet';

const iconDefault = L.icon({
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});
L.Marker.prototype.options.icon = iconDefault;

@Component({
  selector: 'app-map',
  standalone: true,
  template: `
    <div #mapContainer class="w-full h-64 rounded-lg border border-border" style="z-index: 0;"></div>
  `
})
export class MapComponent implements AfterViewInit, OnDestroy {
  @ViewChild('mapContainer') mapContainer!: ElementRef;
  @Input() fromLocation: string = '';
  @Input() toLocation: string = '';

  private map!: L.Map;

  ngAfterViewInit(): void {
    this.initMap();
  }

  private initMap(): void {
    this.map = L.map(this.mapContainer.nativeElement).setView([48.2082, 16.3738], 5);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this.map);

    Promise.all([
      this.geocode(this.fromLocation),
      this.geocode(this.toLocation)
    ]).then(([from, to]) => {
      if (from) {
        L.marker(from).addTo(this.map).bindPopup(`<b>Start:</b> ${this.fromLocation}`).openPopup();
      }
      if (to) {
        L.marker(to).addTo(this.map).bindPopup(`<b>End:</b> ${this.toLocation}`);
      }
      if (from && to) {
        const line = L.polyline([from, to], { color: '#3b82f6', weight: 3, dashArray: '6, 8' }).addTo(this.map);
        this.map.fitBounds(line.getBounds(), { padding: [40, 40] });
      } else if (from) {
        this.map.setView(from, 10);
      }
    });
  }

  private async geocode(location: string): Promise<L.LatLngTuple | null> {
    if (!location?.trim()) return null;
    try {
      const res = await fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(location)}&limit=1`,
        { headers: { 'Accept-Language': 'en' } }
      );
      const data = await res.json();
      if (!data.length) return null;
      return [parseFloat(data[0].lat), parseFloat(data[0].lon)];
    } catch {
      return null;
    }
  }

  ngOnDestroy(): void {
    if (this.map) this.map.remove();
  }
}

