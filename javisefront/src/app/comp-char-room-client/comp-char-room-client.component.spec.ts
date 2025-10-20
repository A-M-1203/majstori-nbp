import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompCharRoomClientComponent } from './comp-char-room-client.component';

describe('CompCharRoomClientComponent', () => {
  let component: CompCharRoomClientComponent;
  let fixture: ComponentFixture<CompCharRoomClientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompCharRoomClientComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CompCharRoomClientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
