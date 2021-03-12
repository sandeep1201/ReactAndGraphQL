// tslint:disable: deprecation
import { error } from 'util';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthHttpClient } from '../interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { ErrorInfo, ErrorInfoContract } from 'src/app/shared/models/ErrorInfoContract';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';

@Injectable()
export abstract class BaseService {
  private pin: string;
  public apiServer: string;

  public isProdBuild: boolean;
  public showEnvLabel: boolean;
  public tableauUrl: string;
  public tableauSiteId: string;
  constructor(protected http: AuthHttpClient, protected logService: LogService) {
    this.apiServer = environment.apiServer;
    this.isProdBuild = environment.production;
    this.showEnvLabel = environment.showEnvLabel;
    this.tableauUrl = environment.tableauUrl;
    this.tableauSiteId = environment.tableauSiteId;
  }

  setPin(pin: string) {
    this.pin = pin;
  }

  getPin(): string {
    return this.pin;
  }

  protected handleError(response: Error | Response) {
    let errorInfo = new ErrorInfo();
    if (response instanceof Error) {
      errorInfo.message = response.message;
      errorInfo.details = response.name + '.' + 'STACK TRACE: ' + response.stack;
    } else {
      try {
        if (response instanceof Response) {
          errorInfo.code = response.status;
        } else {
          const body = response;
          const contract = Object.assign(new ErrorInfoContract(), body);
          errorInfo = ErrorInfo.deserialize(contract);
        }
      } catch (e) {
        //uh oh.
        errorInfo.message = 'Something unexpected happend';
        errorInfo.details = 'Unable to parse error response from server.';
      }
    }
    //TODO: Log error back to server if not from server

    return throwError(errorInfo);
  }

  // protected handleError(error: any) {
  //   // In a real world app, we might use a remote logging infrastructure
  //   // We'd also dig deeper into the error to get a better message
  //   let errMsg = (error.message) ? error.message :
  //     error.status ? `${error.status} - ${error.statusText}` : 'Server error';
  //   console.error(errMsg); // log to console instead
  //   return Observable.throw(errMsg);
  // }

  private extractErrorInfo(res: Response): ErrorInfo {
    const body = res.json();
    return body || {};
  }
  protected extractRuleReasonData(res: WhyReason) {
    const body = res as WhyReason;
    const whyReason = new WhyReason().deserialize(body);
    return whyReason || null;
  }
}
