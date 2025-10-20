import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompLogovanjeComponent } from './comp-logovanje.component';

describe('CompLogovanjeComponent', () => {
  let component: CompLogovanjeComponent;
  let fixture: ComponentFixture<CompLogovanjeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompLogovanjeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompLogovanjeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
