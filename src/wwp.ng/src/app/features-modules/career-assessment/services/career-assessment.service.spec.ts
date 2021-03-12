import { TestBed, inject } from '@angular/core/testing';

import { CareerAssessmentService } from './career-assessment.service';

describe('CareerAssessmentService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CareerAssessmentService]
    });
  });

  it('should be created', inject([CareerAssessmentService], (service: CareerAssessmentService) => {
    expect(service).toBeTruthy();
  }));
});
