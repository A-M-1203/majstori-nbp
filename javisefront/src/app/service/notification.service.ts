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
    this.httpClient.get<NotificationItem[]>(url+"/nottification").subscribe(x=>{
      for(let i=0;i<x.length;i++){
        this._items$.next([x[i],...this._items$.value]);
      }
    })
  }

  get items(): NotificationItem[] {
    return this._items$.value;
  }



  push(text: string, opts?: Partial<NotificationItem>) {
    const item: NotificationItem = {
      id: uid(),
      text,
      time: Date.now(),
      avatarUrl: opts?.avatarUrl,
      link: opts?.link
    };
    this._items$.next([item, ...this._items$.value]);
    return item.id;
  }




  clear() {
    this._items$.next([]);
    this.httpClient.delete(url+"/nottification").subscribe(x=>{
      console.log(x);
    });
  }

  seedDemo() {
    if (this._items$.value.length) return;
    this._items$.next([
      { id: uid(), text: 'ana_likes liked your photo.', time: Date.now() - 2 * 60_000, avatarUrl: 'https://i.pravatar.cc/64?img=5' },
      { id: uid(), text: 'marko99 commented: “🔥🔥”', time: Date.now() - 45 * 60_000, avatarUrl: 'https://i.pravatar.cc/64?img=12' },
      { id: uid(), text: 'jelena started following you.', time: Date.now() - 3 * 60 * 60_000, avatarUrl: 'https://i.pravatar.cc/64?img=8' },
    ]);
  }
}
