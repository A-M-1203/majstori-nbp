import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { ServiceLogovanje } from '../service/ser-logovanje';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: ServiceLogovanje, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    const expectedRoles = next.data['expectedRoles'] as string[];
    const hasToken=this.authService.getToken();
    if (!hasToken) {
      this.router.navigate(['/']);
    }
    return hasToken!=null;
  }
}
