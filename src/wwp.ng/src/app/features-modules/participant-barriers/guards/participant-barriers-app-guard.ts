import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ParticipantGuard } from '../../../shared/guards/participant-guard';
import { map, concatMap } from 'rxjs/operators';

@Injectable()
export class ParticipantBarriersAppGuard implements CanActivate {
  constructor(private appService: AppService, private participantService: ParticipantService, private participantGuard: ParticipantGuard) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let pin = route.parent.params['pin'];
    if (pin === undefined) {
      pin = route.params['pin'];
    }
    const hasAccess = this.participantService.getCachedParticipant(pin).pipe(
      concatMap(p => {
        if (
          (p && p.pin === this.appService.user.elevatedAccessPin && !this.appService.user.isTribalUser) ||
          (this.appService.hasPBAccess(p) && p.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService).length > 0)
        ) {
          return of(true);
        } else {
          return this.participantGuard.showModel(pin).pipe(map(r => r));
        }
      })
    );
    return hasAccess;
  }
}
