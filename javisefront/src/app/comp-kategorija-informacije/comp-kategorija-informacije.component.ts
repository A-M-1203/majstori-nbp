import { Component,OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServiceOglas } from '../service/ser-oglas';
import { Oglas } from '../interface/oglas';
import { ServiceMajstor } from '../service/ser-majstor';
import { Majstor } from '../interface/majstor';
import { Lokacija } from '../interface/lokacija';
import { ServiceLokacija } from '../service/ser-lokacija';

@Component({
  selector: 'app-comp-kategorija-informacije',
  templateUrl: './comp-kategorija-informacije.component.html',
  styleUrl: './comp-kategorija-informacije.component.css'
})
export class CompKategorijaInformacijeComponent implements OnInit{
  
  id: string | null = null;
  oglasi: Oglas[]=[];
  majstori: Majstor[] = [];
  sviMajstori: Majstor[] = [];
  kategorija:string | null=null;
  lokacije: Lokacija[] = [];
  selectedValue: any;
  
  constructor(
    private route: ActivatedRoute,
    private servisOglasi: ServiceOglas,private servisMajstor:ServiceMajstor,
    private servisLokacija: ServiceLokacija
  ) { 
    servisLokacija.vratiOb().subscribe(x=>{
      this.lokacije = x;
    });
    servisLokacija.getLokacije();
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(param=>{
      this.id=param.get('id');
      this.kategorija=param.get("kategorija");
    })
    if(this.id){
      if(this.kategorija=="kategorija"){
        this.servisOglasi.getOglasi("http://localhost:3000/post/filter?kategorija="+this.id).subscribe(x=>{
          this.oglasi=x.post;
        });
        this.servisMajstor.getMajstori("http://localhost:3000/korisnik?kategorija="+this.id)
        .subscribe( x=> {
          console.log(x);
          this.sviMajstori = x.majstors;
          this.majstori = x.majstors;
        })
      }
      else{
        this.servisOglasi.getOglasi("http://localhost:3000/post/filter?podKategorija="+this.id).subscribe(x=>{
          this.oglasi=x.post;
        });
        this.servisMajstor.getMajstori("http://localhost:3000/korisnik?podKategorija="+this.id)
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
