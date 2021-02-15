import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploeeListComponent } from './emploee-list.component';

describe('EmploeeListComponent', () => {
  let component: EmploeeListComponent;
  let fixture: ComponentFixture<EmploeeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmploeeListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploeeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
