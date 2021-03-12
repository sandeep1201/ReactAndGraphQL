import { SupportiveService } from '../models/supportive-service.model';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { BaseService } from '../../../core/services/base.service';
import { AppService } from '../../../core/services/app.service';
import { LogService } from '../../../shared/services/log.service';
import { Observable } from 'rxjs';
import { Utilities } from '../../../shared/utilities';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class SupportiveServiceService extends BaseService {
  private ssUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.ssUrl = this.appService.apiServer + 'api/employability-plan/';
  }

  public getSupportiveServices(pin: string, epId: string): Observable<SupportiveService[]> {
    return this.http.get(this.ssUrl + pin + '/employment-plan/' + epId + '/' + 'supportive-service').pipe(map(this.extractSssData), catchError(this.handleError));
  }

  public saveSupportiveService(pin: string, epId: number, SupportiveService: SupportiveService[]) {
    const body = JSON.stringify(SupportiveService);

    return this.http.post(this.ssUrl + pin + '/employment-plan/' + epId + '/' + 'supportive-service', body).pipe(map(this.extractSsData), catchError(this.handleError));
  }

  private extractSsData(res: SupportiveService[]): SupportiveService[] {
    const jsonObjs = res as SupportiveService[];
    const objs: SupportiveService[] = [];

    for (const obj of jsonObjs) {
      objs.push(new SupportiveService().deserialize(obj));
    }

    return objs || [];
  }

  private extractSssData(res: SupportiveService[]): SupportiveService[] {
    const jsonObjs = res as SupportiveService[];
    const objs: SupportiveService[] = [];

    for (const obj of jsonObjs) {
      objs.push(new SupportiveService().deserialize(obj));
    }

    return objs || [];
  }
}
