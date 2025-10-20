import {Injectable} from '@angular/core'
import {Observable, Subject} from 'rxjs'
import { HttpClient } from '@angular/common/http';
import { Majstor } from '../interface/majstor';
import { map } from 'rxjs/operators';
import { Lokacija } from '../interface/lokacija';
import { Klijent } from '../interface/klijent';
import { Oglas } from '../interface/oglas';
import { Ocena } from '../interface/ocena';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceMajstor {
    private subject = new Subject<string>();
    constructor(private http: HttpClient ){}

    vratiOb(): Observable<string> {
        return this.subject.asObservable();
    }

    postMajstor(maj: Majstor){
        this.http.post<{message: string}>(url+'/majstor/signup', maj)
        .subscribe(x=>{
            this.subject.next(x.message);
            console.log(x.message);
        });
    }

    getProfile(): Observable<{ime:string, prezime:string, email:string, broj:string, lokacija:Lokacija, adresa:string, podkategorija:{naziv:String,_id:String,kategorija:String}[], profilePicture:string}> {
        return this.http.get<{ ime:string, prezime:string, email:string, broj:string, lokacija:Lokacija, adresa:string, podkategorija:{naziv:String,_id:String,kategorija:String}[], profilePicture:string}>("http://localhost:3000/korisnik/profile")
            .pipe(
                map(response => {
                    return { ime: response.ime, prezime: response.prezime, email: response.email, broj:response.broj, lokacija:response.lokacija, adresa:response.adresa, podkategorija:response.podkategorija, profilePicture:response.profilePicture};
                })
            );
    }
    getMajstori(url:string): Observable<{message: string, majstors: Majstor[]}>{
        return this.http.get<{message:string, majstors: Majstor[]}>(url);
    }
    getMajstor(id:string):Observable<{majstor:Majstor,ocene:Ocena[],prosek:Number,post:Oglas[]}>{
        console.log(url+"/majstor/"+id);
        return this.http.get<{majstor:Majstor,ocene:Ocena[],prosek:Number,post:Oglas[]}>(url+"/majstor/"+id);
    }
    editMajstor(form: FormData): Observable<any> {
        return this.http.put(url+"/korisnik", form);
    }
}