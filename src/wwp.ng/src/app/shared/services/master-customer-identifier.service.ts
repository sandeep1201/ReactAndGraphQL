import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { MciParticipantReturn } from '../models/mciParticipantReturn.model';
import { MciParticipantSearch } from '../models/mciParticipantSearch.model';
import { Utilities } from '../utilities';

@Injectable()
export class MasterCustomerIdentifierService {
  private mciUrl: string;
  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.mciUrl = this.appService.apiServer + 'api/mci/search';
  }

  public getSearchResults(model: MciParticipantSearch) {
    const body = JSON.stringify(model);

    return this.http.post(this.mciUrl, body).pipe(map(this.extractMciParticipantssData), catchError(this.handleError));
  }

  private extractMciParticipantssData(res: MciParticipantReturn[]): MciParticipantReturn[] {
    const jsonObjs = res as MciParticipantReturn[];
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
