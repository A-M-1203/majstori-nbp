import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationDataService } from '../service/registration-data.services';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-comp-registracija2',
  templateUrl: './comp-registracija2.component.html',
  styleUrl: './comp-registracija2.component.css'
})
export class CompRegistracija2Component {
  mail: any;
  pas: any;
  passwordLengthValid: boolean = false;
  passwordHasNumber: boolean = false;
  passwordHasCapital: boolean = false;
  checkPassword() {
    const lengthRegex = /^.{8,20}$/;
    const numberRegex = /\d/;
    const capitalLetterRegex = /[A-Z]/;

    this.passwordLengthValid = lengthRegex.test(this.pas);
    this.passwordHasNumber = numberRegex.test(this.pas);
    this.passwordHasCapital = capitalLetterRegex.test(this.pas);
  }
  
  constructor(private router: Router, private registrationDataService: RegistrationDataService) {}

  emailFormControl = new FormControl('', [Validators.required, Validators.email]);

  isFormValid(): boolean {
    return this.emailFormControl.valid && this.passwordLengthValid && this.passwordHasNumber && this.passwordHasCapital;
  }

  navigateToNext() {
    const userType = this.registrationDataService.getUserType();
    this.registrationDataService.setUserEmPas(this.mail,this.pas)
    if (userType === 1) {
      this.router.navigate(['/registracija3']);
    } else if (userType === 2) {
      this.router.navigate(['/registracija3maj']);
    }
  }
}
