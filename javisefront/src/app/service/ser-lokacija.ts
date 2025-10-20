import {Injectable} from '@angular/core'
import {Observable, Subject} from 'rxjs'
import { Lokacija } from '../interface/lokacija'
import { HttpClient } from '@angular/common/http';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceLokacija {
    constructor(private http: HttpClient ){}
    private subject = new Subject<Lokacija[]>();

    vratiOb(): Observable<Lokacija[]> {
        return this.subject.asObservable();
    }

    getLokacije() {
        this.http.get<{lokacije: Lokacija[]}>(url+'/lokacija')
        .subscribe(x=>{
            this.subject.next(x.lokacije);
        });
    }
}