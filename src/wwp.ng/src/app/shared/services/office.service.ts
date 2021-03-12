import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';
import { Injectable } from '@angular/core';
import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { AgencyWorker } from '../models/agency-worker';
import { OfficeSummary } from '../models/office-summary.model';
import { map, catchError } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';
import { observable } from 'rxjs';

@Injectable()
export class OfficeService extends BaseService {
  private officeUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.officeUrl = this.appService.apiServer + 'api/office/';
  }

  public getOfficesByProgramAndWIUID(programCode: string, wiuid: string) {
    return this.http.get(this.officeUrl + programCode + '/' + wiuid).pipe(
      map(res => this.extractOfficeList(res)),
      catchError(this.handleError)
    );
  }

  public getTransferOfficesByProgramWorkerSourceOffice(programCode: string, mainFrameId: string, sourceOfficeId: number) {
    return this.http.get(this.officeUrl + programCode + '/' + mainFrameId + '/' + sourceOfficeId).pipe(
      map(res => this.extractOfficeList(res)),
      catchError(this.handleError)
    );
  }

  public getOfficeForProgramAndCounty(programCode: string, countyOrTribeId: number) {
    return this.http.get(this.officeUrl + 'program/' + programCode + '/county/' + countyOrTribeId).pipe(
      map(res => {
        return this.extractOffice(res);
      }),
      catchError(this.handleError)
    );
  }

  private extractWorkerList(res: AgencyWorker[], isSortedByFirstName: boolean): AgencyWorker[] {
    const body = res as AgencyWorker[];
    const data: AgencyWorker[] = [];

    for (const item of body) {
      data.push(new AgencyWorker().deserialize(item));
    }

    if (isSortedByFirstName === true && data != null) {
      data.sort((a, b) => a.firstName.localeCompare(b.firstName));
    }

    return data || null;
  }

  private extractOfficeList(res: OfficeSummary[]): OfficeSummary[] {
    const body = res as OfficeSummary[];
    const data: OfficeSummary[] = [];
    for (const item of body) {
      data.push(new OfficeSummary().deserialize(item));
    }
    return data || null;
  }

  private extractOffice(res: OfficeSummary): OfficeSummary {
    const body = res as OfficeSummary;
    const data = new OfficeSummary().deserialize(body);
    return data;
  }
}
