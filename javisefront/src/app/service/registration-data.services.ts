// registration-data.service.ts
import { Injectable } from '@angular/core';
import { Lokacija } from '../interface/lokacija';
import { Klijent } from '../interface/klijent';
import { Majstor } from '../interface/majstor';

@Injectable({
  providedIn: 'root'
})
export class RegistrationDataService {
  private userType: number | null = null;
  private ime: string = '';
  private prezime: string= '';
  private email: string= '';
  private broj: string= '';
  private lokacija: string= '';
  private adresa: string= '';
  private podkategorija: string[]= [];
  private password: string= '';

  setUserType(type: number) {
    console.log(type);
    this.userType = type;
  }

  setUserEmPas(mail: string, pas: string) {
    this.email = mail;
    this.password = pas;
  }

  setOther(ime: string, prezime:string,brt:string,adr:string,grad:string) {
    this.ime = ime;
    this.prezime = prezime;
    this.broj = brt;
    this.adresa = adr;
    this.lokacija = grad;
  }

  getUserType(): number | null {
    return this.userType;
  }

  getMail(): string {
    return this.email;
  }

  getPas(): string {
    return this.password;
  }
}
