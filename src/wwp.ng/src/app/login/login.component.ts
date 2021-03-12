// tslint:disable: deprecation
import { Component, Input, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { catchError, concatMap } from 'rxjs/operators';

import { LogService } from '../shared/services/log.service';

import { Login } from '../shared/models/events/login';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AppService } from '../core/services/app.service';
import { AuthHttpClient } from '../core/interceptors/AuthHttpClient';

declare var ga: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @Input()
  modal: Boolean = false;
  @Output()
  submitted = new EventEmitter<{ authorized: boolean; username: string }>();
  isLoggingIn = false;
  isUserAuthenticated = false;
  username: string;
  password: string;
  loginMessage = '';
  lostPasswordURL: string = environment.lostPasswordURL;
  requestIDURL: string = environment.requestIDURL;

  constructor(private router: Router, private appService: AppService, private logService: LogService, private authHttpClient: AuthHttpClient, private http: HttpClient) {}

  ngOnInit() {}

  submitLogin() {
    this.logService.timerStart('login');
    this.isLoggingIn = true;
    this.appService
      .authenticateUser(this.username, this.password)
      .pipe(
        concatMap(isAuth => {
          this.submitted.emit({ authorized: isAuth, username: this.username });

          const l = new Login();
          l.success = isAuth;
          l.user = this.username;

          if (isAuth) {
            this.logService.sessionUser(this.username);
            this.authHttpClient.markUserAsActive();
            if (!this.modal) {
              const redirect = this.appService.redirectUrl ? this.appService.redirectUrl : '/home';
              // Seems like the appService.redirectUrl should be cleared out?? Maybe not
              // needed, but we'll be safe.
              this.appService.redirectUrl = null;
              this.router.navigate([redirect]);
            }
            this.isLoggingIn = false;
            this.logService.timerEndEvent('login', l);
            return this.appService.getFeatureToggleValues();
          } else {
            this.loginMessage = this.appService.authStatus;
            l.err = this.loginMessage;
            this.password = '';
            this.isLoggingIn = false;
            this.logService.timerEndEvent('login', l);
            return of(null);
          }
        }),
        catchError(err => {
          this.loginMessage = err && err.message ? err.message : 'Unknown Login error';
          console.dir(err);
          this.logService.timerEndEvent('login', err);
          this.isLoggingIn = false;
          return of(false);
        })
      )
      .subscribe(res => {
        this.appService.featureToggles.next(res);
      });
  }

  keyDown(e: KeyboardEvent) {
    let allow = true;

    if (e.which === 32) allow = false;

    return allow;
  }
}
