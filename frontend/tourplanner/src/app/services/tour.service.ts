import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class TourService {
    private base = `${environment.apiUrl}/tour`;

    constructor(private http: HttpClient) { }

    getAll(): Observable<any[]> {
        return this.http.get<any[]>(this.base);
    }

    getMyTours(): Observable<any[]> {
        return this.http.get<any[]>(`${this.base}/my`);
    }

    getById(id: number): Observable<any[]> {
        return this.http.get<any>(`${this.base}/${id}`);
    }

    create(dto: any): Observable<any> {
        return this.http.post<any>(this.base, dto);
    }

    update(id: number, dto: any): Observable<any> {
        return this.http.put<any>(`${this.base}/${id}`, dto);
    }

    delete(id: number): Observable<any> {
        return this.http.delete<void>(`${this.base}/${id}`);
    }
}