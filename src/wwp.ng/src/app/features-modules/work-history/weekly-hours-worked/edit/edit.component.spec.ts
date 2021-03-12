import { WeeklyHoursWorkedService } from './../../services/weekly-hours-worked.service';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HourlyEntryEditComponent } from './edit.component';
import * as TypeMoq from 'typemoq';
import { AppService } from 'src/app/core/services/app.service';
import { WeeklyHoursWorked } from '../../models/weekly-hours-worked.model';

describe('EditComponent', () => {
  const appServiceMoq = TypeMoq.Mock.ofType<AppService>();
  const weeklyHoursWorkedServiceMoq = TypeMoq.Mock.ofType<WeeklyHoursWorkedService>();
  let component: HourlyEntryEditComponent;

  beforeEach(() => {
    component = new HourlyEntryEditComponent(weeklyHoursWorkedServiceMoq.object, appServiceMoq.object);
    let weeklyHoursWorkedEntry: WeeklyHoursWorked = new WeeklyHoursWorked();
    weeklyHoursWorkedEntry = Object.assign(
      {
        id: 19,
        employmentInformationId: 53528,
        startDate: '2019-06-02T00:00:00-05:00',
        hours: '40.00',
        details: 'THis is a test',
        totalSubsidyAmount: 80,
        isDeleted: false,
        modifiedBy: 'Sandeep Reddy Alalla',
        modifiedDate: '2020-07-14T14:57:48.58-05:00'
      },
      weeklyHoursWorkedEntry
    );

    component.weeklyHoursWorkedEntry = weeklyHoursWorkedEntry;
    component.weeklyHoursWorkedEntries = [weeklyHoursWorkedEntry];
  });

  it('calculate total Subsidy Hours', () => {
    expect(component.calculateTotalSubsidyHours()).toBe(40);
  });
});
