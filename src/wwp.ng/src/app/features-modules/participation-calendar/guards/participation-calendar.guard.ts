import { Utilities } from './../../../shared/utilities';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationExtras, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../../../shared/models/authorization';
import { ParticipantService } from '../../../shared/services/participant.service';
import { map, take, concatMap } from 'rxjs/operators';
import { ParticipationTrackingService } from '../services/participation-tracking.service';
import * as moment from 'moment';
import { Participant } from 'src/app/shared/models/participant';
import * as _ from 'lodash';

@Injectable()
export class ParticipationCalendarGuard implements CanActivate {
  constructor(
    private router: Router,
    private appService: AppService,
    private participantService: ParticipantService,
    private participationTrackingService: ParticipationTrackingService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const pin = route.paramMap.get('pin');
    let canActivate = false;
    let participant: Participant;
    this.participationTrackingService.canEdit = false;
    return this.participantService.getCachedParticipant(pin).pipe(
      take(1),
      concatMap(res => {
        participant = res;
        const program: any = participant.getMostRecentW2Program();
        if (this.appService.isUserAuthorized(Authorization.isStateStaff, participant)) {
          if (program != null) {
            canActivate = true;
            return of(canActivate);
          } else {
            canActivate = false;
            return of(canActivate);
          }
        } else if (this.appService.isUserAuthorized(Authorization.canAccessParticipationCalendar_View, participant)) {
          return this.participantService.getPepsDataBasedonPinOrProgram(pin, 'WW');
        } else {
          return of(canActivate);
        }
      }),
      map(peps => {
        if (peps && peps.length > 0) {
          const sortedpepsToConsider = peps
            .filter(pep => pep.agencyCode === this.appService.user.agencyCode)
            .sort(function(a, b) {
              // Turn your strings into dates, and then subtract them
              // to get a value that is either negative, positive, or zero.
              return new Date(b.referralDate).getTime() - new Date(a.referralDate).getTime();
            });
          if (sortedpepsToConsider.length > 0) {
            if (!Utilities.stringIsNullOrWhiteSpace(sortedpepsToConsider[0].disenrollmentDate)) {
              const disEnrollmentDate = moment(sortedpepsToConsider[0].disenrollmentDate).toDate();
              this.participationTrackingService.viewDate.next({ viewDate: moment(new Date(disEnrollmentDate.getFullYear(), disEnrollmentDate.getMonth(), 16)).toDate() });
            }
            canActivate = true;
            this.participationTrackingService.canEdit = this.appService.isUserAuthorized(Authorization.canAccessParticipationCalendar_Edit, participant);
          } else {
            canActivate = false;
          }
        }

        if (!canActivate) {
          this.routeToUnauthorized(state.url);
        }
        return canActivate;
      })
    );
  }

  routeToUnauthorized(url: string) {
    // Prepare debugging information for logging
    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: url,
        guard: 'participation-calendar-guard'
      }
    };

    this.router.navigate(['/unauthorized'], navigationExtras);
  }
}
