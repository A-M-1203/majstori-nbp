import { Component, ElementRef, HostListener } from '@angular/core';
import { NotificationItem } from '../interface/notification';
import { NotificationService } from '../service/notification.service';
import { Observable } from 'rxjs';
import { SocketService } from '../service/ser-socket';

@Component({
  selector: 'app-comp-notification',
  templateUrl: './comp-notification.component.html',
  styleUrl: './comp-notification.component.css'
})
export class CompNotificationComponent {
  open = false;
  items$: Observable<NotificationItem[]> = this.ns.items$;
  private notification:any[]=[];

  constructor(public ns: NotificationService, private el: ElementRef<HTMLElement>,private socket:SocketService) {
    // optional demo seed
    //this.ns.seedDemo();
    const token:string | null=localStorage.getItem("jwtToken");
    if(token){
      socket.joinNotificationRoom(token);
      socket.onNotification((n)=>{
        this.notification.push(n);
      })
    }
    ns.get();
  }

  toggle() {
    this.open = !this.open;
  }

  close() {
    this.open = false;
  }

  onItemClick(n: NotificationItem) {
    this.close();
  }

  timeAgo(ms: number): string {
    const diff = Date.now() - ms;
    const m = Math.floor(diff / 60_000);
    if (m < 1) return 'now';
    if (m < 60) return `${m}m`;
    const h = Math.floor(m / 60);
    if (h < 24) return `${h}h`;
    const d = Math.floor(h / 24);
    return `${d}d`;
  }

  // Close on outside click
  @HostListener('document:mousedown', ['$event'])
  onDocMouseDown(ev: MouseEvent) {
    if (!this.open) return;
    const target = ev.target as Node | null;
    if (target && !this.el.nativeElement.contains(target)) this.close();
  }

  // Close on ESC
  @HostListener('document:keydown', ['$event'])
  onKeyDown(ev: KeyboardEvent) {
    if (this.open && ev.key === 'Escape') this.close();
  }
}
