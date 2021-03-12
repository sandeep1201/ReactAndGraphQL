import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Injectable, ComponentRef } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Observable, Subscription, Subject, of } from 'rxjs';
import { take, map, flatMap } from 'rxjs/operators';

import { LoginDialogComponent } from '../components/login-dialog/login-dialog.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { AppService } from 'src/app/core/services/app.service';

@Injectable()
export class AuthGuard implements CanActivate {
  private loginModalComponentObs: Observable<ComponentRef<LoginDialogComponent>>;
  private loginSummitedSub: Subscription;

  constructor(private appService: AppService, private _modalService: ModalService, private authHttpClinet: AuthHttpClient) {}

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

  canActivate() {
    // If you want to avoid having to login, uncomment this line:
    // return Observable.of(true);
    const result = this.appService.isUserAuthenticated().pipe(
      flatMap(isAuth => {
        if (isAuth && !this.authHttpClinet.isUserInactive()) {
          return of(isAuth);
        } else {
          return this.openLoginDialog().pipe(
            map(loggedIn => {
              if (loggedIn) {
                return loggedIn;
              }
              return false;
            })
          );
        }
      })
    );

    return result;
  }
}
