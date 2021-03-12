import { Injectable } from '@angular/core';
import { BaseService } from './../../core/services/base.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';
import { AppService } from './../../core/services/app.service';
import { Utilities } from '../utilities';
import { SimulatedDate } from '../models/simulated-date.model';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class CdoLogService extends BaseService {
  private cdoLogUrl: string;
  public cachedSimulatedDate: SimulatedDate;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.cdoLogUrl = this.appService.apiServer + 'api/current-date-override/log';
  }

  public postCurrentDateOverrideLog(simulatedDate: SimulatedDate): Observable<SimulatedDate> {
    return this.http.post(this.cdoLogUrl, simulatedDate).pipe(map(this.extractSimulatedDateData), catchError(this.handleError));
  }

  private extractSimulatedDateData(res: any): SimulatedDate {
    const obj = new SimulatedDate().deserialize(res);
    return obj || null;
  }
}
