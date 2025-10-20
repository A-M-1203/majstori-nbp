import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompRegistracija3Component } from './comp-registracija3.component';

describe('CompRegistracija3Component', () => {
  let component: CompRegistracija3Component;
  let fixture: ComponentFixture<CompRegistracija3Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompRegistracija3Component]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompRegistracija3Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
