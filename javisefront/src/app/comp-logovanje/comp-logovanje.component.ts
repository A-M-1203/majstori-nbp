import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServiceLogovanje } from '../service/ser-logovanje';


@Component({
  selector: 'app-comp-logovanje',
  templateUrl: './comp-logovanje.component.html',
  styleUrl: './comp-logovanje.component.css'
})
export class CompLogovanjeComponent {
  errorMessage: string = '';

  constructor(private router: Router, private logservice: ServiceLogovanje) { }

  mail = '';
  pasvord = '';
  role='korisnik'

  login() {
    this.logservice.login({ email: this.mail, password: this.pasvord },this.role)
    .subscribe(x=>{
      if (x.type === "korisnik") {
        this.router.navigate(['/glavnaStranicaKlient']);
      } else {
        this.router.navigate(['/glavnaStranicaMajstor']);
      }
    },
    error => {
      alert("Pogrešan E-mail ili pasvord!")
    }
  );
  }    //TODO log out
}
