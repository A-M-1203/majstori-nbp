import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompInformacijeOOglasuComponent } from './comp-informacije-o-oglasu.component';

describe('CompInformacijeOOglasuComponent', () => {
  let component: CompInformacijeOOglasuComponent;
  let fixture: ComponentFixture<CompInformacijeOOglasuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompInformacijeOOglasuComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompInformacijeOOglasuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
