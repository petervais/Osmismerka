import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WordSearchService {
  private apiUrl = 'http://localhost:5121/api/WordSearch/SearchWords';

  constructor(private http: HttpClient) { }

  searchWords(matrix: string[][], words: string[]): Observable<any> {
    return this.http.post(this.apiUrl, { Matrix: matrix, Words: words });
  }
}
