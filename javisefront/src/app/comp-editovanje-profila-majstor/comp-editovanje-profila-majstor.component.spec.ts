import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompEditovanjeProfilaMajstorComponent } from './comp-editovanje-profila-majstor.component';

describe('CompEditovanjeProfilaMajstorComponent', () => {
  let component: CompEditovanjeProfilaMajstorComponent;
  let fixture: ComponentFixture<CompEditovanjeProfilaMajstorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompEditovanjeProfilaMajstorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompEditovanjeProfilaMajstorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
