import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { AppService } from './../../core/services/app.service';
import { Injectable } from '@angular/core';
import { ErrorInfo, ErrorInfoContract } from '../models/ErrorInfoContract';
import { map, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class TableauService {
  public tableauURL: string;

  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.tableauURL = this.appService.apiServer + 'api/tableau/trustedticket';
  }

  getTicket() {
    const responseType = { responseType: 'text' as 'json' };

    return this.http.get(this.tableauURL, responseType).pipe(map(this.extractData), catchError(this.handleError));
  }
  private extractData(res: Response) {
    const body = res as any;

    return body || null;
  }
  protected handleError(response: Error | Response) {
    let errorInfo = new ErrorInfo();
    if (response instanceof Error) {
      errorInfo.message = response.message;
      errorInfo.details = response.name + '.' + 'STACK TRACE: ' + response.stack;
    } else {
      try {
        if (response instanceof Response) {
          const body = response.statusText;
          errorInfo.code = response.status;
        } else {
          let body = response;
          let contract = Object.assign(new ErrorInfoContract(), body);
          errorInfo = ErrorInfo.deserialize(contract);
        }
      } catch (e) {
        //uh oh.
        errorInfo.message = 'Something unexpected happpend';
        errorInfo.details = 'Unable to parse error response from server.';
      }
    }
    //TODO: Log error back to server if not from server

    return throwError(errorInfo);
  }
}
