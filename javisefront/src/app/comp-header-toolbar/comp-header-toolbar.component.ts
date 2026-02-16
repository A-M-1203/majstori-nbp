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




  constructor (private servis: ServiceLogovanje,private socket:SocketService) {

  }

  logOut() {
    this.servis.logout();
  }
}
