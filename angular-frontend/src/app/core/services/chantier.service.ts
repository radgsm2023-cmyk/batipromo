import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Chantier } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class ChantierService {
  private apiUrl = `${environment.apiUrl}/chantiers`;

  constructor(private http: HttpClient) {}

  getChantiers(): Observable<Chantier[]> {
    return this.http.get<Chantier[]>(this.apiUrl);
  }

  getChantierById(id: number): Observable<Chantier> {
    return this.http.get<Chantier>(`${this.apiUrl}/${id}`);
  }

  createChantier(chantier: Partial<Chantier>): Observable<Chantier> {
    return this.http.post<Chantier>(this.apiUrl, chantier);
  }

  updateChantier(id: number, chantier: Partial<Chantier>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, chantier);
  }
}
