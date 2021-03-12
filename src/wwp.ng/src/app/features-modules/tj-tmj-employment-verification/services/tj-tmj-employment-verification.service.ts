import { TJTMJEmploymentVerificationModel } from './../tj-tmj-employment-verification.model';
import { AppService } from 'src/app/core/services/app.service';
import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class TJTMJEmploymentVerificationService extends BaseService {
  private employmentVerificationUrl: string;
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.employmentVerificationUrl = this.appService.apiServer + 'api/employment-verification/';
  }

  public getEmploymentsByJobType(participantId: number, jobTypeId: number, enrollmentDate: string): Observable<TJTMJEmploymentVerificationModel[]> {
    const url = `${this.employmentVerificationUrl}${participantId}/${jobTypeId}/${enrollmentDate}`;
    return this.http.get(url).pipe(map(this.extractEmploymentsData), catchError(this.handleError));
  }

  saveEmploymentVerification(pin: string, verifiedEmployments: TJTMJEmploymentVerificationModel[]) {
    return this.http.post(`${this.employmentVerificationUrl}${pin}`, verifiedEmployments).pipe(catchError(this.handleError));
  }

  private extractEmploymentsData(res: TJTMJEmploymentVerificationModel[]): TJTMJEmploymentVerificationModel[] {
    const jsonObjs = res as TJTMJEmploymentVerificationModel[];
    const objs: TJTMJEmploymentVerificationModel[] = [];
    for (const obj of jsonObjs) {
      objs.push(new TJTMJEmploymentVerificationModel().deserialize(obj));
    }
    return objs || [];
  }
}
