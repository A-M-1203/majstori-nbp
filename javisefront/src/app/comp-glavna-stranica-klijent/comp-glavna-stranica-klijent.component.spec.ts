import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompGlavnaStranicaKlijentComponent } from './comp-glavna-stranica-klijent.component';

describe('CompGlavnaStranicaKlijentComponent', () => {
  let component: CompGlavnaStranicaKlijentComponent;
  let fixture: ComponentFixture<CompGlavnaStranicaKlijentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompGlavnaStranicaKlijentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompGlavnaStranicaKlijentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
