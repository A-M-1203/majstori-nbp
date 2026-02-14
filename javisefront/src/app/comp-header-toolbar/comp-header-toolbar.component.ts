import { Component } from '@angular/core';
import { Router } from 'express';
import { ServiceLogovanje } from '../service/ser-logovanje';
import {CompNotificationComponent} from '../comp-notification/comp-notification.component'
import { SocketService } from '../service/ser-socket';

@Component({
  selector: 'app-comp-header-toolbar',
  templateUrl: './comp-header-toolbar.component.html',
  styleUrl: './comp-header-toolbar.component.css'
})
export class CompHeaderToolbarComponent {


  private notification:any[]=[];

  constructor (private servis: ServiceLogovanje,private socket:SocketService) {
    const token:string | null=localStorage.getItem("jwtToken");
    if(token){
      socket.joinRoom(token);
      socket.onNotification((n)=>{
        this.notification.push(n);
      })
    }
  }

  logOut() {
    const token:string | null=localStorage.getItem("jwtToken");
    if(token){
      this.socket.leaveNotificationRoom(token);
    }
    this.servis.logout();
  }
}
