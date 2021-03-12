import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkerLevelPopClaimsListComponent } from './list.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { PopClaimsService } from '../../services/pop-claims.service';

describe('ListComponent', () => {
  let component: WorkerLevelPopClaimsListComponent;
  let fixture: ComponentFixture<WorkerLevelPopClaimsListComponent>;
  let mockPopClaimsService: PopClaimsService = jasmine.createSpyObj(['getPops']);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [WorkerLevelPopClaimsListComponent],
      providers: [{ provide: PopClaimsService, useValue: mockPopClaimsService }],
      schemas: [NO_ERRORS_SCHEMA]
    });
    fixture = TestBed.createComponent(WorkerLevelPopClaimsListComponent);
    component = fixture.componentInstance;
  }));

  beforeEach(() => {
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('sub header present', () => {
    expect(fixture.nativeElement.querySelector('[data-test="sub-header"]')).toBeTruthy();
  });
  it('test for loading component to be present', () => {
    expect(fixture.nativeElement.querySelector('[data-test="pop-claims-loading"]')).toBeTruthy();
  });
  it('test for no pop claims text', () => {
    expect(fixture.nativeElement.querySelector('[data-test="no-pops"]')).toBeTruthy();
  });

  it('test for no pop claims inner HTML ', () => {
    const el = fixture.debugElement.nativeElement;
    const content = el.querySelector('[data-test="no-pops"] p');
    expect(content.innerHTML).toEqual('No POP Claims');
  });

  it('test for pop list to be present', () => {
    expect(fixture.nativeElement.querySelectorAll('[data-test="pops"]').length).toBeGreaterThan(0);
  });
  it('test for the add button to be present', () => {
    expect(fixture.nativeElement.querySelector('[data-test="add-pop-claims-button"]')).toBeTruthy();
  });
  // it('test for onAdd when the add pop claims button is clicked', () => {
  //   spyOn(component, 'onAdd');
  //   const btn = fixture.nativeElement.querySelector('[data-test="add-pop-claims-button"] button');
  //   btn.click();
  //   expect(component.onAdd).toHaveBeenCalled();
  // });
  it('test for onAdd when the add pop claims button is clicked and set the EDIT mode to true', () => {
    const btn = fixture.nativeElement.querySelector('[data-test="add-pop-claims-button"] button');
    btn.click();
    expect(component.inEditMode).toBeTruthy();
  });
  // test if it has some pop claims by hard coding the elements
  // then convert them to read data from the component
  //then inject the service and make sure the tests work
});
