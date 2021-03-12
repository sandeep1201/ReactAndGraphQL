import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { TestScore } from '../models/test-scores';
import { Utilities } from '../utilities';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';

@Injectable()
export class TestScoresService extends BaseService {
  private testScoresUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.testScoresUrl = this.appService.apiServer + 'api/test-score/';
  }

  // public getTestScoreById(id: number) {
  //   return this.http.get(this.testScoresUrl + this.getPin() + '/' + id)
  //     .map(this.extractTestScoresList)
  //     .catch(this.handleError);
  // }

  public getTestScoreList() {
    return this.http.get(this.testScoresUrl + this.getPin()).pipe(map(this.extractTestScoreList), catchError(this.handleError));
  }

  public getTestScoreByExam(tab: string) {
    return this.http.get(this.testScoresUrl + this.getPin() + '/' + tab).pipe(map(this.extractTestScoreList), catchError(this.handleError));
  }

  public getTestScoreById(id: number) {
    return this.http.get(this.testScoresUrl + this.getPin() + '/exam/' + id).pipe(map(this.extractTestScoreByExam), catchError(this.handleError));
  }

  public deleteTestScoreExam(id: number): Observable<Response> {
    const body = JSON.stringify('');

    return this.http.post(this.testScoresUrl + this.getPin() + '/delete/' + id, body).pipe(catchError(this.handleError));
  }

  public postTestScoreExam(model: TestScore): Observable<Response> {
    const body = JSON.stringify(model);

    return this.http.post(this.testScoresUrl + this.getPin(), body).pipe(catchError(this.handleError));
  }

  private extractTestScoreList(res: TestScore[]): TestScore[] {
    const body = res as TestScore[];
    const data: TestScore[] = [];
    if (body != null) {
      for (const pb of body) {
        data.push(new TestScore().deserialize(pb));
      }
    }
    return data || null;
  }
  private extractTestScoreByExam(res: TestScore): TestScore {
    const body = res as TestScore;
    const e = new TestScore().deserialize(body);
    return e || null;
  }
}
