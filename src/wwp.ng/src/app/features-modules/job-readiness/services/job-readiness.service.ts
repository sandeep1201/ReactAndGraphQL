// tslint:disable: deprecation
import { Injectable } from '@angular/core';
import { BaseService } from '../../../core/services/base.service';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { LogService } from '../../../shared/services/log.service';
import { AppService } from '../../../core/services/app.service';
import { JobReadiness } from '../models/job-readiness.model';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class JobReadinessService extends BaseService {
  private jobReadinessUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.jobReadinessUrl = this.appService.apiServer + 'api/job-readiness/';
  }

  public getJobReadinessData(pin?: string): Observable<JobReadiness> {
    return this.http.get(this.jobReadinessUrl + `${this.getPin() ? this.getPin() : pin}`).pipe(map(this.extractJobReadinessData), catchError(this.handleError));
  }

  public saveJobReadiness(pin: string, id: number, jobReadiness: JobReadiness, hasSaveErrors = false): Observable<JobReadiness> {
    return this.http
      .post(`${this.jobReadinessUrl}${this.getPin() ? this.getPin() : pin}/${id}/${hasSaveErrors}`, jobReadiness)
      .pipe(map(this.extractJobReadinessData), catchError(this.handleError));
  }

  private extractJobReadinessData(res: JobReadiness): JobReadiness {
    const jsonObjs = res as JobReadiness;
    const objs = new JobReadiness().deserialize(jsonObjs);
    return objs;
  }
}
