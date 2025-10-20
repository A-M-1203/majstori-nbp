import { Component, Inject } from '@angular/core';
import { Kategorija } from '../interface/kategorija';
import { ServiceKategorija } from '../service/ser-kategorija';
import { ServiceOglas } from '../service/ser-oglas';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-comp-postavljanje-oglasa',
  templateUrl: './comp-postavljanje-oglasa.component.html',
  styleUrl: './comp-postavljanje-oglasa.component.css'
})
export class CompPostavljanjeOglasaComponent {
  nizKategorija: Kategorija[] = [];
  kategorija: any;
  selectedFiles: any[] = [];
  naziv: any;
  opis: any;
  maxbrojslika: number = 6;
  
  constructor(private serviceKat: ServiceKategorija, @Inject(MAT_DIALOG_DATA) public data:Kategorija[],private serviceOglas: ServiceOglas, private dialogRef: MatDialogRef<CompPostavljanjeOglasaComponent>){
    this.nizKategorija=data;
    serviceKat.getKategorije();
  }
  autoGrow(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    textarea.style.height = 'auto';
    textarea.style.height = `${textarea.scrollHeight}px`;
  }

  onSelectFiles(): void {
    const fileInput = document.querySelector<HTMLInputElement>('#fileInput');
    fileInput?.click();
  }

  onFilesSelected(event: any): void {
    const files = event.target.files;
    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      const reader = new FileReader();
      reader.onload = (e: any) => {
        if(this.selectedFiles.length < this.maxbrojslika){
          this.selectedFiles.push({ file, url: e.target.result });
        }
        else{
          alert("Možete izabrati maksimum 6 slika")
        }
      };
      reader.readAsDataURL(file);
    }
  }

  postaviOglas() {
    const formData = new FormData();
    formData.append('title', this.naziv);
    formData.append('content', this.opis);
    formData.append('podkategorija', this.kategorija);
    formData.append('datumModifikacije', new Date().toISOString());

    this.selectedFiles.forEach((fileObj, index) => {
      formData.append('files', fileObj.file);
    });

    this.serviceOglas.postOglas(formData).subscribe(x => {
      this.dialogRef.close();
      location.reload();
    });
  }
}
