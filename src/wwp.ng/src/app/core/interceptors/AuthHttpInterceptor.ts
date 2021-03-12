import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Injectable, ComponentRef } from '@angular/core';
import { Observable, throwError, of, Subscription, Subject } from 'rxjs';

import { AuthHttpClient, IRequestOptions } from './AuthHttpClient';
import { Router } from '@angular/router';
import { catchError, take, flatMap } from 'rxjs/operators';
import { LoginDialogComponent } from 'src/app/shared/components/login-dialog/login-dialog.component';
import { ModalService } from '../modal';

@Injectable({
  providedIn: 'root'
})
export class AuthHttpInterceptor implements HttpInterceptor {
  private loginModalComponentObs: Observable<ComponentRef<LoginDialogComponent>>;
  private loginSummitedSub: Subscription;
  constructor(private authHttpClient: AuthHttpClient, private router: Router, private _modalService: ModalService) {}

  private openLoginDialog(): Subject<boolean> {
    const isAuthorizedSubject = new Subject<boolean>();
    if (!this.loginModalComponentObs) {
      this.loginModalComponentObs = this._modalService.create(LoginDialogComponent);
    }

    this.loginModalComponentObs.pipe(take(1)).subscribe(componentRef => {
      if (this.loginSummitedSub) {
        this.loginSummitedSub.unsubscribe();
      }
      this.loginSummitedSub = componentRef.instance.loginSubmitted.subscribe(authResult => {
        if (authResult.authorized) {
          isAuthorizedSubject.next(authResult.authorized);
          isAuthorizedSubject.complete();
        }
      });

      componentRef.onDestroy(() => {
        this.loginModalComponentObs = null;
        if (this.loginSummitedSub && !this.loginSummitedSub.closed) {
          this.loginSummitedSub.unsubscribe();
        }
      });
    });

    return isAuthorizedSubject;
  }

  private addHeader(request: HttpRequest<any>, jwt: string, tokenExpired: boolean) {
    if (jwt && !tokenExpired) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${jwt}`
        },
        withCredentials: false
      });
    }

    return request;
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwt = localStorage.getItem('token');
    let tokenExpired = false;

    if (!(request.url.indexOf('auth/status') > -1)) {
      tokenExpired = this.authHttpClient.requestWithToken(jwt);
    }

    request = this.addHeader(request, jwt, tokenExpired);

    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401) {
          return this.openLoginDialog().pipe(
            flatMap(res => {
              if (res) {
                request = this.addHeader(request, jwt, tokenExpired);
                const options = {
                  headers: request.headers,
                  body: request.body,
                  responseType: request.responseType,
                  withCredentials: request.withCredentials
                } as IRequestOptions;
                return this.authHttpClient.request(request.method, request.url, options);
              } else return of(null);
            })
          );
        } else {
          return throwError(err);
        }
      })
    );
  }
}
