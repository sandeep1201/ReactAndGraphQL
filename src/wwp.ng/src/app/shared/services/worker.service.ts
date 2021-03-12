import { Injectable } from '@angular/core';
import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { AgencyWorker } from '../models/agency-worker';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class WorkerService extends BaseService {
  private officeUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.officeUrl = this.appService.apiServer + 'api/workers/';
  }

  public getWorkersByOrgAndRole(org: string, roleCode?: string) {
    const url = roleCode ? `${this.officeUrl}${org}/${roleCode}` : `${this.officeUrl}${org}`;
    return this.http.get(url).pipe(
      map(res => this.extractWorkerList(res, true)),
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
}
