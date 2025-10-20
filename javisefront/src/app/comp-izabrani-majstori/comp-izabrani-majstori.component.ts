import { Component } from '@angular/core';
import { ServiceOcenaKontakt } from '../service/ser-ocene';
import { Ocena } from '../interface/ocena';
import { Router } from '@angular/router';
import { Majstor } from '../interface/majstor';

@Component({
  selector: 'app-comp-izabrani-majstori',
  templateUrl: './comp-izabrani-majstori.component.html',
  styleUrl: './comp-izabrani-majstori.component.css'
})
export class CompIzabraniMajstoriComponent {
goToChat(id: string) {
  this.router.navigate(['glavnaStranicaKlient/izabrani/'+id]);
}
  ocene:Ocena[]=[];
  inputs:number[]=[];
  constructor(private servis:ServiceOcenaKontakt,private router:Router){
    servis.getOcena().subscribe(x=>{
      this.ocene=x.ocene;
      this.initializeItems(x.ocene.length);
    })
  }
  initializeItems(length: number) {
     for (let i = 0; i < length; i++) {
        this.inputs.push(1);
     }
  }
  oceni(i:number,majstorId:string){
    this.ocene[i].ocena=this.inputs[i];
    this.servis.putOcena(majstorId,this.inputs[i]);
  }
}
