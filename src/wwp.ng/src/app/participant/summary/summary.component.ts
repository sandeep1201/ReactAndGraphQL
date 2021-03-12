import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable, forkJoin, throwError } from 'rxjs';
import { Participant, ParticipantDetails } from '../../shared/models/participant';
import { ParticipantService } from '../../shared/services/participant.service';
import { AppService } from './../../core/services/app.service';
import { Authorization } from '../../shared/models/authorization';
import { Utilities } from '../../shared/utilities';
import { EligibilityStatus } from '../../shared/enums/eligibility-status.enum';
import { take, concatMap } from 'rxjs/operators';
import * as moment from 'moment';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit, OnDestroy {
  public participant: Participant;
  public participantDetails: ParticipantDetails;
  private partSub: Subscription;
  private partDetailsSub: Subscription;

  pin: string;
  isLoaded = false;
  isLoadingCWW = false;
  isTjCollapsed = false;
  isTmjCollapsed = false;
  isCfCollapsed = false;
  isLfCollapsed = false;
  isW2Collapsed = false;

  isFcdpCollapsed = false;
  isOverviewCollapsed = false;
  canViewLF = false;
  canViewWW = false;
  canViewCfTmjTjFcdOverview = false;
  canViewLimitedLF = true;
  canViewLimitedWW = true;
  canViewOtherPrograms = true;
  canViewFCDP = false;
  displayOutOfSyncMessage = false;
  displayPhase2Messages = '';
  public redirect;
  showFcdpFeature: boolean;
  get isStateStaff(): boolean {
    return this.appService.isUserAuthorized(Authorization.isStateStaff, null);
  }

  constructor(private route: ActivatedRoute, private router: Router, private partService: ParticipantService, public appService: AppService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
      this.loadParticipant();
    });

    this.route.queryParams.subscribe(params => {
      if (params.redirect) {
        this.redirect = params.redirect;
      }
    });

    this.scrollToTop();
    this.showFcdpFeature = this.appService.getFeatureToggleDate('FCDP');
  }

  scrollToTop() {
    window.scroll(0, 0);
  }

  ngOnDestroy() {
    if (this.partSub) {
      this.partSub.unsubscribe();
    }
    if (this.partDetailsSub != null) {
      this.partDetailsSub.unsubscribe();
    }
  }

  loadParticipant() {
    this.partService
      .getParticipantSummaryDetails(this.pin, true)
      .pipe(
        take(1),
        concatMap(partD => {
          this.initDetailsParticipant(partD);

          return this.partService.isFromPartSumGuard && this.partService.isFromPartSumGuard.value === true
            ? this.partService.getCachedParticipant(this.pin)
            : this.partService.getParticipant(this.pin, true, true);
        })
      )
      .subscribe(part => {
        this.partService.isFromPartSumGuard.next(false);
        this.initParticipant(part);
      });
  }

  private initParticipant(part: Participant) {
    this.participant = part;
    this.getAccessToPsSections(this.participant);
    this.setWarningMessages();
  }

  private initDetailsParticipant(partDetails: ParticipantDetails) {
    this.participantDetails = partDetails;
    this.setEligibilityStatus();
    this.isLoadingCWW = false;
    this.isLoaded = true;
  }

  public setEligibilityStatus() {
    if (this.participantDetails) {
      if (this.participantDetails.w2EligibilityInfo.agStatuseCode === 'C') {
        this.participantDetails.w2EligibilityInfo.agStatuseCode = EligibilityStatus.c;
      }
      if (this.participantDetails.w2EligibilityInfo.agStatuseCode === 'O') {
        this.participantDetails.w2EligibilityInfo.agStatuseCode = EligibilityStatus.o;
      }
      if (this.participantDetails.w2EligibilityInfo.agStatuseCode === 'P') {
        this.participantDetails.w2EligibilityInfo.agStatuseCode = EligibilityStatus.p;
      }
    }
  }

  loadCWW() {
    this.isLoadingCWW = true;

    this.loadParticipant();
  }

  private setWarningMessages() {
    const programs = this.participant.programs;
    programs.forEach(program => {
      if ((program.isLF || program.isW2 || program.isTj || program.isTmj) && program.status.trim() === 'Enrolled') {
        if (program.isLF || program.isW2) {
          this.displayOutOfSyncMessage = this.participantDetails != null && this.participantDetails.officeTransferId != null && this.participantDetails.officeTransferId > 0;
        }

        this.displayPhase2Messages = moment(program.enrollmentDate).isSameOrAfter(this.appService.getFeatureToggleValue('EPGoLive'))
          ? 'No Phase 2 Cut-Over actions are needed for this participant.'
          : Utilities.isStringEmptyOrNull(this.participant.cutOverDate)
          ? 'Complete Phase 2 Cut-over for this participant by submitting a new EP in WWP.'
          : 'Phase 2 Cut-Over has been completed for this participant.';
      }
    });
  }
  toggleCollapse(e: string) {
    switch (e) {
      case 'Tmj':
        this.isTmjCollapsed = !this.isTmjCollapsed;
        break;
      case 'Tj':
        this.isTjCollapsed = !this.isTjCollapsed;
        break;
      case 'Cf':
        this.isCfCollapsed = !this.isCfCollapsed;
        break;
      case 'Lf':
        this.isLfCollapsed = !this.isLfCollapsed;
        break;
      case 'W2':
        this.isW2Collapsed = !this.isW2Collapsed;
        break;
      case 'Fcdp':
        this.isFcdpCollapsed = !this.isFcdpCollapsed;
        break;
      case 'Overview':
        this.isOverviewCollapsed = !this.isOverviewCollapsed;
        break;
    }
  }
  public isStringEmptyOrNull(str: string) {
    return Utilities.isStringEmptyOrNull(str);
  }
  public showEndDate(participant): boolean {
    if (this.participantDetails) {
      if (
        this.isStringEmptyOrNull(this.participantDetails.w2EligibilityInfo.agFailureReasonCode1) &&
        this.isStringEmptyOrNull(this.participantDetails.w2EligibilityInfo.agFailureReasonCode2) &&
        this.isStringEmptyOrNull(this.participantDetails.w2EligibilityInfo.agFailureReasonCode3)
      ) {
        return false;
      } else {
        return true;
      }
    }
  }

  public getAccessToPsSections(participant: Participant) {
    const programs = participant.programs;
    const programsUserHasAccessTo = participant.getMostRecentProgramsByAgency(this.appService.user.agencyCode);
    if (programsUserHasAccessTo.length > 0) {
      for (const prog of programsUserHasAccessTo) {
        if ((prog.isLF || prog.isWW) && this.appService.isUserAuthorized(Authorization.canAccessProgram_WW)) {
          this.canViewLF = true;
          this.canViewWW = true;
        } else {
          this.canViewLimitedWW = true;
          this.canViewLimitedLF = true;
        }
        if (prog.isTj || prog.isTmj || prog.isCF || prog.isFCDP) {
          this.canViewCfTmjTjFcdOverview = true;
        }
      }
    } else {
      this.canViewLimitedWW = true;
      this.canViewLimitedLF = true;
      this.canViewOtherPrograms = false;
      if (this.appService.isUserAuthorized(Authorization.canAccessProgram_FCD)) {
        this.canViewFCDP = true;
        this.canViewOtherPrograms = true;
      }
    }
    if (this.appService.isMostRecentProgramInSisterOrg(programs)) {
      this.canViewOtherPrograms = true;
      if (this.appService.isUserAuthorized(Authorization.canAccessProgram_WW, null)) {
        this.canViewWW = true;
      } else {
        this.canViewLimitedWW = true;
      }
      if (this.appService.isUserAuthorized(Authorization.canAccessProgram_LF, null)) {
        this.canViewLF = true;
      } else {
        this.canViewLimitedLF = true;
      }
      if (
        this.appService.isUserAuthorized(Authorization.canAccessProgram_CF, null) ||
        this.appService.isUserAuthorized(Authorization.canAccessProgram_TMJ, null) ||
        this.appService.isUserAuthorized(Authorization.canAccessProgram_FCD, null)
      ) {
        this.canViewCfTmjTjFcdOverview = true;
      }
    }
    if (programs.length > 0 && !this.canViewCfTmjTjFcdOverview) {
      if (
        this.appService.isUserAuthorized(Authorization.canAccessProgram_CF, null) ||
        this.appService.isUserAuthorized(Authorization.canAccessProgram_TMJ, null) ||
        this.appService.isUserAuthorized(Authorization.canAccessProgram_TJ, null) ||
        this.appService.isUserAuthorized(Authorization.canAccessProgram_FCD, null)
      ) {
        this.canViewCfTmjTjFcdOverview = true;
      }
    }
    if (programs.length > 1) {
      programs.forEach(program => {
        if (program.isCF || program.isW2) {
          // If CFMGR trying to access the PS when coenrolled in W2
          if (this.appService.isUserAuthorized(Authorization.canAccessProgram_CF, null)) {
            this.canViewWW = true;
          } else {
            this.canViewLimitedWW = true;
          }
          // if w2 roles trying to access the PS when co-enrolled in CF
          if (this.appService.isUserAuthorized(Authorization.canAccessProgram_WW, null) && program.isW2) {
            this.canViewCfTmjTjFcdOverview = true;
          }
        }
      });
    }
  }

  private handleError(error: any, caughtObs: Observable<any>) {
    return throwError(error);
  }
}
