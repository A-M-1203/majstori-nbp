import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { CompLogovanjeComponent } from './comp-logovanje/comp-logovanje.component';
import {MatRadioModule} from '@angular/material/radio';

import {MatCardModule} from '@angular/material/card';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatDividerModule} from '@angular/material/divider';
import {MatInputModule} from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { CompRegistracijaComponent } from './comp-registracija/comp-registracija.component';
import { RouterModule } from '@angular/router';
import { CompRegistracija2Component } from './comp-registracija2/comp-registracija2.component';
import { CompRegistracija3Component } from './comp-registracija3/comp-registracija3.component';
import { CompRegistracija3MajstorComponent } from './comp-registracija3-majstor/comp-registracija3-majstor.component';
import{HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {MatSelect, MatSelectModule, matSelectAnimations} from '@angular/material/select';
import { MatOption } from '@angular/material/select';
import {FormControl, Validators, ReactiveFormsModule} from '@angular/forms';
import { CompGlavnaStranicaKlijentComponent } from './comp-glavna-stranica-klijent/comp-glavna-stranica-klijent.component';
import { CompGlavnaStranicaMajstorComponent } from './comp-glavna-stranica-majstor/comp-glavna-stranica-majstor.component';
import {MatToolbarModule} from '@angular/material/toolbar';
import { CompHeaderToolbarComponent } from './comp-header-toolbar/comp-header-toolbar.component';
import {MatGridListModule} from '@angular/material/grid-list';
import { FlexLayoutModule } from '@angular/flex-layout';
import {MatDialogModule, matDialogAnimations} from '@angular/material/dialog';
import { CompPostavljanjeOglasaComponent } from './comp-postavljanje-oglasa/comp-postavljanje-oglasa.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatIconModule} from '@angular/material/icon';
import { ServiceLogovanje } from './service/ser-logovanje';
import { AuthInterceptor } from './interceptor/interceptor';
import { AuthGuard } from './guard/AuthGuard';
import { CompEditovanjeOglasaComponent } from './comp-editovanje-oglasa/comp-editovanje-oglasa.component';
import { CompEditovanjeProfilaMajstorComponent } from './comp-editovanje-profila-majstor/comp-editovanje-profila-majstor.component';
import { PassGuard } from './guard/PassGuard';
import { CompKategorijaInformacijeComponent } from './comp-kategorija-informacije/comp-kategorija-informacije.component';
import { CompInformacijeOMajstoruComponent } from './comp-informacije-o-majstoru/comp-informacije-o-majstoru.component';
import { CompInformacijeOOglasuComponent } from './comp-informacije-o-oglasu/comp-informacije-o-oglasu.component';
import { CompIzabraniMajstoriComponent } from './comp-izabrani-majstori/comp-izabrani-majstori.component';
import { CompEditovanjeKlientaComponent } from './comp-editovanje-klienta/comp-editovanje-klienta.component';
import { CompCharRoomClientComponent } from './comp-char-room-client/comp-char-room-client.component';
import { CompCharRoomMajstorComponent } from './comp-char-room-majstor/comp-char-room-majstor.component';
import { CompInformacijeOKlientuComponent } from './comp-informacije-o-klientu/comp-informacije-o-klientu.component';
import { CompNotificationComponent } from './comp-notification/comp-notification.component';

@NgModule({
  declarations: [
    AppComponent,
    CompLogovanjeComponent,
    CompRegistracijaComponent,
    CompRegistracija2Component,
    CompRegistracija3Component,
    CompRegistracija3MajstorComponent,
    CompGlavnaStranicaKlijentComponent,
    CompGlavnaStranicaMajstorComponent,
    CompHeaderToolbarComponent,
    CompPostavljanjeOglasaComponent,
    CompEditovanjeOglasaComponent,
    CompEditovanjeProfilaMajstorComponent,
    CompKategorijaInformacijeComponent,
    CompInformacijeOMajstoruComponent,
    CompInformacijeOOglasuComponent,
    CompIzabraniMajstoriComponent,
    CompEditovanjeKlientaComponent,
    CompCharRoomClientComponent,
    CompCharRoomMajstorComponent,
    CompInformacijeOKlientuComponent,
    CompNotificationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatCardModule,
    MatFormFieldModule,
    MatButtonModule,
    MatDividerModule,
    MatInputModule,
    FormsModule,
    HttpClientModule,
    MatSelect,
    MatOption,
    ReactiveFormsModule,
    MatToolbarModule,
    MatGridListModule,
    FlexLayoutModule,
    MatDialogModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatRadioModule
  ],
  providers: [
    ServiceLogovanje,
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    provideClientHydration(),
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

