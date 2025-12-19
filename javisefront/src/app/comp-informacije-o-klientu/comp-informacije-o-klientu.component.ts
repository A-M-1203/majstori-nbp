import { Component } from '@angular/core';
import { ServiceKlijent } from '../service/ser-klijent';
import { ActivatedRoute } from '@angular/router';
import { ServiceMajstor } from '../service/ser-majstor';
import { Majstor } from '../interface/majstor';
import { Klijent } from '../interface/klijent';

@Component({
  selector: 'app-comp-informacije-o-klientu',
  templateUrl: './comp-informacije-o-klientu.component.html',
  styleUrl: './comp-informacije-o-klientu.component.css'
})
export class CompInformacijeOKlientuComponent {
  private id:string | null='';
  klijent:Klijent | null=null;
  constructor(private servis:ServiceKlijent,private route:ActivatedRoute){
    route.paramMap.subscribe(param=>{
      this.id=param.get("id");
      if(this.id){
        servis.getKlijentInfo(this.id).subscribe(x=>{
          this.klijent=x;
        })
      }
    })
}
}
