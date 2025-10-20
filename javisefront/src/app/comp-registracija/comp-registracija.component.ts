import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationDataService } from '../service/registration-data.services';

@Component({
  selector: 'app-comp-registracija',
  templateUrl: './comp-registracija.component.html',
  styleUrl: './comp-registracija.component.css'
})
export class CompRegistracijaComponent {
  constructor(private router: Router, private registrationDataService: RegistrationDataService) {}

  selectType(type: number) {
    console.log(type)
    this.registrationDataService.setUserType(type);
    this.router.navigate(['/registracija2']);
  }
}
