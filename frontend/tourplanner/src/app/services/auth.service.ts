import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private base = `${environment.apiUrl}/user`;

    constructor(private http: HttpClient) {}

    register(dto: { username: string, email: string, password: string }): Observable<any> {
        return this.http.post<any>(`${this.base}/register`, dto);
    }

    login(dto: { username: string, password: string }): Observable<any> {
        return this.http.post<any>(`${this.base}/login`, dto).pipe(
            tap(response => {
                if (response?.token) {
                    localStorage.setItem('token', response.token);
                }
            })
        );
    }

    logout(): void {
        localStorage.removeItem('token');
    }

    isLoggedIn(): boolean {
        return !!localStorage.getItem('token');
    }
}