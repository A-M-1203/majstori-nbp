import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import {  NotificationItem } from '../interface/notification';

function uid(): string {
  return Math.random().toString(36).slice(2) + Date.now().toString(36);
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly _items$ = new BehaviorSubject<NotificationItem[]>([]);
  readonly items$ = this._items$.asObservable();

  get items(): NotificationItem[] {
    return this._items$.value;
  }

  get unreadCount(): number {
    return this._items$.value.reduce((acc, n) => acc + (n.isRead ? 0 : 1), 0);
  }

  push(text: string, opts?: Partial<NotificationItem>) {
    const item: NotificationItem = {
      id: uid(),
      text,
      time: Date.now(),
      isRead: false,
      avatarUrl: opts?.avatarUrl,
      link: opts?.link
    };
    this._items$.next([item, ...this._items$.value]);
    return item.id;
  }

  markRead(id: string) {
    this._items$.next(this._items$.value.map(n => n.id === id ? { ...n, isRead: true } : n));
  }

  markAllRead() {
    this._items$.next(this._items$.value.map(n => ({ ...n, isRead: true })));
  }

  clear() {
    this._items$.next([]);
  }

  seedDemo() {
    if (this._items$.value.length) return;
    this._items$.next([
      { id: uid(), text: 'ana_likes liked your photo.', time: Date.now() - 2 * 60_000, isRead: false, avatarUrl: 'https://i.pravatar.cc/64?img=5' },
      { id: uid(), text: 'marko99 commented: “🔥🔥”', time: Date.now() - 45 * 60_000, isRead: false, avatarUrl: 'https://i.pravatar.cc/64?img=12' },
      { id: uid(), text: 'jelena started following you.', time: Date.now() - 3 * 60 * 60_000, isRead: true, avatarUrl: 'https://i.pravatar.cc/64?img=8' },
    ]);
  }
}
