import {Injectable} from '@angular/core'
import {Observable, Subject} from 'rxjs'
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Oglas } from '../interface/oglas';
import { response } from 'express';
import { Majstor } from '../interface/majstor';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceOglas {
    constructor(private http: HttpClient ){}

    postOglas(oglas: FormData): Observable<any> {
        return this.http.post(url+'/post', oglas);
    }

    getOglasi(url:string): Observable<{message:string, post:Oglas[]}>  {
        return this.http.get<{message:string, post:Oglas[]}>(url)
        .pipe(
            map(response => {
                return {message:response.message, post:response.post};
            })
        )
    }

    deleteOglas(id: string) {
        this.http.delete(url+"/post/" + id)
        .subscribe(x => {
            console.log(x)
        })
    }
    getOglas(id:string):Observable<{post:Oglas,majstor:Majstor, prosek: number}>{
        return this.http.get<{post:Oglas,majstor:Majstor,prosek:number}>(url+"/post/"+id);
    }
    editOglas(form: FormData): Observable<any> {
        return this.http.put(url+"/post", form);
    }
}