import { Component } from '@angular/core';
import { Router } from 'express';
import { ServiceLogovanje } from '../service/ser-logovanje';
import {CompNotificationComponent} from '../comp-notification/comp-notification.component'

@Component({
  selector: 'app-comp-header-toolbar',
  templateUrl: './comp-header-toolbar.component.html',
  styleUrl: './comp-header-toolbar.component.css'
})
export class CompHeaderToolbarComponent {

  constructor (private servis: ServiceLogovanje) {}

  logOut() {
    this.servis.logout();
  }
}
