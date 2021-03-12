import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs';

import { AppService } from './../../core/services/app.service';
import { MciParticipantReturn } from '../models/mciParticipantReturn.model';
import { MciParticipantSearch } from '../models/mciParticipantSearch.model';
import { Utilities } from '../utilities';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class ClientRegistrationService {
  private clientRegUrl: string;
  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.clientRegUrl = this.appService.apiServer + 'api/client-registration';
  }

  public getClientByMcId(mciId: string) {
    return this.http.get(this.clientRegUrl + '/' + mciId).pipe(
      map(response => this.extractClient(response)),
      catchError(this.handleError)
    );
  }

  private extractClient(res: Response): MciParticipantReturn[] {
    const jsonObjs = <MciParticipantReturn[]>res.json();
    const objs: MciParticipantReturn[] = [];

    for (const obj of jsonObjs) {
      objs.push(new MciParticipantReturn().deserialize(obj));
    }

    return objs || [];
  }

  private handleError(error: any) {
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return Observable.throw(errMsg);
  }
}
