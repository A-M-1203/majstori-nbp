import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompRegistracija3MajstorComponent } from './comp-registracija3-majstor.component';

describe('CompRegistracija3MajstorComponent', () => {
  let component: CompRegistracija3MajstorComponent;
  let fixture: ComponentFixture<CompRegistracija3MajstorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompRegistracija3MajstorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompRegistracija3MajstorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
