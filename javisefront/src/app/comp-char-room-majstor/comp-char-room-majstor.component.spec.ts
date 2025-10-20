import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompCharRoomMajstorComponent } from './comp-char-room-majstor.component';

describe('CompCharRoomMajstorComponent', () => {
  let component: CompCharRoomMajstorComponent;
  let fixture: ComponentFixture<CompCharRoomMajstorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompCharRoomMajstorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompCharRoomMajstorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
