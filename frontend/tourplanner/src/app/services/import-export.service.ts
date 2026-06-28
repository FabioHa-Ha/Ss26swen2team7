import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImportExportService {
    private base = '/api';
    
    constructor(private http: HttpClient) {}
    
    export(): Observable<Blob> {
        return this.http.get(`${this.base}/export`, { responseType: 'blob' });
    }

    import(file: File): Observable<void> {
        const formData = new FormData();
        formData.append('importFile', file);
        return this.http.post<void>(`${this.base}/import`, formData);
    }
}