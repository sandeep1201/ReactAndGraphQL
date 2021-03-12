import { httpFactory } from '@angular/http/src/http_module';
import { LogService } from 'src/app/shared/services/log.service';
import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { BaseService } from '../../../core/services/base.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { Utilities } from '../../../shared/utilities';
import { map, catchError } from 'rxjs/operators';
import * as moment from 'moment';
import { ChildrenFirstTracking } from '../models/children-first-tracking.model';
import { AppService } from 'src/app/core/services/app.service';

@Injectable()
export class ChildrenFirstTrackingService extends BaseService {
  // this is  only for Children-first-tracking.
  public viewDate: BehaviorSubject<{ viewDate: Date }> = new BehaviorSubject<{ viewDate: Date }>({ viewDate: moment().toDate() });
  public CTUrl: string = '';
  public modeForCFParticipationEntry = new BehaviorSubject<any>({ readOnly: false, inEditView: false });

  constructor(http: AuthHttpClient, log: LogService, private appService: AppService) {
    super(http, log);
    this.CTUrl = this.appService.apiServer + 'api/participation-tracking/';
  }
  getChildrenTrackingDetails(pin: string, participantId: number, startDate: string, endDate: string, programCode: string, isFromDetails: boolean) {
    startDate = startDate.split('-').join('');
    endDate = endDate.split('-').join('');
    return this.http
      .get(`${this.CTUrl}${pin}/${participantId}/${startDate}/${endDate}/${isFromDetails}/${programCode}`)
      .pipe(map(this.extractCTsData), catchError(this.handleError));
  }
  postChildrenFirstTrackingDetails(pin, participationTracking, programCode) {
    return this.http.post(`${this.CTUrl}${pin}/save/${programCode}`, participationTracking).pipe(map(this.extractCTData), catchError(this.handleError));
  }

  makeFullOrNoParticipation(
    pin: string,
    participantId: number,
    fullOrNoParticipation: string,
    startDate,
    endDate,
    programCode: string,
    participationTracking: ChildrenFirstTracking[]
  ) {
    startDate = startDate.split('-').join('');
    endDate = endDate.split('-').join('');
    return this.http
      .post(`${this.CTUrl}${pin}/${participantId}/${fullOrNoParticipation}/${startDate}/${endDate}/${programCode}`, participationTracking)
      .pipe(map(this.extractCTsData), catchError(this.handleError));
  }

  private extractCTsData(res: ChildrenFirstTracking[]): ChildrenFirstTracking[] | null {
    const jsonObjs = res as ChildrenFirstTracking[];
    const objs: ChildrenFirstTracking[] = [];

    for (const obj of jsonObjs) {
      objs.push(new ChildrenFirstTracking().deserialize(obj));
    }

    return objs || [];
  }
  private extractCTData(res: ChildrenFirstTracking): ChildrenFirstTracking {
    const objs = new ChildrenFirstTracking().deserialize(res);
    return objs;
  }
}
