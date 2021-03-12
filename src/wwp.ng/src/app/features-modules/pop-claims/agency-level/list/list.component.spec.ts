import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { AgencyLevelPOPClaimsListComponent } from './list.component';

describe('AgencyLevelListComponent', () => {
  let component: AgencyLevelPOPClaimsListComponent;
  let fixture: ComponentFixture<AgencyLevelPOPClaimsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AgencyLevelPOPClaimsListComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyLevelPOPClaimsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
