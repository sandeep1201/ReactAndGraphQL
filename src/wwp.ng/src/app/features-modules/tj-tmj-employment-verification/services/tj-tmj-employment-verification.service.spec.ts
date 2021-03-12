import { TestBed } from '@angular/core/testing';

import { TJTMJEmploymentVerificationService } from './tj-tmj-employment-verification.service';

describe('TJTMJEmploymentVerificationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TJTMJEmploymentVerificationService = TestBed.get(TJTMJEmploymentVerificationService);
    expect(service).toBeTruthy();
  });
});
