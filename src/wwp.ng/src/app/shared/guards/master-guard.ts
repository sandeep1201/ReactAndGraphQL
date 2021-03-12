import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { ClientRegistrationGuard } from './client-registration-guard';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, ActivatedRoute } from '@angular/router';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from '../services/participant.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { from, of } from 'rxjs';
import { ParticipantGuard } from './participant-guard';
import { CoreAccessGuard } from './core-access-guard';
import { NonParticipationDetailsGuard } from '../../features-modules/non-participation-details/guards/non-participation-details-guard';
import { AuthGuard } from './auth-guard';
import { ParticipantBarriersAppGuard } from 'src/app/features-modules/participant-barriers/guards/participant-barriers-app-guard';
import { ParticipationCalendarGuard } from 'src/app/features-modules/participation-calendar/guards/participation-calendar.guard';
import { ParticipationTrackingService } from 'src/app/features-modules/participation-calendar/services/participation-tracking.service';
import { EmergencyAssistanceApplicationGuard } from 'src/app/features-modules/emergency-assistance/guards/ea-application.guard';
import { EmergencyAssistanceService } from 'src/app/features-modules/emergency-assistance/services/emergancy-assistance.service';
import { EmergenyAssistanceRequestHistoryGuard } from 'src/app/features-modules/emergency-assistance/guards/ea-request-history.guard';
import { TJTMJEmploymentVerificationGuard } from 'src/app/features-modules/tj-tmj-employment-verification/guard/tj-tmj-employment-verification.guard';

@Injectable()
export class MasterGuard implements CanActivate {
  //you may need to include dependencies of individual guards if specified in guard constructor
  constructor(
    private appService: AppService,
    private aRoute: ActivatedRoute,
    private modalService: ModalService,
    private partGuard: ParticipantGuard,
    private participantService: ParticipantService,
    private router: Router,
    private authHttpClient: AuthHttpClient,
    private participationTrackingService: ParticipationTrackingService,
    private emergencyAssistanceService: EmergencyAssistanceService
  ) {}

  private route: ActivatedRouteSnapshot;
  private state: RouterStateSnapshot;

  //This method gets triggered when the route is hit
  public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.route = route;
    this.state = state;

    if (!route.data) {
      return of(false);
    }

    //this.route.data.guards is an array of strings set in routing configuration
    if (!this.route.data.guards || !this.route.data.guards.length) {
      return of(false);
    }

    this.route.data.guards.unshift('AuthGuard');

    return from(this.executeGuards());
  }

  //Execute the guards sent in the route data
  private executeGuards(guardIndex: number = 0): Promise<boolean> {
    return this.activateGuard(this.route.data.guards[guardIndex])
      .toPromise()
      .then(() => {
        if (guardIndex < this.route.data.guards.length - 1) {
          return this.executeGuards(guardIndex + 1);
        } else {
          return Promise.resolve(true);
        }
      })
      .catch(() => {
        return Promise.reject(false);
      });
  }

  //Create an instance of the guard and fire canActivate method returning a promise
  private activateGuard(guardKey: string) {
    let guard:
      | AuthGuard
      | ClientRegistrationGuard
      | CoreAccessGuard
      | NonParticipationDetailsGuard
      | ParticipantBarriersAppGuard
      | ParticipantGuard
      | ParticipationCalendarGuard
      | EmergencyAssistanceApplicationGuard
      | EmergenyAssistanceRequestHistoryGuard
      | TJTMJEmploymentVerificationGuard;

    switch (guardKey) {
      case 'AuthGuard':
        guard = new AuthGuard(this.appService, this.modalService, this.authHttpClient);
        break;
      case 'ClientRegistrationGuard':
        guard = new ClientRegistrationGuard(this.router, this.appService, this.participantService);
        break;
      case 'CoreAccessGuard':
        guard = new CoreAccessGuard(this.router, this.appService, this.participantService);
        break;
      case 'NonParticipationDetailsGuard':
        guard = new NonParticipationDetailsGuard(this.router, this.appService, this.participantService);
        break;
      case 'ParticipationCalendarGuard':
        guard = new ParticipationCalendarGuard(this.router, this.appService, this.participantService, this.participationTrackingService);
        break;
      case 'ParticipantBarriersAppGuard':
        guard = new ParticipantBarriersAppGuard(this.appService, this.participantService, this.partGuard);
        break;
      case 'ParticipantGuard':
        guard = new ParticipantGuard(this.router, this.appService, this.participantService, this.modalService, this.emergencyAssistanceService);
        break;
      case 'EmergencyAssistanceGuard':
        guard = new EmergencyAssistanceApplicationGuard(this.router);
        break;
      case 'EmergenyAssistanceRequestHistoryGuard':
        guard = new EmergenyAssistanceRequestHistoryGuard(this.appService, this.emergencyAssistanceService, this.partGuard);
        break;
      case 'TJTMJEmploymentVerificationGuard':
        guard = new TJTMJEmploymentVerificationGuard(this.router, this.appService, this.participantService);
        break;
      default:
        break;
    }
    return guard.canActivate(this.route, this.state);
  }
}
