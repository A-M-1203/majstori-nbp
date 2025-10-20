import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompIzabraniMajstoriComponent } from './comp-izabrani-majstori.component';

describe('CompIzabraniMajstoriComponent', () => {
  let component: CompIzabraniMajstoriComponent;
  let fixture: ComponentFixture<CompIzabraniMajstoriComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompIzabraniMajstoriComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompIzabraniMajstoriComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
