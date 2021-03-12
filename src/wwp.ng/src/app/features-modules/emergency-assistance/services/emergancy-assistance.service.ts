import { Participant } from './../../../shared/models/participant';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { LogService } from 'src/app/shared/services/log.service';
import { catchError, map } from 'rxjs/operators';
import { EAPreviousGroupMembers } from '../models/ea-request-participant.model';
import { EARequest } from '../models';
import { EAIPV } from '../models/ea-ipv.model';
import { EAPayment } from '../models/ea-request-payment.model';

@Injectable({
  providedIn: 'root'
})
export class EmergencyAssistanceService extends BaseService {
  private eaUrl: string;
  public modeForEARequest = new BehaviorSubject<any>({ readOnly: false, isSearchMode: false, groupMemberMode: false, commentMode: false });
  public modeForIPVRequest = new BehaviorSubject<any>({ isReadoOnly: false, isInEditMode: false, ipvModel: null });
  public modeForEAPayment = new BehaviorSubject<any>({ readOnly: false, isInEditMode: false, data: null });

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.eaUrl = this.appService.apiServer + 'api/ea/';
  }

  public getEAGroupConfidentialParticipant(pin: string): Observable<Participant> {
    const url = `${this.eaUrl}ea-group-confidential-participant/${pin}/`;
    return this.http.get(url).pipe(map(this.extractParticipantData), catchError(this.handleError));
  }

  public getEARequest(pin: string, id: number): Observable<EARequest> {
    const url = `${this.eaUrl}request/${pin}/${id}`;
    return this.http.get(url).pipe(map(this.extractEAData), catchError(this.handleError));
  }

  public getEAPayment(id: string): Observable<EAPayment> {
    const url = `${this.eaUrl}payment/${id}`;
    return this.http.get(url).pipe(map(this.extractPaymentData), catchError(this.handleError));
  }

  public getEARequestList(pin: string): Observable<EARequest[]> {
    const url = `${this.eaUrl}request-list/${pin}`;
    return this.http.get(url).pipe(map(this.extractAllEAData), catchError(this.handleError));
  }

  public saveEAPayment(model: EAPayment, pin: string) {
    const requestUrl = `${this.eaUrl}post-payment/${pin}`;
    return this.http.post(requestUrl, model).pipe(
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  public getPerviousEAGroup(pin: string, id: number): Observable<EAPreviousGroupMembers[]> {
    const url = `${this.eaUrl}agMembers/${pin}/${id}`;
    return this.http.get(url).pipe(map(this.extractEAGroupData), catchError(this.handleError));
  }

  public searchParticipant(pin: string): Observable<EAPreviousGroupMembers> {
    const url = `${this.eaUrl}searchParticipant/${pin}`;
    return this.http.get(url).pipe(map(this.extractSearchData), catchError(this.handleError));
  }

  public getIPVList(pin: string): Observable<EAIPV[]> {
    const url = `${this.eaUrl}ipv-list/${pin}`;
    return this.http.get(url).pipe(map(this.extractEAIPVList), catchError(this.handleError));
  }

  public getIPV(id: number): Observable<EAIPV> {
    const url = `${this.eaUrl}ipv/${id}`;
    return this.http.get(url).pipe(map(this.extractEAIPV), catchError(this.handleError));
  }

  public saveIPV(model: EAIPV, pin: string) {
    const requestUrl = `${this.eaUrl}post-ipv/${pin}`;
    return this.http.post(requestUrl, model).pipe(
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  private extractParticipantData(res: Participant) {
    const body = res as Participant;

    if (body) {
      const participant = new Participant().deserialize(body);
      return participant || null;
    }

    return null;
  }

  private extractPaymentData(res: EAPayment): EAPayment {
    const body = res as EAPayment;
    const eaPayment = new EAPayment().deserialize(body);
    return eaPayment || null;
  }

  private extractEAData(res: EARequest): EARequest {
    const body = res as EARequest;
    const eaRequest = new EARequest().deserialize(body);
    return eaRequest || null;
  }

  private extractAllEAData(res: EARequest[]): EARequest[] {
    const jsonObjs = res as EARequest[];
    const objs: EARequest[] = [];
    for (const obj of jsonObjs) {
      objs.push(new EARequest().deserialize(obj));
    }
    return objs || [];
  }

  private extractEAGroupData(res: EAPreviousGroupMembers[]): EAPreviousGroupMembers[] {
    const jsonObjs = res as EAPreviousGroupMembers[];
    const objs: EAPreviousGroupMembers[] = [];
    for (const obj of jsonObjs) {
      objs.push(new EAPreviousGroupMembers().deserialize(obj));
    }
    return objs || [];
  }

  private extractSearchData(res: EAPreviousGroupMembers): EAPreviousGroupMembers {
    const body = res as EAPreviousGroupMembers;
    const eaRequest = res ? new EAPreviousGroupMembers().deserialize(body) : null;
    return eaRequest || null;
  }

  private extractEAIPVList(res: EAIPV[]): EAIPV[] {
    const jsonObjs = res as EAIPV[];
    const objs: EAIPV[] = [];
    for (const obj of jsonObjs) {
      objs.push(new EAIPV().deserialize(obj));
    }
    return objs || [];
  }

  private extractEAIPV(res: EAIPV): EAIPV {
    const body = res as EAIPV;
    const eaIpv = res ? new EAIPV().deserialize(body) : null;
    return eaIpv || null;
  }
}
