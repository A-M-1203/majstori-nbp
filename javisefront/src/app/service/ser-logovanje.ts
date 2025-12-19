import { Injectable,Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router'
import { url } from '../constant'

@Injectable({providedIn: 'root'})
export class ServiceLogovanje {
  [x: string]: any;

  constructor(private http: HttpClient,@Inject(PLATFORM_ID) private platformId: Object, private router: Router) { }

  login(credentials: { email: string, password: string},type:string): Observable<{token:string, type:string}> {
    return this.http.post<{ token: string, message: string, type: string}>(url+"/"+type+"/signin", credentials)
      .pipe(
        map(response => {
          sessionStorage.setItem('jwtToken',response.token);
          localStorage.setItem('jwtToken', response.token);
          return {token:response.token,type:response.type}
        })
      );
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/']);
  }

  getToken(): string | null {
      return localStorage.getItem('jwtToken');
  }
}
