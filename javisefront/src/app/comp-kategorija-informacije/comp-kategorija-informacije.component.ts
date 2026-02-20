import { Component,OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServiceMajstor } from '../service/ser-majstor';
import { Majstor } from '../interface/majstor';

@Component({
  selector: 'app-comp-kategorija-informacije',
  templateUrl: './comp-kategorija-informacije.component.html',
  styleUrl: './comp-kategorija-informacije.component.css'
})
export class CompKategorijaInformacijeComponent implements OnInit{

  id: string | null = null;
  majstori: Majstor[] = [];
  sviMajstori: Majstor[] = [];
  kategorija:string | null=null;
  lokacije: string[] = ["Beograd","Aleksinac","Novi sad","Niš","Kragujevac","Vranje", "Ivanjica"] ;
  selectedValue: any;

  constructor(
    private route: ActivatedRoute,private servisMajstor:ServiceMajstor
  ) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(param=>{
      this.id=param.get('id');
      this.kategorija=param.get("kategorija");
    })
    if(this.id){
      if(this.kategorija=="kategorija"){

        this.servisMajstor.getMajstori("http://localhost:5104/majstor?kategorija="+this.id)
        .subscribe( x=> {
          console.log(x);
          this.sviMajstori = x.majstors;
          this.majstori = x.majstors;
        })
      }
      else{

        this.servisMajstor.getMajstori("http://localhost:5104/majstor?podKategorija="+this.id)
        .subscribe( x=> {
          this.sviMajstori = x.majstors;
          this.majstori = x.majstors;
        })

      }
    }
  }

  isFormValid(): boolean {
    return this.selectedValue;
  }

  filtriraj() {
    this.majstori = this.sviMajstori.filter( x=> {
      return x.lokacija == this.selectedValue;
    })
  }

  ponistiFilter() {
    this.majstori = this.sviMajstori;
  }
}
