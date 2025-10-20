import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CompGlavnaStranicaMajstorComponent } from '../comp-glavna-stranica-majstor/comp-glavna-stranica-majstor.component';
import { Kategorija } from '../interface/kategorija';
import { ServiceKategorija } from '../service/ser-kategorija';
import { Lokacija } from '../interface/lokacija';
import { ServiceLokacija } from '../service/ser-lokacija';
import { ServiceMajstor } from '../service/ser-majstor';

@Component({
  selector: 'app-comp-editovanje-profila-majstor',
  templateUrl: './comp-editovanje-profila-majstor.component.html',
  styleUrl: './comp-editovanje-profila-majstor.component.css'
})
export class CompEditovanjeProfilaMajstorComponent {
  kategorije: any=[];
  selectedFile: any;
  constructor(
    public dialogRef: MatDialogRef<CompGlavnaStranicaMajstorComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    serviceKat: ServiceKategorija,
    servisLok: ServiceLokacija,
    private servisMaj: ServiceMajstor
  ) {
    this.kategorije=this.data.podkategorija.map((x: { _id: any; })=>x._id);
    serviceKat.vratiOb().subscribe(x=>{
      this.nizKategorija = x;
    })
    serviceKat.getKategorije()
    servisLok.vratiOb().subscribe(x=>{
      this.nizLokacija = x;
    });
    servisLok.getLokacije();
  }

  editujProfil() {
    const formData = new FormData();
    formData.append('ime', this.data.ime);
    formData.append('prezime', this.data.prezime);
    formData.append('broj', this.data.broj);
    //formData.append('podkategorija', this.kategorije);
    for (let i = 0; i < this.kategorije.length; i++) {
        formData.append("podkategorija[]",this.kategorije[i]);
    }
    formData.append('lokacija', this.data.lokacija._id);
    formData.append('adresa', this.data.adresa);
    formData.append('image', this.selectedFile);

    this.servisMaj.editMajstor(formData).subscribe((x)=>{
      location.reload();
    })
    
    this.servisMaj.editMajstor(formData).subscribe(x => {
      this.dialogRef.close();
    });
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

  nizKategorija: Kategorija[] = [];
  nizLokacija: Lokacija[] = [];

  closeDialog() {
    this.dialogRef.close();
}
}
