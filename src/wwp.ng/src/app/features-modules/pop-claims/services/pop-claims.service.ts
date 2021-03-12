import { POPClaimEmployment } from '../models/pop-claim-employment.model';
import { LogService } from 'src/app/shared/services/log.service';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { POPClaim } from '../models/pop-claim.model';
import { AppService } from 'src/app/core/services/app.service';
import { map, catchError } from 'rxjs/operators';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';

@Injectable({
  providedIn: 'root'
})
export class PopClaimsService extends BaseService {
  private popClaimUrl: string;

  constructor(http: AuthHttpClient, private appService: AppService, logService: LogService) {
    super(http, logService);
    this.popClaimUrl = `${this.appService.apiServer}api/pop-claim`;
  }

  getPopClaims(participantId?: number) {
    const url = !participantId ? `${this.popClaimUrl}/list` : `${this.popClaimUrl}/list/${participantId}`;
    return this.http.get(`${url}`).pipe(map(this.extractPopClaimData), catchError(this.handleError));
  }

  getPopClaimsBasedOnStatusesAndAgencyCode(statuses: string[], agencyCode: string) {
    const url = `${this.popClaimUrl}/list/${agencyCode}`;
    const body = JSON.stringify(statuses);
    return this.http.post(`${url}`, body).pipe(map(this.extractPopClaimData), catchError(this.handleError));
  }
  getEmployments(pin: number, popClaimId: number) {
    return this.http.get(`${this.popClaimUrl}/${pin}/pop-claim-employments/${popClaimId}`).pipe(map(this.extractEmploymentsData), catchError(this.handleError));
  }

  savePOP(pin: string, popId: number, popClaim: POPClaim) {
    const body = JSON.stringify(popClaim);

    return this.http.post(`${this.popClaimUrl}/${pin}`, body).pipe(catchError(this.handleError));
  }
  preAdd(pin: string, popId: number, popClaim: POPClaim) {
    const body = JSON.stringify(popClaim);
    return this.http.post(`${this.popClaimUrl}/${pin}/preAdd/${popId}`, body).pipe(map(this.extractRuleReasonData), catchError(this.handleError));
  }

  public extractEmploymentsData(res) {
    const jsonObjs = res as POPClaimEmployment[];
    const objs: POPClaimEmployment[] = [];
    for (const obj of jsonObjs) {
      objs.push(new POPClaimEmployment().deserialize(obj));
    }
    return objs || [];
  }
  public extractPopClaimData(res) {
    const jsonObjs = res as POPClaim[];
    const objs: POPClaim[] = [];
    for (const obj of jsonObjs) {
      objs.push(new POPClaim().deserialize(obj));
    }
    return objs || [];
  }
}
