import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, NavigationExtras, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';

import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../models/authorization';

@Injectable()
export class TimeLimitsAppGuard implements CanActivate {
  constructor(private router: Router, private appService: AppService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // The basic rules for accessing TimeLimits are based upon the timeLimitsView
    // authorization.  All other logic will be handled on the component level.
    const isAuth = this.appService.isUserAuthorized(Authorization.timeLimitsView, null);

    // console.log(this.appService.user.authorizations);

    if (!isAuth) {
      // Prepare debugging information for logging
      let authorizationsString: string;
      if (this.appService.user) authorizationsString = JSON.stringify(this.appService.user.authorizations);
      else authorizationsString = '';
      const navigationExtras: NavigationExtras = {
        queryParams: {
          url: state.url,
          guard: 'time-limits-app-guard'
        }
      };

      this.router.navigate(['/unauthorized'], navigationExtras);
    }

    return of(isAuth);
  }
}
