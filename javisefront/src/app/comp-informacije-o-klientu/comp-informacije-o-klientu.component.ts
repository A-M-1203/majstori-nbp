import { Component } from '@angular/core';
import { ServiceKlijent } from '../service/ser-klijent';
import { ActivatedRoute } from '@angular/router';
import { ServiceMajstor } from '../service/ser-majstor';
import { Majstor } from '../interface/majstor';

@Component({
  selector: 'app-comp-informacije-o-klientu',
  templateUrl: './comp-informacije-o-klientu.component.html',
  styleUrl: './comp-informacije-o-klientu.component.css'
})
export class CompInformacijeOKlientuComponent {
  private id:string | null='';
  majstor:Majstor | null=null;
  constructor(private servis:ServiceMajstor,private route:ActivatedRoute){
    route.paramMap.subscribe(param=>{
      this.id=param.get("id");
      if(this.id){
        servis.getMajstor(this.id).subscribe(x=>{
            this.majstor=x.majstor;          
        });
      }
    })
}
}
