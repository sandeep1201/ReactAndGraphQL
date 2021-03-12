import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationExtras, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { of } from 'rxjs';

import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../models/authorization';
import { ParticipantService } from '../services/participant.service';
import { CoreAccessContext } from '../models/core-access-context';
import { AccessType } from '../enums/access-type.enum';
import { environment } from '../../../environments/environment';
import { flatMap } from 'rxjs/operators';

@Injectable()
export class CoreAccessGuard implements CanActivate {
  public showUnauthorizedSecurityDetails = environment.showUnauthorizedSecurityDetails;

  constructor(private router: Router, private appService: AppService, private participantService: ParticipantService) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    this.appService.coreAccessContext = new CoreAccessContext();
    let pin = route.paramMap.get('pin');
    if (!pin) {
      pin = route.parent.paramMap.get('pin');
    }
    const id = route.paramMap.get('id');
    let canActivate;
    if (pin && this.pinBypass(state.url, pin, id)) {
      canActivate = this.participantService.getCachedParticipant(pin).pipe(
        flatMap(p => {
          this.appService.coreAccessContext.programs = p.programs;
          if (p.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService).length > 0) {
            this.appService.coreAccessContext.isMostRecentProgramInOrg = true;
          } else {
            this.appService.coreAccessContext.isMostRecentProgramInOrg = false;
          }
          if (p.fcdpPrograms.length > 0 && this.appService.isUserInAssociatedAgency(p)) {
            this.appService.coreAccessContext.isUserInAssociatedAgency = true;
          }
          this.appService.isMostRecentProgramInSisterOrg(p.programs);
          this.appService.coreAccessContext.canAccess = this.appService.isUserAuthorized(route.data.authorizations[0], null);
          this.appService.coreAccessContext.isStateStaff = this.appService.isUserAuthorized(Authorization.isStateStaff, null);
          this.appService.coreAccessContext.canEdit = this.appService.isUserAuthorizedForEdit(route.data.authorizations[0]);
          this.appService.coreAccessContext.isParticipantSummary = route.data.authorizations[0] === Authorization.canAccessParticipantSummary;
          this.appService.coreAccessContext.canAccessPOPClaims_View = route.data.authorizations[0] === Authorization.canAccessPOPClaims_View;
          return of(this.evaluateContextAndNavigate(state));
        })
      );
    } else {
      // We'll get here if we don't have a PIN. Hard coding context.isMostRecentProgramInOrg to true allows us to skip PIN specific context questions.
      canActivate = false;
      this.appService.coreAccessContext.isMostRecentProgramInOrg = true;
      this.appService.coreAccessContext.isMostRecentProgramInSisterOrg = false;
      this.appService.coreAccessContext.canAccess = this.appService.isUserAuthorized(route.data.authorizations[0], null);
      this.appService.coreAccessContext.isStateStaff = this.appService.isUserAuthorized(Authorization.isStateStaff, null);
      this.appService.coreAccessContext.canEdit = this.appService.isUserAuthorizedForEdit(route.data.authorizations[0]);
      this.appService.coreAccessContext.isParticipantSummary = route.data.authorizations[0] === Authorization.canAccessParticipantSummary;
      return of(this.evaluateContextAndNavigate(state));
    }
    return canActivate;
  }

  // This check bypasses isMostRecentProgramInOrg check for routes that have PINs but should be accessible regardless of program status
  pinBypass(url: string, pin?: string, id?: string): boolean {
    //TODO: we should find the better way to do this.
    const comapreUrls = [`/pin/${pin}/rfa`, `/pin/${pin}/rfa/${id}`, `/pin/${pin}/client-registration`, `/pin/${pin}/auxiliary`, `/pin/${pin}/drug-screening`];

    if (comapreUrls.includes(url) || url.indexOf('emergency-assistance') >= 0) return false;
    else return true;
  }

  evaluateContextAndNavigate(state: RouterStateSnapshot): boolean {
    const result: AccessType = this.appService.coreAccessContext.evaluate();

    // Prepare debugging information for logging

    const navigationExtras: NavigationExtras = {
      queryParams: {
        url: state.url,
        guard: 'core-access-guard'
      }
    };
    console.log(navigationExtras.queryParams);
    if (result === AccessType.none) {
      if (this.showUnauthorizedSecurityDetails === true) {
        this.router.navigate(['/unauthorized'], navigationExtras);
      } else {
        this.router.navigate(['/unauthorized']);
      }
      return false;
    } else if (result === AccessType.view) {
      return true;
    } else if (result === AccessType.edit) {
      return true;
    } else {
      return false;
    }
  }
}
