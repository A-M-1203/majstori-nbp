import { Component } from '@angular/core';
import { Kategorija } from '../interface/kategorija';
import { ServiceKategorija } from '../service/ser-kategorija';
import { ServiceKlijent } from '../service/ser-klijent';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CompEditovanjeKlientaComponent } from '../comp-editovanje-klienta/comp-editovanje-klienta.component';

@Component({
  selector: 'app-comp-glavna-stranica-klijent',
  templateUrl: './comp-glavna-stranica-klijent.component.html',
  styleUrl: './comp-glavna-stranica-klijent.component.css'
})
export class CompGlavnaStranicaKlijentComponent {
    openDialogEditujProfil() {
      this.dialog.open(CompEditovanjeKlientaComponent,{
        data:{
          ime:this.ime,
          prezime:this.prezime,
          profilePicture:this.profilePicture,
          broj:this.broj,
          lokacija:this.lokacija,
          adresa:this.adresa
        }
      }
    )
  }
  nizKategorija: Kategorija[] = [];
  ime: string = ''
  prezime: string = ''
  profilePicture: string = ''
  adresa:string=''
  broj:string=''
  lokacija:string=''
  constructor(public dialog: MatDialog,serviceKat: ServiceKategorija, servis: ServiceKlijent,private router:Router){
    serviceKat.vratiOb().subscribe(x=>{
      this.nizKategorija = x;
    })
    serviceKat.getKategorije();
    servis.getProfile().subscribe(x=>{
      this.ime = x.ime;
      this.prezime = x.prezime;
      this.profilePicture = x.profilePicture;
      this.adresa=x.adresa;
      this.broj=x.broj;
      this.lokacija=x.lokacija;
    });
  
  }
  odNaIzabrane() {
    this.router.navigate(["glavnaStranicaKlient/izabrani"]);
  }
}
