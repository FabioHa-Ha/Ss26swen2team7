import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

export interface SearchResults {
    tours: any[];
    logs: any[];
}

@Injectable({ providedIn: 'root' })
export class SearchService {
    constructor(private http: HttpClient) {}

    search(query: string): Observable<SearchResults> {
        return this.http.get<SearchResults>('/api/search', {
            params: { query }
        });
    }
}