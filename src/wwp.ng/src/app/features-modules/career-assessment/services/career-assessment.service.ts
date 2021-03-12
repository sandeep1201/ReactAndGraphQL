// tslint:disable: deprecation
import { CareerAssessment } from './../models/career-assessment.model';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { AppService } from './../../../core/services/app.service';
import { AuthHttpClient } from './../../../core/interceptors/AuthHttpClient';
import { ActivatedRoute } from '@angular/router';
import { map, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class CareerAssessmentService {
  private careerAssessmentUrl: string;
  public modeForCareerAssessment = new BehaviorSubject<any>({ readOnly: false, inEditView: false });

  constructor(private route: ActivatedRoute, private http: AuthHttpClient, private appService: AppService) {
    this.careerAssessmentUrl = this.appService.apiServer + 'api/career-assessment/';
  }

  public getAllCareerAssessmentsForPin(pin: string) {
    const requestUrl = `${this.careerAssessmentUrl}${pin}`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractAllCareerAssessmentData(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  public getCareerAssessment(pin: string, id: number) {
    const requestUrl = `${this.careerAssessmentUrl}${pin}/${id}`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractCareerAssessment(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }
  public saveCareerAssessment(model: CareerAssessment, pin: string) {
    const requestUrl = `${this.careerAssessmentUrl}${pin}/save`;
    return this.http.post(requestUrl, model).pipe(
      map(res => res),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  private extractAllCareerAssessmentData(res: CareerAssessment[]) {
    const jsonObjs = res as CareerAssessment[];
    const objs: CareerAssessment[] = [];

    for (const obj of jsonObjs) {
      objs.push(new CareerAssessment().deserialize(obj));
    }

    return objs || [];
  }
  public extractCareerAssessment(res: CareerAssessment[]) {
    const body = res as CareerAssessment[];
    const careerAssessment = new CareerAssessment().deserialize(body);
    return careerAssessment || null;
  }

  private handleError(error: any): Observable<any> {
    if (error instanceof HttpErrorResponse) {
      return throwError(error);
    } else {
      // In a real world app, we might use a remote logging infrastructure
      // We'd also dig deeper into the error to get a better message
      const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      // console.error(errMsg); // log to console instead
      return throwError(errMsg);
    }
  }
}
