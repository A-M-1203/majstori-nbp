import { Component, Inject, OnInit } from '@angular/core';
import { Oglas } from '../interface/oglas';
import { ServiceOglas } from '../service/ser-oglas';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ServiceKategorija } from '../service/ser-kategorija';
import { Kategorija } from '../interface/kategorija';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-comp-editovanje-oglasa',
  templateUrl: './comp-editovanje-oglasa.component.html',
  styleUrl: './comp-editovanje-oglasa.component.css'
})
export class CompEditovanjeOglasaComponent {
  
  naziv: string = '';
  kategorija: string = '';
  opis: string = '';
  selectedFiles: any[] = [];
  nizKategorija: Kategorija[] = [];
  nizSlika: string[] = [];
  id: string = '';

  constructor(private servis:ServiceOglas, @Inject(MAT_DIALOG_DATA) public data: {oglas:Oglas,podkategorije:Kategorija[]}, private servisKat:ServiceKategorija, private dialogRef: MatDialogRef<CompEditovanjeOglasaComponent>) {
    console.log(this.data.podkategorije);
    console.log(this.nizKategorija);
  }

  ngOnInit(): void {
    if (this.data) {
      this.naziv = this.data.oglas.title;
      this.kategorija = this.data.oglas.podkategorija;
      this.opis = this.data.oglas.content;
      this.nizSlika = this.data.oglas.slike;
      this.id = this.data.oglas._id;
      this.nizKategorija=this.data.podkategorije;
    }
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
        this.selectedFiles.push({ file, url: e.target.result });
      };
      reader.readAsDataURL(file);
    }
  }

  editujOglas() {
    const formData = new FormData();
    formData.append('id', this.id);
    formData.append('title', this.naziv);
    formData.append('content', this.opis);
    formData.append('podkategorija', this.kategorija);
    formData.append('datumModifikacije', new Date().toISOString());

    this.selectedFiles.forEach((fileObj, index) => {
      formData.append('files', fileObj.file);
    });

    this.servis.editOglas(formData).subscribe(x => {
      this.dialogRef.close();
      location.reload();
    });
  }

}
