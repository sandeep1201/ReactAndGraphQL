import { ParticipantService } from './../../../shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, NavigationExtras, Router, RouterStateSnapshot } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { Authorization } from 'src/app/shared/models/authorization';

@Injectable()
export class TJTMJEmploymentVerificationGuard implements CanActivate {
  constructor(private router: Router, private appService: AppService, private partService: ParticipantService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let pin = route.parent.params['pin'];
    if (!pin) pin = route.params['pin'];

    const hasAccess = this.partService.getCachedParticipant(pin).pipe(
      concatMap(participant => {
        let activateGuard = false;
        if (
          participant &&
          this.appService.isUserAuthorized(Authorization.canAccessTJTMJEmploymentVerification_View) &&
          (((participant.enrolledTJProgram || participant.enrolledTmjProgram) && this.appService.isStateStaff) ||
            (this.appService.isUserAuthorized(Authorization.canAccessProgram_TJ) &&
              participant.enrolledTJProgram &&
              participant.enrolledTJProgram.agencyCode === this.appService.user.agencyCode) ||
            (this.appService.isUserAuthorized(Authorization.canAccessProgram_TMJ) &&
              participant.enrolledTmjProgram &&
              participant.enrolledTmjProgram.agencyCode === this.appService.user.agencyCode))
        ) {
          activateGuard = true;
        } else this.routeToUnauthorized(state.url);
        return of(activateGuard);
      })
    );

    return hasAccess;
  }

  routeToUnauthorized(url: string) {
    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: url,
        guard: 'tj-tmj-employment-verification-guard'
      }
    };
    this.router.navigate(['/unauthorized'], navigationExtras);
  }
}
