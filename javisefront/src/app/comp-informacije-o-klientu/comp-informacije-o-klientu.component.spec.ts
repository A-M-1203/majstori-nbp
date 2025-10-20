import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompInformacijeOKlientuComponent } from './comp-informacije-o-klientu.component';

describe('CompInformacijeOKlientuComponent', () => {
  let component: CompInformacijeOKlientuComponent;
  let fixture: ComponentFixture<CompInformacijeOKlientuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompInformacijeOKlientuComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompInformacijeOKlientuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
