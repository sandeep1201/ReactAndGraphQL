import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourcesRepeaterComponent } from './resources-repeater.component';

describe('ResourcesRepeaterComponent', () => {
  let component: ResourcesRepeaterComponent;
  let fixture: ComponentFixture<ResourcesRepeaterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourcesRepeaterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourcesRepeaterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
