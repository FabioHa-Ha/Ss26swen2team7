import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface RouteInfo {
    distanceKm: number;
    durationMinutes: number;
}

@Injectable({ providedIn: 'root' })
export class RouteService {

    constructor(private http: HttpClient) {}

    getRoute(from: string, to: string, transportTypeId: number): Observable<RouteInfo> {
        return this.http.get<RouteInfo>('/api/route', {
            params: { from, to, transportTypeId: transportTypeId.toString() }
        });
    }
}