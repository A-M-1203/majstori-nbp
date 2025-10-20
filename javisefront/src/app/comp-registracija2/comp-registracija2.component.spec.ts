import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompRegistracija2Component } from './comp-registracija2.component';

describe('CompRegistracija2Component', () => {
  let component: CompRegistracija2Component;
  let fixture: ComponentFixture<CompRegistracija2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompRegistracija2Component]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompRegistracija2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
