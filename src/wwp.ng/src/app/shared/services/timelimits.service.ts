import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { ExtensionContract, ExtensionSequenceContract, ReasonForChangeContract, TimelineMonthContract, ExtensionReasonContract } from './contracts/timelimits/service.contract';
import { BaseService } from './../../core/services/base.service';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import * as moment from 'moment';
// import * as faker from 'faker';

import { AppService } from './../../core/services/app.service';
import { AssistanceGroup, ClockTypes, Extension, Timeline, TimelineMonth, ReasonForChange, ExtensionReason, ExtensionSequence } from '../models/time-limits';
import { LogService } from './log.service';
import { take, map, catchError, flatMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';
// import { TimeLimitsTestService } from "app/shared/services/timelimits.service.test";

@Injectable()
export class TimeLimitsService extends BaseService {
  public static _w2StartDate: moment.Moment;
  public static get w2StartDate() {
    if (!TimeLimitsService._w2StartDate) {
      TimeLimitsService._w2StartDate = moment('1996-09-01');
    }
    return TimeLimitsService._w2StartDate.clone();
  }
  public static set w2StartDate(val: moment.Moment) {
    TimeLimitsService._w2StartDate = moment(val);
  }
  // testTimelimitsService: TimeLimitsTestService;

  // //observable sources
  // private clockSummarySelected = new Subject<ClockTypes>();
  // private timelineMonthSelected = new Subject<TimelineMonth>();

  // //observable streams
  // clockSummarySelected$ = this.clockSummarySelected.asObservable();
  // timelineMonthSelected$ = this.timelineMonthSelected.asObservable();

  // // commands
  // selectClockSummary(clockType: ClockTypes){
  //     this.clockSummarySelected.next(clockType);
  // }

  private assistanceGroupUrl: string;
  private extensionUrl: string;
  private timelineUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService, private logger: LogService) {
    super(http, logService);
    this.assistanceGroupUrl = this.appService.apiServer + 'api/assistancegroup/';
    this.extensionUrl = this.appService.apiServer + 'api/extensions/';
    this.timelineUrl = this.appService.apiServer + 'api/timelimits/';
    //TODO:Remove Test methods
    // this.testTimelimitsService = new TimeLimitsTestService();
  }

  public getTimeline(pinNumber: string): Observable<Timeline> {
    // let timeline = this.testTimelimitsService.getTestTimeline(pinNumber);
    // if (timeline) {
    //   return Observable.of(timeline);
    // }

    return this.http
      .get(this.timelineUrl + pinNumber)
      .pipe(take(1))
      .pipe(
        map(response => {
          return this.exractTimelineData(response);
        })
      );
    //return this.getTestTimeline(pinNumber);
  }

  public getTicksByPin(pinNumber: string, clockTypes: ClockTypes): Observable<TimelineMonth> {
    let t = this.getTimeline(pinNumber).pipe(
      flatMap(c => {
        return c.getTicks(clockTypes);
      })
    );
    return t;
  }

  public getExtensionSequence(pinNumber: string, extId: number) {
    // let ext = this.testTimelimitsService.getTestExtensionSequence(pinNumber, extId);
    // if (ext) {
    //   return Observable.of(ext)
    // }

    return this.http.get(this.extensionUrl + pinNumber + '/' + extId).pipe(
      map(response => {
        return this.extractExtensionSequenceData(response);
      }),
      catchError(this.handleError)
    );
  }

  public saveExtension(pin: string, model: Extension): Observable<ExtensionSequence> {
    let serializedData = model.serialize();
    return this.http.post(this.extensionUrl + pin, JSON.stringify(serializedData)).pipe(
      map(response => {
        this.getTimelineSnapshot(pin);
        return this.extractExtensionSequenceData(response);
      }),
      catchError(this.handleError)
    );
  }

  public saveMonths(pin: string, models: TimelineMonth[]): Observable<TimelineMonth[]> {
    let serializedData = [];
    for (let model of models) {
      serializedData.push(model.serialize());
    }
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.post(this.timelineUrl + pin + '/months/', JSON.stringify(serializedData)).pipe(
      map(response => {
        this.getTimelineSnapshot(pin);
        return this.extractDataTimelineMonths(response);
      }),
      catchError(this.handleError)
    );
  }

  public saveMonth(pin: string, model: TimelineMonth): Observable<TimelineMonth[]> {
    var data = model.serialize();
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.timelineUrl + pin + '/month', JSON.stringify(data)).pipe(
      map(response => {
        this.getTimelineSnapshot(pin);
        return this.extractDataTimelineMonth(response);
      }),
      catchError(this.handleError)
    );
  }

  getMonthHistory(pin: string, date: moment.MomentInput): Observable<TimelineMonth[]> {
    // TODO:
    return this.http.post(this.timelineUrl + pin + '/month/history', JSON.stringify(moment(date).toISOString())).pipe(
      map(response => {
        return this.extractDataTimelineMonths(response);
      }),
      catchError(this.handleError)
    );
  }

  public getReasonsForChange(): Observable<ReasonForChange[]> {
    return this.http
      .get(this.timelineUrl + 'month/change-reasons')
      .pipe(
        map(response => {
          return this.extractReasonsForChange(response);
        })
      )
      .pipe(catchError(this.handleError));

    //return ReasonForChange.selectableReasonCodes.slice(0);
  }

  public getExtensionReasons(type: 'Approval' | 'Denial'): Observable<Map<number, ExtensionReason>> {
    let requestUrl = this.extensionUrl + 'reasons' + '/' + type.toLowerCase();

    return this.http.get(requestUrl).pipe(
      map(x => {
        return this.extractExtensionReasons(x);
      }),
      catchError(this.handleError)
    );
  }

  public getTimelineSnapshot(pin: string) {
    this.http
      .get(this.timelineUrl + pin + '/snapshot')
      .pipe(catchError(this.handleError))
      .pipe(take(1))
      .subscribe(x => {
        this.logger.info('snapshot saved!');
      });
  }

  public getParticpantAssitanceGroup(pin: string): Observable<AssistanceGroup> {
    return this.http.get(this.assistanceGroupUrl + pin).pipe(
      map(x => {
        return this.extractAssistanceGroupData(x);
      }),
      catchError(this.handleError)
    );
  }

  private extractData(res: any) {
    let body = res;
    return body || {};
  }

  private extractAssistanceGroupData(res: AssistanceGroup): AssistanceGroup {
    let body = res || {};
    let assistanceGroup = new AssistanceGroup();
    if (body) {
      assistanceGroup.deserialize(res);
    }
    return assistanceGroup;
  }
  private exractTimelineData(res: Timeline): Timeline {
    let body = res as Timeline;
    let timeline = new Timeline(this.appService);

    if (body) {
      let monthsData = body.timelineMonths;
      let extensionsData = body.extensionSequences;

      for (let extData of extensionsData) {
        let contract = Object.assign(new ExtensionSequenceContract(), extData);
        let extension = ExtensionSequence.deserialize(contract);
        timeline.extensionSequences.push(extension);
      }

      for (let monthData in monthsData) {
        let month = TimelineMonth.deserialize(monthsData[monthData], this.logger);
        timeline.setTimelineMonth(month);
      }
    }
    return timeline;
  }

  private extractExtensionSequenceData(res: ExtensionSequence): ExtensionSequence {
    let body = res;
    let extSequence: ExtensionSequence = null;
    if (body) {
      let contract = Object.assign(new ExtensionSequenceContract(), body);
      extSequence = ExtensionSequence.deserialize(contract);
    }
    return extSequence;
  }

  private extractExtensionData(res: Extension): Extension {
    let body = res;
    let ext: Extension = null;
    if (body) {
      let contract = Object.assign(new ExtensionContract(), body);
      ext = Extension.deserialize(contract);
    }
    return ext;
  }

  private extractExtensionReasons(res: Response): Map<number, ExtensionReason> {
    let reasons = new Map<number, ExtensionReason>();

    let body = res as any;
    if (body) {
      for (let reasonData of body) {
        let contract = Object.assign(new ExtensionReasonContract(), reasonData);
        let reason = ExtensionReason.deserialize(contract);
        reasons.set(reason.id, reason);
      }
    }

    return reasons;
  }

  private extractDataTimelineMonths(res: Response): TimelineMonth[] {
    let body = res || [];
    let months: TimelineMonth[] = [];
    if (body) {
      for (let monthData in body) {
        let contract = Object.assign(new TimelineMonthContract(), body[monthData]);
        let month = TimelineMonth.deserialize(contract, this.logger);
        if (month.tick.tickType == ClockTypes.CMC && !month.tick.clockTypes.state.State && !month.tick.clockTypes.state.Federal) {
        } else months.push(month);
      }
    }
    return months;
  }

  private extractDataTimelineMonth(res: Response): TimelineMonth[] {
    let body = res;
    let contract = Object.assign(new TimelineMonthContract(), body);
    let months: TimelineMonth[] = [];
    let month: TimelineMonth = null;
    if (body) {
      month = TimelineMonth.deserialize(contract, this.logger);
      months.push(month);
    }
    return months;
  }

  private extractReasonsForChange(res: Response): ReasonForChange[] {
    let body = res;

    let reasonsForChange: ReasonForChange[] = [];
    for (let reasonData in body) {
      let contract = Object.assign(new ReasonForChangeContract(), body[reasonData]);
      let model = ReasonForChange.deserialize(contract);
      reasonsForChange.push(model);
    }
    return reasonsForChange;
  }
}
