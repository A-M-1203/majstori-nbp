import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { ServiceLogovanje } from '../service/ser-logovanje';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class KorisnikGuard implements CanActivate {

  constructor(private authService: ServiceLogovanje, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const expectedRoles = next.data['expectedRoles'] as string[];
    const hasToken=this.authService.getToken();
    if (!hasToken) {
      this.router.navigate(['/']);
    }
    else{
        const type:any=jwtDecode(hasToken);
        console.log(type);
        if(type.type!=="korisnik"){
          alert("Niste ulogovani kao korisnik");
          this.authService.logout();
          this.router.navigate(['/']);
        }
    }
    return hasToken!=null;
  }
}