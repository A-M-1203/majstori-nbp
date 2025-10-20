import {Injectable} from '@angular/core'
import {Observable, Subject} from 'rxjs'
import { HttpClient } from '@angular/common/http';
import { Klijent } from '../interface/klijent';
import { map } from 'rxjs/operators';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceKlijent {
    private subject = new Subject<string>();
    constructor(private http: HttpClient ){}

    vratiOb(): Observable<string> {
        return this.subject.asObservable();
    }

    postKlijent(kl: Klijent){
        this.http.post<{message: string}>(url+'/korisnik/signup', kl)
        .subscribe(x=>{
            this.subject.next(x.message);
            console.log(x.message);
        });
    }

    getProfile(): Observable<{ime: string, prezime: string, profilePicture: string,adresa:string, lokacija:string,broj:string}> {
        return this.http.get<{ ime: string, prezime: string, profilePicture: string,adresa:string, lokacija:string,broj:string}>("http://localhost:3000/korisnik/profile")
            .pipe(
                map(response => {
                    return { ime: response.ime, prezime: response.prezime, profilePicture: response.profilePicture,adresa:response.adresa,lokacija:response.lokacija,broj:response.broj };
                })
            );
    }
    editKorisnik(form: FormData): Observable<any> {
        return this.http.put("http://localhost:3000/korisnik", form);
    }
}