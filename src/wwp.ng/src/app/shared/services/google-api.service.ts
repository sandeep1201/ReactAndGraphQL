import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { GoogleApi } from '../models/google-api';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';

@Injectable()
export class GoogleApiService {
  private addressesUrl: string;
  private usAddressesUrl: string;
  private citiesUrl: string;
  private uscitiesUrl: string;
  private wicitiesUrl: string;
  private collegesUrl: string;
  private schoolsUrl: string;
  private zipCodeUrl: string;

  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.addressesUrl = this.appService.apiServer + 'api/gapi/addresses/';
    this.usAddressesUrl = this.appService.apiServer + 'api/gapi/us-addresses/';
    this.collegesUrl = this.appService.apiServer + 'api/gapi/colleges/';
    this.citiesUrl = this.appService.apiServer + 'api/gapi/cities/';
    this.uscitiesUrl = this.appService.apiServer + 'api/gapi/uscities/';
    this.wicitiesUrl = this.appService.apiServer + 'api/gapi/wicities/';
    this.schoolsUrl = this.appService.apiServer + 'api/gapi/schools/';
    this.zipCodeUrl = this.appService.apiServer + 'api/gapi/zipCode/';
  }

  searchForColleges(search: string, placeId: string): Observable<GoogleApi> {
    return this.http.get(this.collegesUrl + placeId + '/' + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForCities(search: string): Observable<GoogleApi> {
    return this.http.get(this.citiesUrl + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForUsCities(search: string): Observable<GoogleApi> {
    return this.http.get(this.uscitiesUrl + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForWiCities(search: string): Observable<GoogleApi> {
    return this.http.get(this.wicitiesUrl + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForSchools(search: string, placeId: string): Observable<GoogleApi> {
    return this.http.get(this.schoolsUrl + placeId + '/' + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForAddresses(search: string, placeId: string): Observable<GoogleApi> {
    return this.http.get(this.addressesUrl + placeId + '/' + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  searchForUSAddresses(search: string, placeId: string): Observable<GoogleApi> {
    return this.http.get(this.usAddressesUrl + placeId + '/' + search).pipe(map(this.extractData), catchError(this.handleError));
  }

  getZipCodeByPlaceId(placeId: string): Observable<GoogleApi> {
    return this.http.get(this.zipCodeUrl + placeId).pipe(map(this.extractData), catchError(this.handleError));
  }

  private extractData(res: GoogleApi) {
    let body = res as GoogleApi;
    return body;
  }

  private handleError(error: any) {
    // In a real world app, we might use a remote logging infrastructure
    // We'd also dig deeper into the error to get a better message
    let errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    return Observable.throw(errMsg);
  }
}
