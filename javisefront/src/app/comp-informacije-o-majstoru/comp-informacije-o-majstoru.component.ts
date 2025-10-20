import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServiceMajstor } from '../service/ser-majstor';
import { Majstor } from '../interface/majstor';
import { Ocena } from '../interface/ocena';
import { Oglas } from '../interface/oglas';
import { ServiceOcenaKontakt } from '../service/ser-ocene';
@Component({
  selector: 'app-comp-informacije-o-majstoru',
  templateUrl: './comp-informacije-o-majstoru.component.html',
  styleUrl: './comp-informacije-o-majstoru.component.css'
})
export class CompInformacijeOMajstoruComponent {
dodajUKontakte() {
    this.servisOcenaKontakt.addOcenaKontakt(this.majstor?._id!!);
}
  private id:string | null='';
  majstor:Majstor | null=null;
  ocene:Ocena[]=[];
  post:Oglas[]=[];
  prosek:Number=0;
    constructor(private servis:ServiceMajstor,private route:ActivatedRoute,private servisOcenaKontakt:ServiceOcenaKontakt){
        route.paramMap.subscribe(param=>{
          this.id=param.get("id");
          if(this.id){
            servis.getMajstor(this.id).subscribe(x=>{
              this.majstor=x.majstor;
              this.ocene=x.ocene;
              this.post=x.post;
              this.prosek=x.prosek;
            });
          }
        })
    }
}
