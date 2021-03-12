import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable, throwError } from 'rxjs';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class HelpService {
  private helpUrl: string;

  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.helpUrl = this.appService.apiServer + 'api/help/';
  }

  getHelpUrl(featureName: string): Observable<string> {
    return this.http.get(this.helpUrl + featureName).pipe(map(this.extractData), catchError(this.handleError));
  }

  private extractData(res: Response) {
    let body = res as any;
    return body;
  }

  private handleError(error: any) {
    // In a real world app, we might use a remote logging infrastructure
    // We'd also dig deeper into the error to get a better message
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return throwError(errMsg);
  }
}
