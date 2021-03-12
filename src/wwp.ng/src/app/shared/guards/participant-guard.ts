import { Participant } from 'src/app/shared/models/participant';
import { EmergencyAssistanceService } from './../../features-modules/emergency-assistance/services/emergancy-assistance.service';
import { Injectable, ComponentRef } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, NavigationExtras } from '@angular/router';
import { Observable, Subject, of, forkJoin } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../../shared/models/authorization';
import { ModalService } from 'src/app/core/modal/modal.service';
import { RestrictedAccessDialogComponent } from '../components/restricted-access-dialog/restricted-access-dialog.component';
import { RequestRestrictedAccessDialogComponent } from '../components/request-restricted-access-dialog/request-restricted-access-dialog.component';
import { ParticipantService } from '../services/participant.service';
import { take, map, flatMap, mergeMap, concatMap } from 'rxjs/operators';

@Injectable()
export class ParticipantGuard implements CanActivate {
  get canRequestRestrictedAccess(): boolean {
    return this.appService.isUserAuthorized(Authorization.canRequestRestrictedAccess, null);
  }
  get canAccessPB(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessPB, null);
  }
  get canAccessPHI(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessPHI, null);
  }
  get isStateStaff(): boolean {
    return this.appService.isUserAuthorized(Authorization.isStateStaff, null);
  }

  private restrictedAccessModalComponentObs: Observable<ComponentRef<RestrictedAccessDialogComponent>>;
  private requestResctrictedAccessModalComponentObs: Observable<ComponentRef<RequestRestrictedAccessDialogComponent>>;

  constructor(
    private router: Router,
    private appService: AppService,
    private participantService: ParticipantService,
    private modalService: ModalService,
    private eaService: EmergencyAssistanceService
  ) {}

  private openRestrictedDialog(): Subject<boolean> {
    const hasAccessSubject = new Subject<boolean>();

    if (!this.restrictedAccessModalComponentObs) {
      this.restrictedAccessModalComponentObs = this.modalService.create(RestrictedAccessDialogComponent);
    }

    this.restrictedAccessModalComponentObs.pipe(take(1)).subscribe(componentRef => {
      componentRef.instance.actionSubmitted.subscribe(result => {
        hasAccessSubject.next(result.canAccess);
        hasAccessSubject.complete();
      });

      componentRef.onDestroy(() => {
        this.restrictedAccessModalComponentObs = null;
      });
    });

    return hasAccessSubject;
  }

  private openRequestRestrictedDialog(): Subject<any> {
    const isRequestingAccessSubject = new Subject<object>();
    if (!this.requestResctrictedAccessModalComponentObs) {
      this.requestResctrictedAccessModalComponentObs = this.modalService.create(RequestRestrictedAccessDialogComponent);
    }
    this.requestResctrictedAccessModalComponentObs.subscribe(componentRef => {
      this.participantService.actionSubmitted.subscribe(result => {
        isRequestingAccessSubject.next(result);
        isRequestingAccessSubject.complete();
      });
      componentRef.onDestroy(() => {
        this.requestResctrictedAccessModalComponentObs = null;
      });
    });
    return isRequestingAccessSubject;
  }

  public requestingElevatedAccess(p: any, sectionRequestingAccessTo?: string, pin?: string): Observable<boolean> {
    return this.openRequestRestrictedDialog().pipe(
      mergeMap(as => {
        if (as.isRequestingAccess) {
          return this.appService.requestRestrictedAccess(as.model, p.pin).pipe(
            flatMap(res => {
              //if (res) {
              // TODO: Remove this once the JWT is used for tracking what is the current
              // elevated pin.
              this.appService.user.elevatedAccessPin = p.pin;
              if (sectionRequestingAccessTo === 'PBSection') {
                this.appService.PBSection.next({ hasPBAccessBol: true, canRequestPBAccess: false, requestedElevatedAccess: true });
              }
              if (sectionRequestingAccessTo === 'FBSection') {
                this.appService.FBSection.next({ hasFBAccessBol: true, canRequestFBAccess: false, requestedElevatedAccess: true });
              }
              return of(true);
            })
          );
        } else {
          // for now we are redirecting to the home page but this needs to be sent to a previous route
          if (sectionRequestingAccessTo === 'clearance' || sectionRequestingAccessTo === 'EAHistory') {
            this.router.navigate(['/home']);
            return of(false);
          } else if (this.router.url.indexOf('assessment/edit') > -1) {
            const url = `/pin/${pin}/assessment/edit`;
            this.router.navigate([url]);
            return of(false);
          } else {
            this.router.navigate(['/home']);
            return of(false);
          }
        }
      })
    );
  }

  public restrictAccess(): Observable<boolean> {
    return this.openRestrictedDialog().pipe(
      map(acs => {
        // If we get here (as of Sprint 35), it means the user doesn't have access so
        // we will always redirect.
        this.appService.isUrlChangeBlocked = false;
        this.router.navigate(['/home']);
        return acs;
      })
    );
  }

  private routeAuthorized(canAccess: boolean, route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    // If we get here it means the user is logged in but is not authorized for the page.
    // Re-route to unauthorized page.
    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: state.url,
        guard: 'participant-guard'
      }
    };
    this.router.navigate(['/unauthorized'], navigationExtras);
    return false;
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    let pin = route.params['pin'];

    if (!pin) {
      pin = route.parent.params.pin;
    }
    const isFromPartSumGuard = route.data && route.data.isFromPartSumGuard ? route.data.isFromPartSumGuard : false;

    const hasAccess = this.participantService.getCachedParticipant(pin, isFromPartSumGuard).pipe(
      concatMap(p => {
        const eaRequest = p && p.isConfidentialCase && this.appService.isUserEAWorker() ? this.eaService.getEARequestList(pin) : of(null);

        return forkJoin(of(p), eaRequest).pipe(
          flatMap(res => {
            if (res[0] && res[0].isConfidentialCase) {
              if (res[0].hasConfidentialAccess || res[0].pin === this.appService.user.elevatedAccessPin) {
                return of(true);
              } else {
                // Also adding a check to see if the user is accessing the pin with in the agency.
                if (this.canRequestElevatedAccess(res)) {
                  return this.requestingElevatedAccess(res[0]);
                } else {
                  return this.restrictAccess();
                }
              }
            } else {
              return of(true);
            }
          })
        );
      })
    );

    return hasAccess;
  }

  canRequestElevatedAccess(res) {
    return (
      (this.canRequestRestrictedAccess && this.appService.isUserEASupervisor()) ||
      (this.canRequestRestrictedAccess &&
        this.appService.isUserEAWorker() &&
        ((res[0].getMostRecentW2Program() && res[0].getMostRecentW2Program().agencyCode === this.appService.user.agencyCode) ||
          (res[1] && res[1].length > 0 && res[1][0].organizationCode === this.appService.user.agencyCode))) ||
      (this.canRequestRestrictedAccess && res[0].getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService).length > 0) ||
      (this.canRequestRestrictedAccess && (this.isStateStaff || this.appService.user.isTribalUser || this.appService.isUserInAssociatedAgency(res[0])))
    );
  }

  showRequestElevatedAccess(pinNumber: number): Observable<boolean> {
    const pin = pinNumber.toString();
    const deactivate = this.participantService.getCachedParticipant(pin).pipe(
      flatMap(p => {
        if (p && p.isConfidentialCase) {
          if (p.hasConfidentialAccess || p.pin === this.appService.user.elevatedAccessPin) {
            return of(true);
          } else {
            return this.requestingElevatedAccess(p, 'clearance');
          }
        } else {
          return of(true);
        }
      })
    );
    return deactivate;
  }

  showModel(pin?: string): Observable<boolean> {
    const segments = this.router.url.split('/') || null;

    if (segments[2]) {
      pin = segments[2];
    }

    const showPB = this.participantService.getCachedParticipant(pin).pipe(
      flatMap(p => {
        if (p && p.pin === this.appService.user.elevatedAccessPin && !this.appService.user.isTribalUser) {
          return of(true); /* Also adding a check to see if the user is accessing the pin with in the agency.*/
        } else if (
          (this.canAccessPB &&
            this.canRequestRestrictedAccess &&
            (p.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService).length > 0 || this.appService.isUserInAssociatedAgency(p))) ||
          (this.canAccessPB && (this.isStateStaff || this.appService.user.isTribalUser))
        ) {
          return this.requestingElevatedAccess(p, 'PBSection', pin);
        } else {
          return this.restrictAccess();
        }
      })
    );
    return showPB;
  }

  showModelsForPHI(pin?: string): Observable<boolean> {
    const segments = this.router.url.split('/') || null;
    if (segments[2]) {
      pin = segments[2];
    }
    const showPHI = this.participantService.getCachedParticipant(pin).pipe(
      flatMap(p => {
        if (p && p.pin === this.appService.user.elevatedAccessPin) {
          return of(true);
        } else if (
          (this.canAccessPHI &&
            this.canRequestRestrictedAccess &&
            (p.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService).length > 0 || this.appService.isUserInAssociatedAgency(p))) ||
          (this.canAccessPHI && this.isStateStaff)
        ) {
          return this.requestingElevatedAccess(p, 'FBSection', pin);
        } else {
          return this.restrictAccess();
        }
      })
    );
    return showPHI;
  }

  showElevatedAccessModelForEA(pin: string, participant: Participant): Observable<boolean> {
    const showEA = of(participant).pipe(
      flatMap(p => {
        if (p.pin === this.appService.user.elevatedAccessPin) return of(true);
        else if (
          this.canRequestRestrictedAccess &&
          (this.appService.isUserEASupervisor() ||
            this.isStateStaff ||
            (this.appService.isUserEAWorker() && p.eaRequests && p.eaRequests.length > 0 && p.eaRequests[0].organizationCode === this.appService.user.agencyCode))
        ) {
          return this.requestingElevatedAccess(p, 'EAHistory', pin);
        } else {
          return this.restrictAccess();
        }
      })
    );

    return showEA;
  }
}
