import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { ServiceLogovanje } from '../service/ser-logovanje';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class PassGuard implements CanActivate {

  constructor(private authService: ServiceLogovanje, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
      const hasToken=this.authService.getToken();
      if(!hasToken){
        return true;
      }
      else{
      const type:any=localStorage.getItem('type');
      if(type=="majstor"){
        this.router.navigate(['/glavnaStranicaMajstor']);
      }
      else{
        this.router.navigate(['/glavnaStranicaKlient']);
      }
      return false;
      }
  }
}