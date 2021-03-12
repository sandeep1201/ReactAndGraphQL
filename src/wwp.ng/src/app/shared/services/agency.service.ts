import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
// import { LogService } from './log.service';
import { Injectable } from '@angular/core';
import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { AgencyWorker } from '../models/agency-worker';
import { Office, Agency } from '../models/office';
import { ProgramWorker } from '../models/program-workers.model';
import { catchError, map } from 'rxjs/operators';
import { LogService } from './log.service';

@Injectable()
export class AgencyService extends BaseService {
  private agencyUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.agencyUrl = this.appService.apiServer + 'api/agency/';
  }

  /**
   * Deprecated.
   *
   * @param {number} officeNumber
   * @returns
   * @memberof AgencyService
   */
  public getAgencyOfficesBySrcOfficeForTransfers(officeNumber: number) {
    return this.http.get(this.agencyUrl + officeNumber + '/transfer-destinations').pipe(map(this.extractOfficeList), catchError(this.handleError));
  }

  public getAgencyWorkersByAgencyCode(agencyCode: string) {
    return this.http.get(this.agencyUrl + agencyCode + '/program-workers').pipe(map(this.extractList), catchError(this.handleError));
  }

  public getAgencyWorkersByAgencyCodeAndProgramCode(agencyCode: string, programCode: string) {
    return this.http.get(this.agencyUrl + agencyCode + '/program-workers/' + programCode).pipe(map(this.extractWorkers), catchError(this.handleError));
  }

  public getOfficesByMyAccess() {
    return this.http.get(this.agencyUrl + 'my-offices').pipe(map(this.extractOfficeList), catchError(this.handleError));
  }

  public getAgencies() {
    return this.http.get(this.agencyUrl + 'agencies').pipe(
      map(resp => resp.map(agency => new Agency().deserialize(agency))),
      catchError(err => this.handleError(err))
    );
  }

  private extractList(res: ProgramWorker[]): ProgramWorker[] {
    const body = res;
    const data: ProgramWorker[] = [];
    for (const item of body) {
      data.push(new ProgramWorker().deserialize(item));
    }

    return data || null;
  }

  private extractWorkers(res: ProgramWorker[]): AgencyWorker[] {
    const body = res;
    const data: ProgramWorker[] = [];
    for (const item of body) {
      data.push(new ProgramWorker().deserialize(item));
    }
    if (data[0] != null) {
      return data[0].agencyWorkers.sort((a, b) => a.lastName.localeCompare(b.lastName)) || null;
    } else {
      return null;
    }
  }

  private extractAgencyList(res: Agency[]): Agency[] {
    const body = res;
    const data: Agency[] = [];
    for (const item of body) {
      data.push(new Agency().deserialize(item));
    }
    return data || null;
  }

  private extractOfficeList(res: Office[]): Office[] {
    const body = res;
    const data: Office[] = [];
    for (const item of body) {
      data.push(new Office().deserialize(item));
    }
    return data || null;
  }
}
