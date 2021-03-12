import { Utilities } from 'src/app/shared/utilities';
import { AppService } from 'src/app/core/services/app.service';
import { LogService } from './../../../shared/services/log.service';
import { BaseService } from 'src/app/core/services/base.service';
import { Injectable } from '@angular/core';
import { ParticipationTracking } from './../../../shared/models/participation-tracking.model';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { catchError, map } from 'rxjs/operators';
import * as moment from 'moment';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ParticipationTrackingService extends BaseService {
  private PTUrl: string;
  public momentDate = moment(Utilities.currentDate).toDate();

  public canEdit = false;
  // this is  only for Participation Tracking.
  public viewDate = new BehaviorSubject<any>({ viewDate: moment(new Date(this.momentDate.getFullYear(), this.momentDate.getMonth(), 16)).toDate() });
  public modeForParticipationEntry = new BehaviorSubject<any>({ readOnly: false, inEditView: false });
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.PTUrl = this.appService.apiServer + 'api/participation-tracking/';
  }

  getParticipationTrackingDetails(pin, participantId, startDate, endDate, isFromDetails) {
    return this.http.get(`${this.PTUrl}${pin}/${participantId}/${startDate}/${endDate}/${isFromDetails}`).pipe(map(this.extractPTsData), catchError(this.handleError));
  }
  postParticipationTrackingDetails(pin, participationTracking) {
    return this.http.post(`${this.PTUrl}${pin}/save`, participationTracking).pipe(map(this.extractPTData), catchError(this.handleError));
  }

  deleteParticipationTrackingDetails(pin, participantId, id) {
    return this.http.delete(`${this.PTUrl}${pin}/${id}/${participantId}/delete`).pipe(map(this.extractPTData), catchError(this.handleError));
  }
  makeFullOrNoParticipation(pin: string, participantId: number, fullOrNoParticipation: string, startDate, endDate, participationTracking: ParticipationTracking[]) {
    return this.http
      .post(`${this.PTUrl}${pin}/${participantId}/${fullOrNoParticipation}/${startDate}/${endDate}`, participationTracking)
      .pipe(map(this.extractPTsData), catchError(this.handleError));
  }

  private extractPTsData(res: ParticipationTracking[]): ParticipationTracking[] | null {
    const jsonObjs = res as ParticipationTracking[];
    const objs: ParticipationTracking[] = [];

    for (const obj of jsonObjs) {
      objs.push(new ParticipationTracking().deserialize(obj));
    }

    return objs || [];
  }
  private extractPTData(res: ParticipationTracking): ParticipationTracking {
    const objs = new ParticipationTracking().deserialize(res);
    return objs;
  }
}
