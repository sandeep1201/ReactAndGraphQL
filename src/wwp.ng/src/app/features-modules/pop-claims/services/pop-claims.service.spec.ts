import { TestBed, tick, fakeAsync } from '@angular/core/testing';
import { PopClaimsService } from './pop-claims.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of } from 'rxjs';

describe('PopClaimsService', () => {
  let mockPopClaimsService: PopClaimsService;
  let mockHttpClient: HttpClient;
  const mockPops = [
    { cliamType: 'A', claimStatus: 'Approved', worker: 'abc' },
    { cliamType: 'B', claimStatus: 'Denied', worker: 'xyz' },
    { cliamType: 'A', claimStatus: 'Pending', worker: 'bdc' }
  ];
  beforeEach(() =>
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, HttpClientModule],
      providers: [PopClaimsService]
    })
  );
  beforeEach(() => {
    mockPopClaimsService = TestBed.get(PopClaimsService);
    mockHttpClient = TestBed.get(HttpClient);
  });

  // Spy on and mock HttpClient
  // use our service to get the pops
  // verify that the service returned the mocked data
  // verify that the service called the proper Http end point

  // it('should be created', () => {
  //   const service: PopClaimsService = TestBed.get(PopClaimsService);
  //   expect(service).toBeTruthy();
  // });
  it('test for few Http things', fakeAsync(() => {
    const spy = jasmine.createSpy('spy');
    spyOn(mockHttpClient, 'get').and.returnValue(of(mockPops));
    mockPopClaimsService.getPopClaims().subscribe(spy);

    tick();
    expect(mockHttpClient.get).toHaveBeenCalledWith('api/pops');
  }));
});
