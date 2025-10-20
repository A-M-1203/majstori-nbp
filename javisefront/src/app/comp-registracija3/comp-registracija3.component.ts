import { Component } from '@angular/core';
import { Lokacija } from '../interface/lokacija';
import { ServiceLokacija } from '../service/ser-lokacija';
import { Observable } from 'rxjs';
import { serialize } from 'v8';
import { RegistrationDataService } from '../service/registration-data.services';
import { Klijent } from '../interface/klijent';
import { ServiceKlijent } from '../service/ser-klijent';
import { Router } from '@angular/router';

@Component({
  selector: 'app-comp-registracija3',
  templateUrl: './comp-registracija3.component.html',
  styleUrl: './comp-registracija3.component.css'
})
export class CompRegistracija3Component {
  selectedValue: string ='';
  nizLokacija: String[] =  ["Beograd","Aleksinac","Novi sad","Niš","Kragujevac","Vranje", "Ivanjica"];
  mail='';
  pasvord='';
  ime: any;
  prezime: any;
  brt: any;
  adresa: any;

  constructor(service: ServiceLokacija, private registrationDataService: RegistrationDataService,private servis: ServiceKlijent,private router: Router){

      this.mail=registrationDataService.getMail();
      this.pasvord=registrationDataService.getPas();
  }

  isFormValid(): boolean {
    return this.ime && this.prezime && this.brt && this.adresa && this.selectedValue;
  }

  kreirajNalog() {
    let klijent = {
      ime: this.ime,
      prezime: this.prezime,
      email: this.mail,
      broj: this.brt,
      lokacija: this.selectedValue,
      adresa: this.adresa,
      password: this.pasvord
    }
    this.servis.postKlijent(klijent);
    this.router.navigate(['/']);
  }
}
