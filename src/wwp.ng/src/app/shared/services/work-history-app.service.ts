import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs';

import { AppService } from '../../core/services/app.service';
import { BaseService } from '../../core/services/base.service';
import { Employment } from '../models/work-history-app';
import { AuthHttpClient } from '../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';
import { WhyReason } from '../models/why-reasons.model';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class WorkHistoryAppService extends BaseService {
  // tslint:disable: deprecation
  private workHistoryUrl: string;
  private deletePreCheckUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.workHistoryUrl = this.appService.apiServer + 'api/employments/';
    this.deletePreCheckUrl = this.appService.apiServer + 'api/employments/';
  }

  getEmploymentList(pin?: number) {
    return this.http.get(this.workHistoryUrl + `${this.getPin() ? this.getPin() : pin}`).pipe(map(this.extractEmploymentsData), catchError(this.handleError));
  }

  deletePreCheck(id: number): Observable<WhyReason> {
    return this.http.get(this.deletePreCheckUrl + this.getPin() + '/predelete/' + id).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return Observable.throw(err);
      })
    );
  }

  upsertPreCheck(id: number, model: Employment, isHD: boolean): Observable<WhyReason> {
    const url = `${this.workHistoryUrl}${this.getPin()}/preAdd/${model.id}/${isHD}`;
    const body = JSON.stringify(model);

    return this.http.post(url, body).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return Observable.throw(err);
      })
    );
  }

  getEmployment(id: number) {
    return this.http.get(this.workHistoryUrl + this.getPin() + '/' + id).pipe(map(this.extractEmploymentData), catchError(this.handleError));
  }

  deleteEmployment(id: number, deleteReason: number) {
    return this.http.delete(this.workHistoryUrl + 'delete/' + this.getPin() + '/' + id + '/' + deleteReason).pipe(catchError(this.handleError));
  }

  postEmployment(model: Employment): Observable<Response> {
    const body = JSON.stringify(model);

    return this.http.post(this.workHistoryUrl + this.getPin() + '/' + model.id, body).pipe(catchError(this.handleError));
  }

  private extractEmploymentsData(res: Employment[]): Employment[] {
    const body = res as Employment[];
    const employments: Employment[] = [];
    for (const emp of body) {
      employments.push(new Employment().deserialize(emp));
    }
    return employments || null;
  }

  private extractEmploymentData(res: Employment): Employment {
    const body = res as Employment;
    const e = new Employment().deserialize(body);
    return e || null;
  }
}
