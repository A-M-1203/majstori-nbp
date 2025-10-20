import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompEditovanjeOglasaComponent } from './comp-editovanje-oglasa.component';

describe('CompEditovanjeOglasaComponent', () => {
  let component: CompEditovanjeOglasaComponent;
  let fixture: ComponentFixture<CompEditovanjeOglasaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompEditovanjeOglasaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompEditovanjeOglasaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
