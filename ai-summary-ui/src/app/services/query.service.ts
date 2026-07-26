import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface QueryResponse {
  answer: string;
  sources?: number[];
}

@Injectable({
  providedIn: 'root',
})
export class QueryService {

  private apiUrl = 'https://sturdy-space-funicular-55j4v59v6pfvpqw-5253.app.github.dev/api/query';
  constructor(private http: HttpClient) {}

  ask(question: string): Observable<QueryResponse>{
    const documentId = Number(localStorage.getItem('documentId'));
    return this.http.post<QueryResponse>(this.apiUrl, { question, documentId});
  }
}
