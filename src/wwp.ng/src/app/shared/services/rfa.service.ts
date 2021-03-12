import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';

import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { RFAProgram, OldRfaProgram } from '../models/rfa.model';
import { Utilities } from '../utilities';
import { LogService } from './log.service';
import { RfaEligibility } from '../models/server-message.model';
import { WhyReason } from '../models/why-reasons.model';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class RfaService extends BaseService {
  private rfaUrl: string;
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.rfaUrl = this.appService.apiServer + 'api/pin/';
  }

  public getRfasByPin(pin: string): Observable<RFAProgram[]> {
    return this.http.get(this.rfaUrl + pin + '/rfa').pipe(map(this.extractRfasData), catchError(this.handleError));
  }

  public getSortedRfasByPin(pin: string): Observable<RFAProgram[]> {
    return this.http.get(this.rfaUrl + pin + '/sortedrfa').pipe(map(this.extractRfasData), catchError(this.handleError));
  }
  public getOldRfasByPin(pin: string): Observable<OldRfaProgram[]> {
    return this.http.get(this.rfaUrl + pin + '/oldrfas').pipe(map(this.extractOldRfasData), catchError(this.handleError));
  }

  public getRfaById(id: string): Observable<RFAProgram> {
    return this.http.get(this.rfaUrl + this.getPin() + '/rfa/' + id).pipe(map(this.extractRfaData), catchError(this.handleError));
  }

  public doesRfaExistByTypeAndStatuses(pin: string, programName: string, statusQuery: string) {
    return this.http.put(this.rfaUrl + pin + '/rfa/' + programName + '/any?' + statusQuery, '').pipe(map(this.extractRfaData), catchError(this.handleError));
  }

  public preCheckRfaCreation(pin: number, rfa: RFAProgram): Observable<WhyReason> {
    return this.http.post(this.rfaUrl + pin + '/rfa/precheck', rfa).pipe(
      map(this.extractWhyReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  public validateRfa(pin: number, rfa: RFAProgram): Observable<WhyReason> {
    return this.http.post(this.rfaUrl + pin + '/rfa/validate/wpoffice', rfa).pipe(
      map(this.extractWhyReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  public denyRfaById(id: string, pin: string): Observable<RFAProgram> {
    return this.http.put(this.rfaUrl + pin + '/rfa/' + id + '/rfa-denied', '').pipe(map(this.extractRfaData), catchError(this.handleError));
  }

  public referRfaById(id: string, pin: string): Observable<RFAProgram> {
    return this.http.put(this.rfaUrl + pin + '/rfa/' + id + '/referred', '').pipe(map(this.extractRfaData), catchError(this.handleError));
  }

  public saveRfa(rfa: RFAProgram, pin: string): Observable<RFAProgram> {
    return this.http.post(this.rfaUrl + pin + '/rfa/' + rfa.id, rfa).pipe(
      map(this.extractRfaData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  public determineProgramEligibility(rfa: RFAProgram, pin: string): Observable<RfaEligibility> {
    return this.http.post(this.rfaUrl + pin + '/rfa/' + rfa.id + '/eligibility', rfa).pipe(
      map(this.extractRfaEligibility),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  private extractRfaEligibility(res: RfaEligibility): RfaEligibility {
    const jsonObj = res as RfaEligibility;
    const obj = new RfaEligibility().deserialize(jsonObj);
    return obj || null;
  }

  private extractRfaData(res: RFAProgram): RFAProgram {
    const jsonObj = res as RFAProgram;
    const obj = new RFAProgram().deserialize(jsonObj);
    return obj || null;
  }

  private extractBoolData(res: Boolean): Boolean {
    return res as Boolean;
  }

  private extractOldRfasData(res: OldRfaProgram[]): OldRfaProgram[] {
    const jsonObjs = res as OldRfaProgram[];
    const objs: OldRfaProgram[] = [];

    for (const obj of jsonObjs) {
      objs.push(new OldRfaProgram().deserialize(obj));
    }

    return objs || [];
  }
  private extractRfasData(res: RFAProgram[]): RFAProgram[] {
    const jsonObjs = res as RFAProgram[];
    const objs: RFAProgram[] = [];

    for (const obj of jsonObjs) {
      objs.push(new RFAProgram().deserialize(obj));
    }

    return objs || [];
  }

  private extractWhyReasonData(res: WhyReason) {
    const body = res as WhyReason;
    const whyReason = new WhyReason().deserialize(body);
    return whyReason || null;
  }
}
