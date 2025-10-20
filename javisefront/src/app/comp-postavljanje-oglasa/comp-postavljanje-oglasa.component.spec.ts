import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompPostavljanjeOglasaComponent } from './comp-postavljanje-oglasa.component';

describe('CompPostavljanjeOglasaComponent', () => {
  let component: CompPostavljanjeOglasaComponent;
  let fixture: ComponentFixture<CompPostavljanjeOglasaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompPostavljanjeOglasaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompPostavljanjeOglasaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
