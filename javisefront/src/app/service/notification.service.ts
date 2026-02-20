import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import {  NotificationItem } from '../interface/notification';
import { HttpClient } from '@angular/common/http';
import { url } from '../constant';

function uid(): string {
  return Math.random().toString(36).slice(2) + Date.now().toString(36);
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly _items$ = new BehaviorSubject<NotificationItem[]>([]);
  readonly items$ = this._items$.asObservable();


  /**
   *
   */
  constructor(private httpClient:HttpClient) {
  }

  get(){
    return this.httpClient.get<NotificationItem[]>(url+"/nottification");
  }

  get items(): NotificationItem[] {
    return this._items$.value;
  }








  clear() {
    this._items$.next([]);
    this.httpClient.delete(url+"/nottification").subscribe(x=>{
      console.log(x);
    });
  }

}
