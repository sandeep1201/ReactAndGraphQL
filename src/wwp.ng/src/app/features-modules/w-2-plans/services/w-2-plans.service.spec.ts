import { TestBed } from '@angular/core/testing';
import { W2PlansService } from './w-2-plans.service';

describe('W2PlansService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: W2PlansService = TestBed.get(W2PlansService);
    expect(service).toBeTruthy();
  });
});
