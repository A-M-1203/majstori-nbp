import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompKategorijaInformacijeComponent } from './comp-kategorija-informacije.component';

describe('CompKategorijaInformacijeComponent', () => {
  let component: CompKategorijaInformacijeComponent;
  let fixture: ComponentFixture<CompKategorijaInformacijeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompKategorijaInformacijeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompKategorijaInformacijeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
