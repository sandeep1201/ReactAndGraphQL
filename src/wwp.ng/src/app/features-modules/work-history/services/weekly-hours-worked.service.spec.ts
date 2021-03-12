import { TestBed } from '@angular/core/testing';

import { WeeklyHoursWorkedService } from './weekly-hours-worked.service';

describe('WeeklyHoursWorkedService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WeeklyHoursWorkedService = TestBed.get(WeeklyHoursWorkedService);
    expect(service).toBeTruthy();
  });
});
