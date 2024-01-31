import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamtreeComponent } from './teamtree.component';

describe('TeamtreeComponent', () => {
  let component: TeamtreeComponent;
  let fixture: ComponentFixture<TeamtreeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamtreeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamtreeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
