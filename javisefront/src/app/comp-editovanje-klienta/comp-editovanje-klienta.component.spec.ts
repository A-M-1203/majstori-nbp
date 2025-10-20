import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompEditovanjeKlientaComponent } from './comp-editovanje-klienta.component';

describe('CompEditovanjeKlientaComponent', () => {
  let component: CompEditovanjeKlientaComponent;
  let fixture: ComponentFixture<CompEditovanjeKlientaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompEditovanjeKlientaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompEditovanjeKlientaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
