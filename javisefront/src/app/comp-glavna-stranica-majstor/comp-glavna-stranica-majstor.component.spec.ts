import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompGlavnaStranicaMajstorComponent } from './comp-glavna-stranica-majstor.component';

describe('CompGlavnaStranicaMajstorComponent', () => {
  let component: CompGlavnaStranicaMajstorComponent;
  let fixture: ComponentFixture<CompGlavnaStranicaMajstorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompGlavnaStranicaMajstorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompGlavnaStranicaMajstorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
