import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompRegistracijaComponent } from './comp-registracija/comp-registracija.component';
import { CompLogovanjeComponent } from './comp-logovanje/comp-logovanje.component';
import { CompRegistracija2Component } from './comp-registracija2/comp-registracija2.component';
import { CompRegistracija3Component } from './comp-registracija3/comp-registracija3.component';
import { CompRegistracija3MajstorComponent } from './comp-registracija3-majstor/comp-registracija3-majstor.component';
import { CompGlavnaStranicaKlijentComponent } from './comp-glavna-stranica-klijent/comp-glavna-stranica-klijent.component';
import { CompGlavnaStranicaMajstorComponent } from './comp-glavna-stranica-majstor/comp-glavna-stranica-majstor.component';
import { AuthGuard } from './guard/AuthGuard';
import { PassGuard } from './guard/PassGuard';
import { CompKategorijaInformacijeComponent } from './comp-kategorija-informacije/comp-kategorija-informacije.component';
import { CompInformacijeOMajstoruComponent } from './comp-informacije-o-majstoru/comp-informacije-o-majstoru.component';
import { CompIzabraniMajstoriComponent } from './comp-izabrani-majstori/comp-izabrani-majstori.component';
import { MajstorGuard } from './guard/MajstorGuard';
import { KorisnikGuard} from './guard/KorisnikGuard'
import { CompCharRoomClientComponent } from './comp-char-room-client/comp-char-room-client.component';
import { CompCharRoomMajstorComponent } from './comp-char-room-majstor/comp-char-room-majstor.component';
import { CompInformacijeOKlientuComponent } from './comp-informacije-o-klientu/comp-informacije-o-klientu.component';

const routes: Routes = [
  {path: '', component: CompLogovanjeComponent,canActivate:[PassGuard]},
  {path: 'registracija', component: CompRegistracijaComponent,canActivate:[PassGuard]},
  {path: 'registracija2', component: CompRegistracija2Component,canActivate:[PassGuard]},
  {path: 'registracija3', component: CompRegistracija3Component,canActivate:[PassGuard]},
  {path: 'registracija3maj', component: CompRegistracija3MajstorComponent,canActivate: [PassGuard]},
  {path: 'glavnaStranicaKlient', component: CompGlavnaStranicaKlijentComponent,canActivate: [KorisnikGuard]},
  {path: 'glavnaStranicaMajstor', component: CompGlavnaStranicaMajstorComponent,canActivate:[MajstorGuard]},
  {path:'glavnaStranicaMajstor/room',component:CompCharRoomMajstorComponent,canActivate:[MajstorGuard]},
  {path:'glavnaStranicaMajstor/room/:id',component:CompInformacijeOKlientuComponent,canActivate:[MajstorGuard]},
  {path:'glavnaStranicaKlient/izabrani',component:CompIzabraniMajstoriComponent,canActivate:[KorisnikGuard]},
  {path:'glavnaStranicaKlient/izabrani/:id',component:CompCharRoomClientComponent,canActivate:[KorisnikGuard]},
  {path:'glavnaStranicaKlient/majstor/:id',component:CompInformacijeOMajstoruComponent,canActivate:[KorisnikGuard]},
  {path: 'glavnaStranicaKlient/:kategorija/:id',component:CompKategorijaInformacijeComponent,canActivate:[KorisnikGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
