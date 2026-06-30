import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface WeatherInfo {
    place: string;
    temperature: number;
    feelsLikeTemperature: number;
    windSpeed: number;
    humidity: number;
    uvIndex: number;
    chanceOfRain: number;
}

@Injectable({ providedIn: 'root' })
export class WeatherService {
    constructor(private http: HttpClient) {}

    getCurrentWeather(lat: number, lon: number): Observable<WeatherInfo> {
        return this.http.get<WeatherInfo>(`/api/currentweather/${lat}/${lon}`);
    }
}