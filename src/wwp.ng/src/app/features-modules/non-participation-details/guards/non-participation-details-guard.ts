import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationExtras, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../../../shared/models/authorization';
import { ParticipantService } from '../../../shared/services/participant.service';
import { map, take, concatMap } from 'rxjs/operators';

@Injectable()
export class NonParticipationDetailsGuard implements CanActivate {
  constructor(private router: Router, private appService: AppService, private participantService: ParticipantService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const pin = route.paramMap.get('pin');
    let canActivate = false; // initialize with a false value
    if (pin) {
      return this.participantService.getCachedParticipant(pin).pipe(
        take(1),
        concatMap(p => {
          //Check if role is DCF Staff - General/Monitoring/W2 Help desk
          const w2programs = p.getMostRecentW2Program();
          if (this.appService.isUserAuthorized(Authorization.isStateStaff, p)) {
            if (w2programs != null) {
              canActivate = true;
            }
            return of(canActivate);
          }
          //Check if role is W-2 specific
          else if (this.appService.isUserAuthorizedToViewNonParticipationDetails(p)) {
            //Fetch the details to check if user is ever enrolled to same agency
            return this.participantService.getPepsDataBasedonPinOrProgram(pin, 'WW');
          }
        }),
        map(peps => {
          if (peps && peps.length > 0) {
            //if any agency matches w/worker's agency, then user is eligible
            canActivate = peps.some(pep => pep.agencyCode === this.appService.user.agencyCode);
          }

          if (!canActivate) {
            this.routeToUnauthorized(state.url);
          }

          return canActivate;
        })
      );
    }
  }

  routeToUnauthorized(url: string) {
    // Prepare debugging information for logging
    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: url,
        guard: 'non-participation-details-guard'
      }
    };

    this.router.navigate(['/unauthorized'], navigationExtras);
  }
}
