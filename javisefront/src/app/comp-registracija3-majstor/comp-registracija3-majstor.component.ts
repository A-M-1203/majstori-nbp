import { Component } from '@angular/core';
import { Lokacija } from '../interface/lokacija';
import { ServiceLokacija } from '../service/ser-lokacija';
import { ServiceKategorija } from '../service/ser-kategorija';
import { Observable } from 'rxjs';
import { serialize } from 'v8';
import { Kategorija } from '../interface/kategorija';
import { Majstor } from '../interface/majstor';
import { ServiceMajstor } from '../service/ser-majstor';
import { RegistrationDataService } from '../service/registration-data.services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-comp-registracija3-majstor',
  templateUrl: './comp-registracija3-majstor.component.html',
  styleUrl: './comp-registracija3-majstor.component.css'
})
export class CompRegistracija3MajstorComponent {
  selectedValue: string ='';
  nizLokacija: any[] = ["Beograd","Aleksinac","Novi sad","Niš","Kragujevac","Vranje", "Ivanjica"] ;
  nizKategorija: Kategorija[] = [];
  ime: any;
  prezime: any;
  adresa: any;
  mail='';
  pasvord='';
  brt: any;
  kategorije: any;

  constructor(service: ServiceLokacija, serviceKat: ServiceKategorija, private registrationDataService: RegistrationDataService,private servis: ServiceMajstor, private router: Router){

    service.getLokacije();
    this.mail=registrationDataService.getMail();
      this.pasvord=registrationDataService.getPas();
    serviceKat.vratiOb().subscribe(x=>{
      this.nizKategorija = x;
    })
    serviceKat.getKategorije();
  }

  isFormValid(): boolean {
    return this.ime && this.prezime && this.brt && this.adresa && this.selectedValue && this.kategorije;
  }

  kreirajNalog() {
    let majstor = {
      ime: this.ime,
      prezime: this.prezime,
      email: this.mail,
      broj: this.brt,
      lokacija: this.selectedValue,
      adresa: this.adresa,
      podkategorija: this.kategorije,
      password: this.pasvord
    }
    this.servis.postMajstor(majstor);
    this.router.navigate(['/']);
  }
}
