import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, catchError, filter } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { FinalistAddress } from '../models/finalist-address.model';

@Injectable()
export class FinalistAddressService {
  private baseUrl: string;
  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.baseUrl = this.appService.apiServer + 'api/finalist-address';
  }

  public getFinalistAddress(model: FinalistAddress) {
    const body = JSON.stringify(model);

    return this.http.post(this.baseUrl + '/finalist', body).pipe(
      map(res => {
        return this.extractFinalistAddress(res);
      }),
      catchError(this.handleError)
    );
  }

  private extractFinalistAddress(res: FinalistAddress) {
    const obj = new FinalistAddress().deserialize(res);
    console.log('res', res);
    console.log('obj', obj);
    return obj || null;
  }

  private handleError(error: any) {
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return Observable.throw(errMsg);
  }
}
