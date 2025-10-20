import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompRegistracijaComponent } from './comp-registracija.component';

describe('CompRegistracijaComponent', () => {
  let component: CompRegistracijaComponent;
  let fixture: ComponentFixture<CompRegistracijaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompRegistracijaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompRegistracijaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
