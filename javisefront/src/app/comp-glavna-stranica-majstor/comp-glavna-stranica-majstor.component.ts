import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ServiceMajstor } from '../service/ser-majstor';
import { Lokacija } from '../interface/lokacija';
import { Oglas } from '../interface/oglas';
import { CompEditovanjeProfilaMajstorComponent } from '../comp-editovanje-profila-majstor/comp-editovanje-profila-majstor.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-comp-glavna-stranica-majstor',
  templateUrl: './comp-glavna-stranica-majstor.component.html',
  styleUrl: './comp-glavna-stranica-majstor.component.css'
})
export class CompGlavnaStranicaMajstorComponent implements OnInit{
  openChatRoom() {
     this.router.navigate(['/glavnaStranicaMajstor/room']);
  }
  ime: any;
  prezime: any;
  email: string = '';
  broj: string = '';
  lokacija: string='';
  adresa: string = '';
  podkategorija: {naziv:String,_id:String,kategorija:String}[] = [];
  profilePicture: string = '';


  constructor(public dialog: MatDialog, private servisMajstor: ServiceMajstor,private router:Router) {
  }

  ngOnInit(): void {
      this.servisMajstor.getProfile().subscribe(x=>{
        this.ime = x.ime;
        this.prezime = x.prezime;
        this.email = x.email;
        this.broj = x.broj;
        this.lokacija = x.lokacija;
        this.adresa = x.adresa;
        this.podkategorija = x.podkategorija;
        this.profilePicture = x.profilePicture;
    });
  }






  openDialogEditujProfil() {
    this.dialog.open(CompEditovanjeProfilaMajstorComponent, {
      data: {
        ime: this.ime,
        prezime: this.prezime,
        email: this.email,
        broj: this.broj,
        lokacija: this.lokacija,
        adresa: this.adresa,
        podkategorija: this.podkategorija,
        profilePicture: this.profilePicture
      },
      enterAnimationDuration: '400ms',
      exitAnimationDuration: '400ms'
    })
  }
}
