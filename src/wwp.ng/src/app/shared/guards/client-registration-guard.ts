import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationExtras, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../models/authorization';
import { ParticipantService } from '../services/participant.service';
import { CoreAccessContext } from '../models/core-access-context';
import { AccessType } from '../enums/access-type.enum';
import { flatMap } from 'rxjs/operators';

@Injectable()
export class ClientRegistrationGuard implements CanActivate {
  constructor(private router: Router, private appService: AppService, private participantService: ParticipantService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const pin = route.paramMap.get('pin');
    let canActivate;
    if (pin) {
      canActivate = this.participantService.getCachedParticipant(pin).pipe(flatMap(p => {
        if (p.hasBeenThroughClientReg === true) {
          if (this.appService.isUserAuthorizedToEditClientReg(p, true)) {
            return of(true);
          } else if (this.appService.isUserAuthorizedToViewClientReg(p)) {
            return of(true);
          } else {
            this.routeToUnauthorized(state.url);
            return of(false);
          }
        } else {
          const navigationExtras: NavigationExtras = {
            queryParams: {
              redirect: 'true'
            }
          };
          if (this.appService.isUserAuthorized(Authorization.canAccessClearance)) {
            this.router.navigate(['/clearance'], navigationExtras);
            return of(false);
          } else {
            this.router.navigate(['/pin/' + pin], navigationExtras);
            return of(false);
          }
        }
      }));
    }

    return canActivate;
  }

  routeToUnauthorized(url: string) {
    // Prepare debugging information for logging
    const authorizationsString = JSON.stringify(this.appService.user.authorizations);
    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: url,
        guard: 'client-registration-guard'
      }
    };

    this.router.navigate(['/unauthorized'], navigationExtras);
  }
}
