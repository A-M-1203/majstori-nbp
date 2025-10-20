import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CompPostavljanjeOglasaComponent } from '../comp-postavljanje-oglasa/comp-postavljanje-oglasa.component';
import { ServiceMajstor } from '../service/ser-majstor';
import { Lokacija } from '../interface/lokacija';
import { ServiceOglas } from '../service/ser-oglas';
import { Oglas } from '../interface/oglas';
import { CompEditovanjeOglasaComponent } from '../comp-editovanje-oglasa/comp-editovanje-oglasa.component';
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
  lokacija: Lokacija | undefined;
  adresa: string = '';
  podkategorija: {naziv:String,_id:String,kategorija:String}[] = [];
  profilePicture: string = '';

  oglasi: Oglas[] = [];

  constructor(public dialog: MatDialog, private servisMajstor: ServiceMajstor, private servisOgl:ServiceOglas,private router:Router) {
    servisOgl.getOglasi("http://localhost:3000/post")
    .subscribe(x => {
      this.oglasi = x.post
    })
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

  openDialogPostavi(): void {
    this.dialog.open(CompPostavljanjeOglasaComponent, {
      data:[{naziv:'',podkategorije:this.podkategorija}],
      enterAnimationDuration: '400ms',
      exitAnimationDuration: '400ms'
    });
  }

  openDialogEdituj(oglas: Oglas): void {
    this.dialog.open(CompEditovanjeOglasaComponent, {
      data: {oglas:oglas,podkategorije:[{naziv:'',podkategorije:this.podkategorija.map(x=>{
        return {naziv:x.naziv,_id:x._id}
      })}]},
      enterAnimationDuration: '400ms',
      exitAnimationDuration: '400ms'
    });
  }

  obrisiOglas(id:string) {
    this.oglasi=this.oglasi.filter(b=>b._id!=id);
    this.servisOgl.deleteOglas(id);
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
