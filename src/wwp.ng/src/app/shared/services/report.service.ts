import { Injectable } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';

import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';
import { Employment } from '../models/work-history-app';
import { Utilities } from '../utilities';
import { map } from 'rxjs/internal/operators/map';
import { catchError } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';
import { PrintEP } from '../models/print-ep.model';

@Injectable()
export class ReportService extends BaseService {
  // tslint:disable: deprecation
  private reportUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.reportUrl = this.appService.apiServer + 'api/reports/';
  }

  getEmploymentReport(employments: Employment[], pin: string) {
    const body = JSON.stringify(employments);
    let pdfHeaders = new HttpHeaders();
    pdfHeaders = pdfHeaders.set('Accept', 'application/json');
    pdfHeaders = pdfHeaders.set('Content-Type', 'application/json');
    const responseType = { responseType: 'blob' as 'json', headers: pdfHeaders };

    return this.http.post(this.reportUrl + pin + '/work-history', body, responseType).pipe(map(this.extractBlobData), catchError(this.handleError));
  }

  getEpReport(pin: string, body: PrintEP) {
    let pdfHeaders = new HttpHeaders();
    pdfHeaders = pdfHeaders.set('Accept', 'application/json');
    pdfHeaders = pdfHeaders.set('Content-Type', 'application/json');
    const responseType = { responseType: 'blob' as 'json', headers: pdfHeaders };

    return this.http.post(this.reportUrl + pin + '/ep', body, responseType).pipe(map(this.extractBlobData), catchError(this.handleError));
  }

  getBatchDetailsReport(pin: string, participationPeriod: string, periodYear: string, caseNumbers: string) {
    let pdfHeaders = new HttpHeaders();
    pdfHeaders = pdfHeaders.set('Accept', 'application/json');
    pdfHeaders = pdfHeaders.set('Content-Type', 'application/json');
    const responseType = { responseType: 'blob' as 'json', headers: pdfHeaders };

    return this.http
      .get(`${this.reportUrl}${pin}/batch-details/${participationPeriod}/${periodYear}/${caseNumbers}`, responseType)
      .pipe(map(this.extractBlobData), catchError(this.handleError));
  }

  private extractBlobData(res: Blob) {
    const body = res as Blob;
    return body;
  }
}
