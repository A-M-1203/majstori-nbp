import {Injectable} from '@angular/core'
import { HttpClient } from '@angular/common/http';
import { Observable,Subject } from 'rxjs';
import { Message } from '../interface/message';
import { url } from '../constant'

@Injectable({providedIn: "root"})
export class ServiceMessage{
    constructor(private http: HttpClient ){}
    private subject=new Subject<Message[]>();
    returnObs():Observable<Message[]>{
        return this.subject.asObservable();
    }
    getMessages(id:String){
        return this.http.get<Message[]>(url+"/message/"+id);
    }
    sendMessage(message:Message){
      this.http.post<boolean>(url+"/message",message).subscribe(x=>{
          console.log(x);
      });
    }
}