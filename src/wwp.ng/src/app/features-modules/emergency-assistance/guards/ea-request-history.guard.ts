import { AppService } from 'src/app/core/services/app.service';
import { ParticipantGuard } from './../../../shared/guards/participant-guard';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { EmergencyAssistanceService } from '../services/emergancy-assistance.service';
import { map, concatMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class EmergenyAssistanceRequestHistoryGuard implements CanActivate {
  constructor(private appService: AppService, private eaService: EmergencyAssistanceService, private participantGuard: ParticipantGuard) {}

  canActivate(route: ActivatedRouteSnapshot) {
    let pin = route.parent.params['pin'];

    if (!pin) {
      pin = route.params['pin'];
    }

    this.appService.loadingComponentInput.next({ isLoaded: false, loadingLabel: 'Emergency Assistance' });

    const hasAccess = this.eaService.getEAGroupConfidentialParticipant(pin).pipe(
      concatMap(participant => {
        this.appService.loadingComponentInput.next({ isLoaded: true, loadingLabel: '' });
        this.appService.loadingComponentInput.complete();
        if (participant && participant.isConfidentialCase) {
          return this.participantGuard.showElevatedAccessModelForEA(pin, participant).pipe(map(r => r));
        } else {
          return of(true);
        }
      })
    );

    return hasAccess;
  }
}
