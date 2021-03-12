import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map, catchError, filter } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { MciParticipantReturn } from '../models/mciParticipantReturn.model';
import { MciParticipantSearch } from '../models/mciParticipantSearch.model';
import { Status } from '../models/status.model';
import { Utilities } from '../utilities';
import { ClientRegistration } from '../models/client-registration.model';
import { FinalistAddress } from '../models/finalist-address.model';

@Injectable()
export class ClientRegistrationService {
  private baseUrl: string;
  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.baseUrl = this.appService.apiServer + 'api/client-registration';
  }

  public getClientRegistration(mciParticipant: MciParticipantReturn) {
    const body = JSON.stringify(mciParticipant);

    return this.http.post(this.baseUrl + '/' + mciParticipant.mciId, body).pipe(
      map(res => this.extractData(res)),
      catchError(this.handleError)
    );
  }

  public getFinalistAddress(model: FinalistAddress, pin) {
    const body = JSON.stringify(model);

    return this.http.post(this.baseUrl + '/' + pin + '/finalist', body).pipe(
      map(res => {
        return this.extractFinalistAddress(res);
      }),
      catchError(this.handleError)
    );
  }

  public saveClientRegistration(model: ClientRegistration) {
    const body = JSON.stringify(model);

    return this.http.post(this.baseUrl, body).pipe(
      map(res => {
        return this.extractStatusData(res);
      }),
      catchError(this.handleError)
    );
  }

  private extractData(res: ClientRegistration): ClientRegistration {
    const body = res as ClientRegistration;
    const obj = new ClientRegistration().deserialize(body);
    return obj || null;
  }

  private extractStatusData(res: Status): Status {
    const body = res as Status;
    const obj = new Status().deserialize(body);
    return obj || null;
  }

  private extractFinalistAddress(res: FinalistAddress) {
    const obj = new FinalistAddress().deserialize(res);
    return obj || null;
  }

  private handleError(error: any) {
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return Observable.throw(errMsg);
  }
}
