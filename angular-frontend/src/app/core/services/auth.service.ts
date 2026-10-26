import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, User } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(this.getStoredUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  public get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  public getToken(): string | null {
    return localStorage.getItem('batipromo_angular_token');
  }

  public isAuthenticated(): boolean {
    return !!this.getToken();
  }

  public login(credentials: { email: string; password: string }): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, credentials).pipe(
      tap(res => {
        if (res && res.token) {
          localStorage.setItem('batipromo_angular_token', res.token);
          localStorage.setItem('batipromo_angular_user', JSON.stringify(res.user));
          this.currentUserSubject.next(res.user);
        }
      })
    );
  }

  public logout(): void {
    localStorage.removeItem('batipromo_angular_token');
    localStorage.removeItem('batipromo_angular_user');
    this.currentUserSubject.next(null);
  }

  private getStoredUser(): User | null {
    try {
      const stored = localStorage.getItem('batipromo_angular_user');
      return stored ? JSON.parse(stored) : null;
    } catch {
      return null;
    }
  }
}
