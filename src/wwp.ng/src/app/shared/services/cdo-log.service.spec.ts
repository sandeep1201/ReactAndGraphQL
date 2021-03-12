import { TestBed, inject } from '@angular/core/testing';

import { CdoLogService } from './cdo-log.service';

describe('CdologService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CdoLogService]
    });
  });

  it('should be created', inject([CdoLogService], (service: CdoLogService) => {
    expect(service).toBeTruthy();
  }));
});
