import {Injectable} from '@angular/core'
import {Observable, Subject} from 'rxjs'
import { HttpClient } from '@angular/common/http';
import { Kategorija } from '../interface/kategorija';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceKategorija {
    constructor(private http: HttpClient ){}
    private subject = new Subject<Kategorija[]>();

    vratiOb(): Observable<Kategorija[]> {
        return this.subject.asObservable();
    }

    getKategorije() {
        this.http.get<{kategorije: Kategorija[]}>(url+'/kategorije')
        .subscribe(x=>{
            console.log(x);
            this.subject.next(x.kategorije);
        });
    }
}