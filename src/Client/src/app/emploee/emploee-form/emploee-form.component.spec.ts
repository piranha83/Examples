import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploeeFormComponent } from './emploee-form.component';

describe('EmploeeFormComponent', () => {
  let component: EmploeeFormComponent;
  let fixture: ComponentFixture<EmploeeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmploeeFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploeeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
