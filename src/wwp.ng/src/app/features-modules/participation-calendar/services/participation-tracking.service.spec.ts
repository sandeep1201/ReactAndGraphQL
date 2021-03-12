import { TestBed } from '@angular/core/testing';

import { ParticipationTrackingService } from './participation-tracking.service';

describe('ParticipationTrackingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ParticipationTrackingService = TestBed.get(ParticipationTrackingService);
    expect(service).toBeTruthy();
  });
});
