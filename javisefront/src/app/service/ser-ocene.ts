import {Injectable} from '@angular/core'
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ocena } from '../interface/ocena';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceOcenaKontakt {
    constructor(private http: HttpClient ){}

    addOcenaKontakt(id:String){
        this.http.post(url+"/ocena/",{majstorId:id}).subscribe(x=>{
            console.log(x);
            alert("Ocena dodata");
        },error=>{
            alert("Kontakt je vec dodat");
        });
    }
    getOcena():Observable<{ocene:Ocena[]}>{
        return this.http.get<{ocene:Ocena[]}>(url+"/ocena/");
    }
    putOcena(id:string,ocena:number){
        return this.http.put(url+"/ocena",{majstorId:id,ocena:ocena}).subscribe(x=>{
            console.log(x);
        })
    }
    getChats():Observable<{chats:Ocena[]}>{
        return this.http.get<{chats:Ocena[]}>(url+"/ocena/chats");
    }
}