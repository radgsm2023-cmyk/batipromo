import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Caisse, CaisseMouvement } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class CaisseService {
  private apiUrl = `${environment.apiUrl}/caisses`;

  constructor(private http: HttpClient) {}

  getCaisses(): Observable<Caisse[]> {
    return this.http.get<Caisse[]>(this.apiUrl);
  }

  getMouvements(caisseId?: number, limit = 50): Observable<CaisseMouvement[]> {
    let params = new HttpParams().set('limit', limit.toString());
    if (caisseId) {
      params = params.set('caisseId', caisseId.toString());
    }
    return this.http.get<CaisseMouvement[]>(`${this.apiUrl}/mouvements`, { params });
  }

  createMouvement(mouvementData: {
    caisseId: number;
    type: 'ALIMENTATION' | 'DECAISSEMENT';
    montant: number;
    motif: string;
    rubrique?: string;
    beneficiaireName?: string;
    pieceJustificativeRef?: string;
    modePaiement: string;
  }): Observable<{ success: boolean; numeroBon: string; nouveauSolde: number }> {
    return this.http.post<{ success: boolean; numeroBon: string; nouveauSolde: number }>(
      `${this.apiUrl}/mouvements`,
      mouvementData
    );
  }
}
