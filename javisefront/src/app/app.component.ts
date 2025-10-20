import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'javisefront';
  showToolbar: boolean = false;
  private noToolbarRoutes: string[] = ['/', '/registracija', '/registracija2', '/registracija3', '/registracija3maj'];

  constructor(private router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.showToolbar = !this.noToolbarRoutes.includes(event.urlAfterRedirects);
      }
    });
  }
}
