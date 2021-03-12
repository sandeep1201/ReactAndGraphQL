import { TestBed, inject } from '@angular/core/testing';

import { JobReadinessService } from './job-readiness.service';

describe('JobReadinessService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [JobReadinessService]
    });
  });

  it('should be created', inject([JobReadinessService], (service: JobReadinessService) => {
    expect(service).toBeTruthy();
  }));
});
