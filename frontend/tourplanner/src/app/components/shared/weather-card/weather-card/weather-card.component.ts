import { Component, Input, input, OnInit } from '@angular/core';
import { WeatherInfo, WeatherService } from '../../../../services/weather.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-weather-card',
  imports: [CommonModule],
  templateUrl: './weather-card.component.html',
  styleUrl: './weather-card.component.css',
})
export class WeatherCardComponent implements OnInit {
  @Input() locationName: string = '';
  @Input() label: string = '';

  weather: WeatherInfo | null = null;
  loading = true;
  error = false;

  constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.loadWeather();
  }

  private async loadWeather(): Promise<void> {
    try {
      const coords = await this.geocode(this.locationName);
      if (!coords) {
        this.error = true;
        this.loading = false;
        return;
      }

      this.weatherService.getCurrentWeather(coords[0], coords[1]).subscribe({
        next: (data) => {
          this.weather = data;
          this.loading = false;
        },
        error: () => {
          this.error = true;
          this.loading = false;
        }
      });
    }
    catch {
      this.error = true;
      this.loading = false;
    }
  }

  private async geocode(location: string): Promise<[number, number] | null> {
    try {
      const res = await fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(location)}&limit=1`
      );
      const data = await res.json();
      if (!data.length) {
        return null;
      }

      return [parseFloat(data[0].lat), parseFloat(data[0].lon)];
    }
    catch {
      return null;
    }
  }
}
