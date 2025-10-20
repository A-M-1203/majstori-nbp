import { Component } from '@angular/core';
import { ServiceOglas } from '../service/ser-oglas';
import { Oglas } from '../interface/oglas';
import { ActivatedRoute, Route } from '@angular/router';
import { Majstor } from '../interface/majstor';
@Component({
  selector: 'app-comp-informacije-o-oglasu',
  templateUrl: './comp-informacije-o-oglasu.component.html',
  styleUrl: './comp-informacije-o-oglasu.component.css'
})
export class CompInformacijeOOglasuComponent {

  oglas:Oglas | null=null;
  majstor:Majstor | null=null;
  private id:string| null=null;
  currentSlideIndex: number = 0;
  prosek: number = 0;

  constructor(private servis:ServiceOglas,private route:ActivatedRoute){
      route.paramMap.subscribe(params=>{
        this.id=params.get('id');
        if(this.id){
          servis.getOglas(this.id).subscribe(x=>{
            this.oglas=x.post;
            this.majstor=x.majstor;
            this.prosek = x.prosek;
          });
        }
      });
  }

  prevImage() {
    if (this.oglas?.slike?.length) {
      this.currentSlideIndex = (this.currentSlideIndex - 1 + this.oglas.slike.length) % this.oglas.slike.length;
    }
  }

  nextImage() {
    if (this.oglas?.slike?.length) {
      this.currentSlideIndex = (this.currentSlideIndex + 1) % this.oglas.slike.length;
    }
  }
}
