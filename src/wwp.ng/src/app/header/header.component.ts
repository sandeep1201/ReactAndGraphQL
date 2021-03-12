// tslint:disable: no-shadowed-variable
import { FeatureToggleService } from './../shared/services/feature-toggle.service';
import { Component, ComponentRef, HostListener, OnInit, ElementRef, OnDestroy } from '@angular/core';
import { LoginSimulationDialogComponent } from '../shared/components/login-simulation-dialog/login-simulation-dialog.component';
import { ContactInfoDialogComponent } from '../shared/components/contact-info-dialog/contact-info-dialog.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Router, ActivationEnd, ActivatedRoute } from '@angular/router';
import { interval } from 'rxjs';

import { Authorization } from '../shared/models/authorization';
import { HelpService } from '../shared/services/help.service';
import { version, build } from '../shared/version';
import { Utilities } from '../shared/utilities';
import { TimeoutDialogComponent } from '../shared/components/timeout-dialog/timeout-dialog.component';
import { DateChangerComponent } from '../shared/components/admin/date-changer/date-changer.component';
import { SystemClockService } from '../shared/services/system-clock.service';
import * as moment from 'moment';
import { environment } from '../../environments/environment';
import { timeInterval } from 'rxjs/operators';
import { AppService } from '../core/services/app.service';
import { AuthHttpClient } from '../core/interceptors/AuthHttpClient';
import { ParticipantService } from '../shared/services/participant.service';
import { Participant } from '../shared/models/participant';

declare var $: any;

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  providers: [HelpService]
})
export class HeaderComponent implements OnInit {
  public isWorkerToolsSubMenuDisplayed = false;
  public isCaseManagementSubMenuDisplayed = false;
  public isUserAccountSubMenuDisplayed = false;
  public isPTSubMenuDisplayed = false;
  public isPTDisplayed = false;
  public isEASubMenuDisplayed = false;

  public isAgencyToolsSubMenuDisplayed = false;
  public ver: string;
  public isPinBased = false;
  public logoPath = 'assets/img/logo.png';
  public participantPin: string;

  private tempModalRef: ComponentRef<LoginSimulationDialogComponent>;
  private contactInfoModalRef: ComponentRef<ContactInfoDialogComponent>;
  private simulateCurrentDateModalRef: ComponentRef<DateChangerComponent>;
  private timeoutModalRef: ComponentRef<TimeoutDialogComponent>;
  private readonly WARNING_SECONDS_BEFORE_TIMEOUT = 120;
  private isSimulatingCurrentDateEnabled = environment.isSimulatingCurrentDateEnabled;
  public showCareerAssessmentAndJobReadinessFeature = false;
  public showAuxiliary = false;
  public showPaymentDetails = false;
  public showPinCommentsFeature = false;
  public showOrganisationFeature = false;
  public showWorkerTaskFeature = false;
  public showContactsInfoFeature = false;
  public showDrugScreeningFeature = false;
  public pinCommentsFeatureToggleDate = moment();

  constructor(
    public appService: AppService,
    private router: Router,
    private route: ActivatedRoute,
    private authHttpClient: AuthHttpClient,
    private modalService: ModalService,
    private eRef: ElementRef,
    public helpService: HelpService,
    private partService: ParticipantService
  ) {
    this.ver = version.toString();

    // If we're including a build, then add it.
    if (build.length > 0) {
      this.ver = `${this.ver} (${build})`;
    }
    router.events.subscribe(val => {
      if (val instanceof ActivationEnd) {
        // Everytime the router changes, lets clear pin.
        this.participantPin = '';
        this.isPinBased = false;
        this.participantPin = val.snapshot.params['pin'];
        if (this.participantPin != null) {
          this.isPinBased = true;
        }
        this.closeSubMenu();
      }
    });
  }

  ngOnInit() {
    // TODO: Figure out why we cant use ActivatedRoute to query params on first load.
    let str = this.router.url.substring(this.router.url.indexOf('/pin/') + 5);
    if (!+str) {
      str = str.split('/')[0];
    }
    if (!Utilities.isStringEmptyOrNull(str)) {
      this.participantPin = str;
      this.isPinBased = true;
    }
    interval(1000 * 30)
      .pipe(timeInterval())
      .subscribe(x => {
        // This timer will verify if there is an auth user and then how many
        // seconds until they are considered inactive.  If that gets lower
        // than the threshold (currently set at 2 minutes), then the user
        // will get a warning.
        //
        // If that user doesn't renew their session and become active then
        // this code will also invoke the LOGOUT route.
        if (this.appService.isUserAuthenticatedCurrently()) {
          const seconds = this.authHttpClient.getSecondsUntilInactive();
          if (seconds < 0) {
            this.cleanupTimeoutWarning();
            // If we're not logged in correctly, lets do a hard logout. This forces a refresh of the app.
            // But no need to logout on login screen.
            if (this.router.isActive('login', false) === false) {
              if (this.appService.apiServer !== 'http://localhost:11001/') window.location.href = this.appService.apiServer + 'logout';
              else window.location.href = 'http://localhost:4200/logout';
            }
          } else if (seconds < this.WARNING_SECONDS_BEFORE_TIMEOUT) {
            this.showTimeoutWarning();
          }
        }
      });
  }
  @HostListener('document: click', ['$event.target'])
  onClick(target: HTMLElement) {
    if (this.eRef.nativeElement.contains(target)) {
      // Nothing for inside click.
    } else {
      this.closeSubMenu();
    }
  }

  get canSimulateUsers(): boolean {
    return this.appService.isUserAuthorized(Authorization.canSimulateLogins, null);
  }

  get isSimulatingUser(): boolean {
    return this.appService.isUserSimulated;
  }

  get isSimulatingCurrentDate(): boolean {
    return SystemClockService.isTimeSimulated;
  }

  get canSimulateCurrentDate(): boolean {
    return this.appService.isUserAuthorized(Authorization.canSimulateDateTime, null) && this.isSimulatingCurrentDateEnabled;
  }

  get canAccessOrganizationInformation(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessOrganizationInformation, null);
  }

  get canAccessContactInfo(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessContactInfo, null);
  }
  get canAccessProgram_WW(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessProgram_WW, null);
  }
  get isStateStaff(): boolean {
    return this.appService.isUserAuthorized(Authorization.isStateStaff, null);
  }
  get canAccessProgram_CF(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessProgram_CF, null);
  }
  get canAccessChildrenFirstTracking_View(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessChildrenFirstTracking_View, null);
  }
  get canAccessChildrenFirstTracking_Edit(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessChildrenFirstTracking_Edit, null);
  }
  get canShowPT(): boolean {
    return this.appService.getFeatureToggleDate('ParticipationTracking');
  }
  get canAccessParticipationCalendar_Edit(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessParticipationCalendar_Edit, null);
  }
  get canAccessParticipationCalendar_View(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessParticipationCalendar_View, null);
  }

  get canAccessDrugScreening_View(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessDrugScreening_View, null);
  }
  get canShowEA(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessEA_View, null) && this.appService.getFeatureToggleDate('EmergencyAssistance');
  }
  get canShowAgencyTools(): boolean {
    return this.appService.getFeatureToggleDate('AgencyTools');
  }
  get canShowApproverPOPClaims(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessApproverPOP_View, null) && this.appService.getFeatureToggleDate('POPClaims');
  }
  get canShowPOPClaims(): boolean {
    return (
      (this.appService.isUserAuthorized(Authorization.canAccessPOPClaims_View, null) || this.appService.isUserAuthorized(Authorization.canAccessAdjudicatorPOP_View, null)) &&
      this.appService.getFeatureToggleDate('POPClaims')
    );
  }

  get canShowAdjudicatorPOPClaims(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessAdjudicatorPOP_View, null) && this.appService.getFeatureToggleDate('POPClaims');
  }
  get canShowTransactions(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessTransactions_View, null) && this.appService.getFeatureToggleDate('Transactions');
  }
  get canShowEmploymentVerification(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessTJTMJEmploymentVerification_View, null) && this.appService.getFeatureToggleDate('TJTMJEmpVerification');
  }
  get canShowW2Plans(): boolean {
    return this.appService.getFeatureToggleDate('W2Plans');
  }

  get isHeaderShown(): boolean {
    let isHeaderShown = false;
    if (this.appService.user != null && !this.router.url.includes('login')) {
      isHeaderShown = true;
    }

    return isHeaderShown;
  }

  navigateByPage(page: string, noPin = false) {
    this.router.navigateByUrl(noPin ? `/${page}` : 'pin/' + this.participantPin + '/' + page);
    this.closeSubMenu();
  }

  goHome() {
    this.router.navigateByUrl('/home');
  }

  clickWorkerTools() {
    this.isWorkerToolsSubMenuDisplayed = !this.isWorkerToolsSubMenuDisplayed;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
    this.showOrganisationFeature = this.appService.getFeatureToggleDate('OrganizationInformation');
    if (!this.showAuxiliary) {
      this.showAuxiliary = this.appService.getFeatureToggleDate('Auxiliary');
    }
    this.showWorkerTaskFeature = this.appService.getFeatureToggleDate('WorkerTaskList');
  }

  clickCaseManagement() {
    this.isCaseManagementSubMenuDisplayed = !this.isCaseManagementSubMenuDisplayed;
    if (!this.showCareerAssessmentAndJobReadinessFeature) {
      this.showCareerAssessmentAndJobReadinessFeature = this.appService.getFeatureToggleDate('CareerAndJobReadiness');
    }
    if (!this.showPinCommentsFeature) {
      this.showPinCommentsFeature = this.appService.getFeatureToggleDate('PinComments');
    }
    if (!this.showAuxiliary) {
      this.showAuxiliary = this.appService.getFeatureToggleDate('Auxiliary');
    }
    if (!this.showPaymentDetails) {
      this.showPaymentDetails = this.appService.getFeatureToggleDate('PaymentDetails');
    }
    if (!this.showDrugScreeningFeature) {
      this.showDrugScreeningFeature = this.appService.getFeatureToggleDate('DrugScreening');
    }
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
  }

  clickUserAccount() {
    this.isUserAccountSubMenuDisplayed = !this.isUserAccountSubMenuDisplayed;
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
    this.showContactsInfoFeature = this.appService.getFeatureToggleDate('ContactsInfo');
  }

  clickPT() {
    this.isPTSubMenuDisplayed = !this.isPTSubMenuDisplayed;
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
  }

  clickEA() {
    this.isEASubMenuDisplayed = !this.isEASubMenuDisplayed;
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
  }
  clickAgencyTools() {
    this.isAgencyToolsSubMenuDisplayed = !this.isAgencyToolsSubMenuDisplayed;
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
  }

  navigateTo(page: string) {
    switch (page) {
      case 'home': {
        this.router.navigateByUrl('/home');
        break;
      }
      case 'participant-summary': {
        this.router.navigateByUrl('/pin/' + this.participantPin);
        break;
      }
      case 'pop-claims/adjudicator': {
        this.router.navigateByUrl('/pop-claims/adjudicator');
        break;
      }
      case 'clearance': {
        this.router.navigateByUrl('/clearance');
        break;
      }
      case 'help': {
        this.helpService.getHelpUrl('main-header-help').subscribe(x => {
          window.open(x[0], '_blank');
        });
        break;
      }
      case 'release-notes': {
        this.router.navigateByUrl('/release-notes');
        break;
      }
      case 'logout': {
        this.router.navigateByUrl('/logout');
        break;
      }
      case 'employability-plan': {
        this.router.navigateByUrl('pin/' + this.participantPin + '/employability-plan/list');
        break;
      }
      case 'reports': {
        this.router.navigateByUrl('/reports');
        break;
      }
      case 'career-assessment': {
        this.router.navigateByUrl(`/pin/${this.participantPin}/career-assessment`);
        break;
      }
      case 'job-readiness': {
        this.router.navigateByUrl('pin/' + this.participantPin + '/job-readiness');
        break;
      }

      case 'pin-comments': {
        this.router.navigateByUrl('pin/' + this.participantPin + '/pin-comments');
        break;
      }
      case 'organization-information': {
        this.router.navigateByUrl('/organization-information');
        break;
      }
      case 'worker-task': {
        this.router.navigateByUrl('/worker-task');
        break;
      }
      default: {
        break;
      }
    }
    this.closeSubMenu();
  }

  private closeSubMenu() {
    this.isWorkerToolsSubMenuDisplayed = false;
    this.isCaseManagementSubMenuDisplayed = false;
    this.isUserAccountSubMenuDisplayed = false;
    this.isPTSubMenuDisplayed = false;
    this.isEASubMenuDisplayed = false;
    this.isAgencyToolsSubMenuDisplayed = false;
  }

  showSimulateUserBox() {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalService.create<LoginSimulationDialogComponent>(LoginSimulationDialogComponent).subscribe(x => {
      this.tempModalRef = x;
    });
  }

  showContactInfoBox() {
    if (this.contactInfoModalRef && this.contactInfoModalRef.instance) {
      this.contactInfoModalRef.instance.destroy();
    }
    this.modalService.create<ContactInfoDialogComponent>(ContactInfoDialogComponent).subscribe(x => {
      this.contactInfoModalRef = x;
    });
  }

  cancelUserSimualtion() {
    this.appService.stopUserSimulation();
  }

  showSimulateCurrentDateBox() {
    if (this.simulateCurrentDateModalRef && this.simulateCurrentDateModalRef.instance) {
      this.simulateCurrentDateModalRef.instance.destroy();
    }
    this.modalService.create<DateChangerComponent>(DateChangerComponent).subscribe(x => {
      this.simulateCurrentDateModalRef = x;
    });
  }

  cancelCurrentDateSimulation() {
    this.appService.stopDateSimulation();
    SystemClockService.cancelSimulateClientDateTime();
  }

  showTimeoutWarning() {
    // NOTE: We may get called multiple times during the timeout scenario.  If
    // we do, ignore the secondary requests.
    if (this.timeoutModalRef && this.timeoutModalRef.instance) {
      return;
    }

    this.modalService.create<TimeoutDialogComponent>(TimeoutDialogComponent).subscribe(ref => {
      this.timeoutModalRef = ref;

      ref.onDestroy(() => {
        this.timeoutModalRef = null;
      });
    });
  }
  cleanupTimeoutWarning() {
    if (this.timeoutModalRef && this.timeoutModalRef.instance) {
      this.timeoutModalRef.instance.destroy();
    }
  }
}
