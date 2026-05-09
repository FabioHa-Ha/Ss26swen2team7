import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ImageService {
  private apiUrl = '/api/image';

  constructor(private http: HttpClient) {}

  uploadImage(file: File, tourId: number): Observable<number> {
    const formData = new FormData();
    formData.append('image', file);
    formData.append('tourId', tourId.toString());
    return this.http.post<number>(this.apiUrl, formData);
  }

  getImageUrl(id: number): string {
    return `${this.apiUrl}/${id}`;
  }
}