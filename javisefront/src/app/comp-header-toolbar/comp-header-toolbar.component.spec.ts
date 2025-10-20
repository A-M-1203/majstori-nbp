import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompHeaderToolbarComponent } from './comp-header-toolbar.component';

describe('CompHeaderToolbarComponent', () => {
  let component: CompHeaderToolbarComponent;
  let fixture: ComponentFixture<CompHeaderToolbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompHeaderToolbarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompHeaderToolbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
