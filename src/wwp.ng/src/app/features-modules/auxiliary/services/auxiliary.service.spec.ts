import { TestBed } from '@angular/core/testing';

import { AuxiliaryService } from './auxiliary.service';

describe('AuxiliaryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AuxiliaryService = TestBed.get(AuxiliaryService);
    expect(service).toBeTruthy();
  });
});
