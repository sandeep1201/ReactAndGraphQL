import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';

import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { DropDownField } from '../models/dropdown-field';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class FeatureToggleService {
  public FeatureToggleUrl: string;

  constructor(private http: AuthHttpClient, private appService: AppService) {}

  getFeatureToggleDate(feature: string): Observable<DropDownField[]> {
    this.FeatureToggleUrl = `${this.appService.apiServer}api/fielddata/feature-value/${feature}`;
    return this.http.get(this.FeatureToggleUrl).pipe(map(this.extractData), catchError(this.handleError));
    //return null;
  }

  private extractData(res: Response) {
    let body = <DropDownField[]>res.json();
    return body;
  }

  private handleError(error: any) {
    // In a real world app, we might use a remote logging infrastructure
    // We'd also dig deeper into the error to get a better message
    let errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return Observable.throw(errMsg);
  }
}
