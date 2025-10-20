import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CompGlavnaStranicaKlijentComponent } from '../comp-glavna-stranica-klijent/comp-glavna-stranica-klijent.component';
import { ServiceLokacija } from '../service/ser-lokacija';
import { ServiceKlijent } from '../service/ser-klijent';
import { Lokacija } from '../interface/lokacija';

@Component({
  selector: 'app-comp-editovanje-klienta',
  templateUrl: './comp-editovanje-klienta.component.html',
  styleUrl: './comp-editovanje-klienta.component.css'
})
export class CompEditovanjeKlientaComponent {
closeDialog() {
      this.dialogRef.close();
}
editujProfil() {
  const formData = new FormData();
  formData.append('ime', this.data.ime);
  formData.append('prezime', this.data.prezime);
  formData.append('broj', this.data.broj);
  //formData.append('podkategorija', this.kategorije);
  formData.append('lokacija', this.data.lokacija._id);
  formData.append('adresa', this.data.adresa);
  formData.append('image', this.selectedFile);
  this.servisKli.editKorisnik(formData).subscribe((x)=>{
    location.reload();
  })
}
  kategorije: any;
  selectedFile: any;
  nizLokacija: Lokacija[] = [];
  constructor(
    public dialogRef: MatDialogRef<CompGlavnaStranicaKlijentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    servisLok: ServiceLokacija,
    private servisKli: ServiceKlijent
  ) {
      console.log(data);
      servisLok.vratiOb().subscribe(x=>{
        this.nizLokacija = x;
      });
      servisLok.getLokacije();
  }
  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] as File;
    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e) => {
        const img = new Image();
        img.onload = () => {
          const canvas = document.createElement('canvas');
          const ctx = canvas.getContext('2d')!;
          canvas.width = 150;
          canvas.height = 150;
          ctx.drawImage(img, 0, 0, 150, 150);
          const dataURL = canvas.toDataURL('image/jpeg');
          this.data.profilePicture = dataURL;
        };
        img.src = e.target?.result as string;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }
}
