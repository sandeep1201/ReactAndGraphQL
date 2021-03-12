import { HttpClient, HttpHeaders, HttpParams, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable, ComponentRef } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as moment from 'moment';

import { JwtAuthConfig } from '../jwt-auth-config';
import { LoginDialogComponent } from '../../shared/components/login-dialog/login-dialog.component';
import { ModalService } from '../modal';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';

export interface IRequestOptions {
  headers?: HttpHeaders;
  observe?: 'body';
  params?: HttpParams;
  reportProgress?: boolean;
  responseType?: any;
  withCredentials?: boolean;
  body?: any;
  observeResponse?: 'response';
}

// export function applicationHttpClientCreator(http: HttpClient,jwtAuthConfig: JwtAuthConfig, modalService: ModalService) {
//   return new AuthHttpClient(http,jwtAuthConfig,modalService);
// }

@Injectable()
export class AuthHttpClient {
  private config: IRequestOptions;
  public jwtConfig: JwtAuthConfig;
  private jwtHelper: JwtHelperService;
  //private api = 'https://someurl.example';

  set lastRequest(value: moment.Moment) {
    localStorage.setItem('lr', value.format('YYYYMMDDHHmmss'));
  }

  get lastRequest() {
    const lastRequestString = localStorage.getItem('lr');
    if (lastRequestString != null && lastRequestString.trim() !== '') {
      return moment(lastRequestString, 'YYYYMMDDHHmmss', true).clone();
    } else {
      // If we cant read lastRequestString, lets set it to way in the past.
      console.warn('Missing LR');
      return moment('20001210150807', 'YYYYMMDDHHmmss', true).clone();
    }
  }
  // 1200 is 20mins.
  private readonly MAX_SECONDS_OF_INACTIVITY = 1200;

  private loginModalComponentObs: Observable<ComponentRef<LoginDialogComponent>>;
  private loginSummitedSub: Subscription;

  // Extending the HttpClient through the Angular DI.
  public constructor(public http: HttpClient, private authConfigOptions: JwtAuthConfig, private modalService: ModalService) {
    // If you don't want to use the extended versions in some cases you can access the public property and use the original one.
    // for ex. this.httpClient.http.get(...)
    //this.jwtConfig = authConfigOptions;
    //this.config = authConfigOptions;
    // this.config.noJwtError = true; // supress JWT client side errors and try the request anyway
    this.jwtHelper = new JwtHelperService();
  }

  /**
   * Indicates if the authenticated user is inactive.
   *
   * @returns {boolean}
   * @memberof AuthHttpClient
   */
  public isUserInactive(): boolean {
    const now = moment();

    if (this.lastRequest == null) {
      // We can use the logic above because we know the token will already be checked and must be
      // valid and non-expired.  So, if we get here it means we don't have the last request in
      // memory so we'll check local storage.
      const lr = localStorage.getItem('lr');

      if (lr == null || lr.length === 0) {
        return true;
      }
    }

    // We'll get the duration in seconds in case we want to do some testing and use a smaller
    // time duration.
    const duration = moment.duration(now.diff(this.lastRequest)).asSeconds();

    // Do a quick sanity check here to make sure the duration hasn't been tampered with
    // by checking that it's not less than 0 (meaning the previous value is in the future)
    // as well as that the duration is within 20 minutes from now.  (20 * 60) == 1200
    if (duration < 0 || duration > this.MAX_SECONDS_OF_INACTIVITY) {
      return true;
    }

    // Update the last request value in local storage.
    this.markUserAsActive(now);

    return false;
  }

  /**
   * Indicates if the number of seconds until the authenticated user is inactive.
   *
   * @returns {number}
   * @memberof AuthHttpClient
   */
  public getSecondsUntilInactive(): number {
    // If we don't have a valid lastRequest then that means the user is not
    // logged in, so indicate a negative value meaning they are inactive.
    if (this.lastRequest == null) {
      return -1;
    }

    // We'll get the duration in seconds since the last request.
    const now = moment();
    const duration = moment.duration(now.diff(this.lastRequest)).asSeconds();

    if (duration < 0 || duration > this.MAX_SECONDS_OF_INACTIVITY) {
      return -1;
    }

    return this.MAX_SECONDS_OF_INACTIVITY - duration;
  }

  /**
   * Marks user as active and updates the last request tracking variable.
   * @param [timestamp]
   */
  public markUserAsActive(timestamp?: moment.Moment) {
    if (timestamp === undefined) {
      timestamp = moment();
    }

    this.lastRequest = timestamp;
  }

  /**
   * Clears user active indicator, which is useful when the user logs out.
   */
  public clearUserActive() {
    localStorage.removeItem('lr');
  }

  private openLoginDialog(): Subject<Response> {
    const loginSubject: Subject<Response> = new Subject<Response>();

    if (!this.loginModalComponentObs) {
      this.loginModalComponentObs = this.modalService.create(LoginDialogComponent);
    }
    if (this.loginModalComponentObs) {
      this.loginModalComponentObs.pipe(take(1)).subscribe(componentRef => {
        if (this.loginSummitedSub) {
          this.loginSummitedSub.unsubscribe();
        }
        // this.loginSummitedSub = componentRef.instance.loginSubmitted.subscribe(authResult => {
        //   if (authResult.authorized) {
        //     this.lastRequest = moment();
        //     const opts = new ResponseOptions();
        //     opts.body = authResult;
        //     opts.status = 200;
        //     const response = new Response(opts);
        //     loginSubject.next(response);
        //     loginSubject.complete();
        //   }
        // });

        componentRef.onDestroy(() => {
          this.loginModalComponentObs = null;
          if (this.loginSummitedSub && !this.loginSummitedSub.closed) {
            this.loginSummitedSub.unsubscribe();
          }
        });
      });
    }
    return loginSubject;
  }

  public requestWithToken(token: string): boolean {
    // TODO: cache calculation so we don't do this on requests with the same token
    // trying to avoid user manipulation causing the app to crash
    let tokenExpired: boolean;
    try {
      tokenExpired = this.jwtHelper.isTokenExpired(token);
      if (tokenExpired === false) {
        // Check the timeout
        tokenExpired = this.isUserInactive();
      }
    } catch (e) {
      tokenExpired = true;
    }
    return tokenExpired;
  }

  // private requestWithToken(req: Request, token: string): Observable<Response> {
  //   // TODO: cache calculation so we don't do this on requests with the same token
  //   // trying to avoid user manipulation causing the app to crash
  //   let tokenExpired: boolean;
  //   try {
  //     tokenExpired = this.jwtHelper.isTokenExpired(token);
  //     if (tokenExpired === false) {
  //       // Check the timeout
  //       tokenExpired = this.isUserInactive();
  //     }
  //   } catch (e) {
  //     tokenExpired = true;
  //   }

  //   if (tokenExpired && !this.config.noClientCheck) {
  //     // note: We are ignoring the noJwtError config value because i don't see a use for throwing exceptions on the client.
  //     // if the token expired the client-side action should always be the same (try to refresh, open login dialog, etc)
  //     return this.openLoginDialog().flatMap(res => {
  //       const requestOpts = new RequestOptions();
  //       requestOpts.body = req.getBody();
  //       requestOpts.headers = req.headers;
  //       requestOpts.method = req.method;
  //       requestOpts.url = req.url;
  //       requestOpts.responseType = req.responseType;
  //       requestOpts.withCredentials = req.withCredentials;
  //       return this.request(new Request(requestOpts)); // Retry original request
  //     });
  //   } else {
  //     req.headers.set(this.config.headerName, this.config.headerPrefix + token);
  //   }
  //   return super.request(req);
  // }

  // public setGlobalHeaders(headers: Array<Object>, request: Request | RequestOptionsArgs) {
  //   if (!request.headers) {
  //     request.headers = new Headers();
  //   }
  //   headers.forEach((header: Object) => {
  //     const key: string = Object.keys(header)[0];
  //     const headerValue: string = (header as any)[key];
  //     (request.headers as Headers).set(key, headerValue);
  //   });
  // }

  // public request(url: string | Request, options?: RequestOptionsArgs): Observable<Response> {
  //   if (typeof url === 'string') {
  //     return this.get(url, options); // Recursion: transform url from String to Request
  //   }
  //   // else if ( ! url instanceof Request ) {
  //   //   throw new Error('First argument must be a url string or Request instance.');
  //   // }

  //   // from this point url is always an instance of Request;
  //   const req: Request = url as Request;
  //   if (req.withCredentials === false || (options && options.withCredentials === false)) {
  //     return super.request(req);
  //   } else {
  //     const token: string = this.jwtConfig.tokenGetter();
  //     return this.requestWithToken(req, token);
  //   }

  //   // if (token instanceof Promise) {
  //   //   return Observable.fromPromise(token).mergeMap((jwtToken: string) => this.requestWithToken(req, jwtToken));
  //   // } else {
  //   //   return this.requestWithToken(req, token);
  //   // }
  // }

  /**
   * Request Method
   * @param url
   * @param options
   */
  public request<T>(method: string, url: string, options?: IRequestOptions): Observable<HttpResponse<any>> {
    //options = Utilities.getApiAuthorizedRequestOptions();
    return this.http.request<any>(method, url, options);
  }

  /**
   * GET request
   * @param {string} endPoint it doesn't need / in front of the end point
   * @param {IRequestOptions} options options of the request like headers, body, etc.
   * @returns {Observable<T>}
   */
  public get<T>(url: string, options?: IRequestOptions): Observable<any> {
    //options = Utilities.getApiAuthorizedRequestOptions();
    return this.http.get<any>(url, options);
  }

  /**
   * GET request w/Response
   * @param {string} endPoint it doesn't need / in front of the end point
   * @returns {Observable<T>}
   */
  public getResponse<T>(url: string): Observable<HttpResponse<any>> {
    return this.http.get<any>(url, { observe: 'response' });
  }

  /**
   * POST request
   * @param {string} endPoint end point of the api
   * @param {Object} params body of the request.
   * @param {IRequestOptions} options options of the request like headers, body, etc.
   * @returns {Observable<T>}
   */
  public post<T>(url: string, params?: Object, options?: IRequestOptions): Observable<any> {
    let reqOptions = null;
    if (options) {
      reqOptions = options;
    } else {
      reqOptions = this.getApiAuthorizedRequestOptions();
    }
    return this.http.post<any>(url, params, reqOptions);
  }

  /**
   * PUT request
   * @param {string} endPoint end point of the api
   * @param {Object} params body of the request.
   * @param {IRequestOptions} options options of the request like headers, body, etc.
   * @returns {Observable<T>}
   */
  public put<T>(url: string, params?: Object): Observable<any> {
    const options = this.getApiAuthorizedRequestOptions();
    return this.http.put<any>(url, params, options);
  }

  /**
   * DELETE request
   * @param {string} endPoint end point of the api
   * @param {IRequestOptions} options options of the request like headers, body, etc.
   * @returns {Observable<T>}
   */
  public delete<T>(url: string): Observable<any> {
    const options = this.getApiAuthorizedRequestOptions();
    return this.http.delete<any>(url, options);
  }

  public deleteResponse<T>(url: string): Observable<HttpResponse<any>> {
    return this.http.delete<any>(url, { observe: 'response' });
  }

  public getApiAuthorizedRequestOptions(): IRequestOptions {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = {
      headers: headers,
      withCredentials: false
    } as IRequestOptions;

    return options;
  }

  public getApiAuthorizedResponseOptions(): IRequestOptions {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = {
      headers: headers,
      withCredentials: false,
      observeResponse: 'response'
    } as IRequestOptions;

    return options;
  }
}
