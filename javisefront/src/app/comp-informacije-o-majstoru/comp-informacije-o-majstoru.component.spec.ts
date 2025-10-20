import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompInformacijeOMajstoruComponent } from './comp-informacije-o-majstoru.component';

describe('CompInformacijeOMajstoruComponent', () => {
  let component: CompInformacijeOMajstoruComponent;
  let fixture: ComponentFixture<CompInformacijeOMajstoruComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompInformacijeOMajstoruComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompInformacijeOMajstoruComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
