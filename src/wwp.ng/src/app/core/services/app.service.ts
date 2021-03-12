import { Utilities } from './../../shared/utilities';
import { Event } from './../../shared/models/event.model';
import { Office } from './../../shared/models/office';
import { FeatureToggle } from './../../shared/models/feature-toggle.model';
import { LogService } from './../../shared/services/log.service';
import { AuthHttpClient, IRequestOptions } from './../interceptors/AuthHttpClient';
import { CoreAccessContext } from './../../shared/models/core-access-context';
import { ValidationError, ValidationCode } from './../../shared/models/validation-error';
import { Authorization } from './../../shared/models/authorization';
// tslint:disable: deprecation
// tslint:disable: no-use-before-declare
// tslint:disable: triple-equals
import { BehaviorSubject, Observable, Subject, of } from 'rxjs';
import { filter, pairwise, map, flatMap, catchError } from 'rxjs/operators';
import { Injectable, ComponentRef } from '@angular/core';
import { Router, NavigationEnd, NavigationCancel, Event as NavigationEvent, ActivationEnd } from '@angular/router';

import { JwtHelperService } from '@auth0/angular-jwt';
import * as moment from 'moment';
import { BaseService } from './base.service';
import { JwtAuthConfig } from '../jwt-auth-config';
import { SystemClockService } from 'src/app/shared/services/system-clock.service';
import { User } from 'src/app/shared/models/user';
import { ValidationMsg } from 'src/app/shared/services/validation-msg';
import { HttpHeaders } from '@angular/common/http';
import { SemanticVersion } from 'src/app/shared/version';
import { Participant } from 'src/app/shared/models/participant';
import { EnrolledProgramStatus } from 'src/app/shared/enums/enrolled-program-status.enum';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { HasProgramCode } from 'src/app/shared/interfaces/program-code.interface';
import { Contact } from 'src/app/features-modules/contacts/models/contact';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { Location } from '@angular/common';

@Injectable()
export class AppService extends BaseService {
  simulatedToken: string;
  public isFEP = false;
  public employmentId: number;
  public epProgramTypeId: number;
  originalTokenSetter: (data: any) => void;
  originalTokenGetter: () => string;
  public isUserSimulated: boolean;

  public componentDataModifiedFromElasped = false;

  public componentDataModified = new BehaviorSubject<any>({ dataModified: false });

  public static get currentDate() {
    return SystemClockService.appDateTime;
  }
  get canAccessPB(): boolean {
    return this.isUserAuthorized(Authorization.canAccessPB, null);
  }
  get canAccessPHI(): boolean {
    return this.isUserAuthorized(Authorization.canAccessPHI, null);
  }
  get isStateStaff(): boolean {
    return this.isUserAuthorized(Authorization.isStateStaff, null);
  }
  get canAccessFamilyBarrierSection_View(): boolean {
    return this.isUserAuthorized(Authorization.canAccessFamilyBarrierSection_View, null);
  }
  get canAccessFamilyBarrierSection_Edit(): boolean {
    return this.isUserAuthorized(Authorization.canAccessFamilyBarrierSection_Edit, null);
  }
  get isAdjudictor(): boolean {
    return this.isUserAuthorized(Authorization.canAccessAdjudicatorPOP_View, null);
  }
  get isPOPClaimApprover(): boolean {
    return this.isUserAuthorized(Authorization.canAccessApproverPOP_View, null);
  }

  // This is the only way we can differentiate between worker and (approver or adjusdicator) because all of them have the canAccessPOPClaims_View access
  get isPOPClaimWorker(): boolean {
    return this.isUserAuthorized(Authorization.canAccessPOPClaims_Edit, null);
  }

  public isUrlChangeBlocked = false;
  public isEPUrlChangeBlocked = false;
  public isDialogPresent = false;
  public isChildDialogPresent = false;
  public setDialogueFromDriverFlow = false;
  public previousRoutingEvent: NavigationEvent;
  public currentRoutingEvent: NavigationEvent;
  public sumulationStatusChanged = new Subject<boolean>();
  public currentEnvironment: string;
  private exitUrl: string;

  private authUrl: string;
  public authStatus: string;
  private authorizations: Array<Authorization> = null;

  user: User;
  redirectUrl: string;

  private jwtHelper: JwtHelperService;

  private validationErrors: Array<ValidationError> = null;
  /* creating this BehaviorSubject so that these two variables can be used throughout the components
  and can be modified on the fly using .next() */
  public employabilityPlan = new BehaviorSubject<any>({ id: 0 });
  public activitiesFromOverview = new BehaviorSubject<any>({});
  public goalsFromOverview = new BehaviorSubject<any>({});
  public employabilityPlanInfo = new BehaviorSubject<any>(null);
  public inHistoryMode = new BehaviorSubject<any>({ inHistory: false });
  public submittedEps = new BehaviorSubject<any>({});
  public submittedEpGoToOverview = new BehaviorSubject<any>({ submittedEp: false });
  public isEventsLoaded = new BehaviorSubject<boolean>(false);
  public loadingComponentInput = new BehaviorSubject<any>({ isLoaded: true, loadingLabel: '' });
  public cachedEvents = new BehaviorSubject<Event[]>(null);
  public isCommentsShown = false;
  public participantInfo = new BehaviorSubject<any>({});
  public PBSection = new BehaviorSubject<any>({
    hasPBAccessBol: false,
    canRequestPBAccess: false,
    requestedElevatedAccess: false
  });

  public FBSection = new BehaviorSubject<any>({
    hasFBAccessBol: false,
    canRequestFBAccess: false,
    requestedElevatedAccess: false
  });
  public featureToggles = new BehaviorSubject<any>(null);

  private validationErrorsJson = ValidationMsg;

  public coreAccessContext: CoreAccessContext;

  private isFeatureToggleValuesCalled = false;
  public wageHistory = new BehaviorSubject<any>(null);

  constructor(http: AuthHttpClient, logService: LogService, private router: Router, private jwtAuthConfig: JwtAuthConfig, private location: Location) {
    super(http, logService);

    this.authUrl = this.apiServer + 'api/auth';
    // this.log = new Logger();
    this.jwtHelper = new JwtHelperService();

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd || e instanceof NavigationCancel),
        pairwise()
      )
      .subscribe(events => {
        this.previousRoutingEvent = events[0];
        this.currentRoutingEvent = events[1];
      });

    const origToken = this.getOrigAuthToken();
    this.setAuthToken(origToken);
    SystemClockService.cancelSimulateClientDateTime();

    if (this.location.path() !== '/logout' && !this.isFeatureToggleValuesCalled) {
      this.isFeatureToggleValuesCalled = true;
      this.callFeatureToggleValues();
    }
  }

  callFeatureToggleValues() {
    if (
      this.isUserAuthenticatedCurrently() &&
      !this.http.isUserInactive() &&
      (!this.featureToggles.value || (this.featureToggles.value && this.featureToggles.value.length === 0))
    ) {
      this.getFeatureToggleValues().subscribe(res => this.featureToggles.next(res));
    }
  }

  authenticateUser(username: string, password: string) {
    // get auth & token from the server
    const body = JSON.stringify({ username: username, password: password });

    return this.http.post(this.authUrl, body).pipe(
      map(response => {
        const r: any = response || {};

        if (r == null) {
          this.authStatus = 'User is unauthorized';
          this.setOrigAuthToken('');
          this.setAuthToken('');
          this.user = null;
          this.coreAccessContext = null;
          // return false;
        } else {
          this.authStatus = r.message;
          // store token
          // HACK: Temp Auth!!!
          // this.authToken = JSON.stringify(r.user);
          this.setOrigAuthToken(r.token);
          this.setAuthToken(r.token);
        }
      }),
      flatMap(() => {
        return this.isUserAuthenticated();
      })
    );
  }

  private parseAuthorizations(auths: string[]) {
    this.authorizations = [];

    for (const auth of auths) {
      const authEnum = Authorization[auth];
      if (authEnum == null) {
        // TODO: Log this condition.
      } else {
        this.authorizations.push(authEnum);
      }
    }
  }

  public getAppRoles(): Observable<any> {
    return this.http.get(this.authUrl + '/roles').pipe(
      map(x => {
        const res = x;
        return res;
      })
    );
  }

  public getUsernames(): Observable<any> {
    return this.http.get(this.authUrl + '/usernames').pipe(
      map(x => {
        const res = x;
        return res;
      })
    );
  }

  public getAgencyCode(): Observable<any> {
    return this.http.get(this.authUrl + '/agencycodes').pipe(
      map(x => {
        const res = x;
        return res;
      })
    );
  }

  public getOffices(): Observable<Office[]> {
    return this.http.get(this.authUrl + '/offices').pipe(
      map(x => {
        const data = x;
        // TODO: Fix serialization/deserialization
        const results: Office[] = [];

        for (const contract of data) {
          results.push(Object.assign(new Office(), contract));
        }
        return results;
      })
    );
  }

  public getFeatureToggleValues(): Observable<FeatureToggle[]> {
    const featureToggleUrl = `${this.apiServer}api/participants/featureToggles/all`;
    return this.http.get(featureToggleUrl).pipe(
      map(res => this.extractFeatureToggleData(res)),
      catchError(err => this.handleError(err))
    );
  }

  private extractFeatureToggleData(res: FeatureToggle[]) {
    const jsonObjs = res as FeatureToggle[];
    const objs: FeatureToggle[] = [];

    for (const obj of jsonObjs) {
      objs.push(new FeatureToggle().deserialize(obj));
    }

    return objs || [];
  }

  public getFeatureToggleDate(feature: string) {
    let featureToggleDate = null;
    let showFeature = false;
    const featureToggleValues = this.featureToggles.value;
    if (featureToggleValues && featureToggleValues.length > 0) {
      featureToggleDate = featureToggleValues.filter(i => i.parameterName === feature)[0].parameterValue;
      if (Utilities.currentDate.isSameOrAfter(moment(featureToggleDate))) showFeature = true;
      else showFeature = false;
    }
    return showFeature;
  }

  public getFeatureToggleValue(feature: string) {
    let featureToggleValue = null;
    const featureToggleValues = this.featureToggles.value;
    if (featureToggleValues && featureToggleValues.length > 0) {
      featureToggleValue = featureToggleValues.filter(i => i.parameterName === feature)[0].parameterValue;
    }
    return featureToggleValue;
  }

  public getServerStatus(): Observable<ServerStatusContract> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = {
      headers: headers,
      withCredentials: false
    } as IRequestOptions; // dont' try to pass token
    return this.http.get(this.authUrl + '/status', options).pipe(
      map(x => {
        const contract = new ServerStatusContract();
        const responseObj = x;
        contract.date = moment(responseObj.date);
        contract.version = new SemanticVersion(+responseObj.version.major, +responseObj.version.minor, +responseObj.version.build, responseObj.version.revision);
        contract.environment = responseObj.environment;
        return contract;
      })
    );
  }

  logoutUser(): void {
    this.authStatus = '';
    this.user = null;
    this.coreAccessContext = null;
    this.setOrigAuthToken('');
    this.setAuthToken('');
    this.featureToggles.next(null);
    this.http.clearUserActive();
  }

  isUserNavigationBlocked(): Observable<boolean> {
    const result = this.isUrlChangeBlocked;
    return of(result);
  }

  isUserAuthenticated(): Observable<boolean> {
    const validToken = this.isUserAuthenticatedCurrently();
    return of(validToken);
  }

  isUserAuthenticatedCurrently(): boolean {
    return this.validateUserToken();
  }

  private validateUserToken(): boolean {
    let validToken = false;
    try {
      const token = this.getAuthToken();
      validToken = this.validateToken(token);
      if (validToken && !this.user) {
        this.authStatus = 'auth OK';
        const user = this.getUserFromToken(token);
        this.user = user;
      }
    } catch (err) {
      validToken = false;
      // TODO: add some debugging statements, but to not
      // propegate an errors validating the ticket
    }
    return validToken;
  }
  simulateUser(username: string, roles: string[], orgCode: string = null) {
    const body = {
      username: username,
      roles: roles,
      orgCode: orgCode,
      authorizedUserToken: this.jwtAuthConfig.tokenGetter('token')
    };
    return this.http.post(this.authUrl + '/simulate/', body).pipe(
      map(response => {
        const r = response || {};

        if (r == null || r.token == null) {
          this.authStatus = 'Simulation Failed';
          this.isUserSimulated = false;
          return false;
        } else {
          this.authStatus = r.message;
          // store token
          // HACK: Temp Auth!!!
          this.isUserSimulated = true;
          this.user = null;
          this.coreAccessContext = null;
          this.setAuthToken(r.token);
          this.validateUserToken();
          this.sumulationStatusChanged.next(true);
          return true;
        }
      })
    );
  }

  simulateDate(username: string, roles: string[], orgCode: string = null, cdoDate: string) {
    const body = {
      username: username,
      roles: roles,
      orgCode: orgCode,
      authorizedUserToken: this.getAuthToken(),
      cdoDate: cdoDate
    };
    return this.http.post(this.authUrl + '/simulate/', body).pipe(
      map(response => {
        const r = response || {};

        if (r == null || r.token == null) {
          this.authStatus = 'Simulation Failed';
          this.isUserSimulated = false;
          return false;
        } else {
          this.authStatus = r.message;
          this.user = null;
          this.coreAccessContext = null;
          this.setAuthToken(r.token);
          this.validateUserToken();
          return true;
        }
      })
    );
  }

  requestRestrictedAccess(model, pin: string) {
    const body = JSON.stringify(model);
    return this.http.post(`${this.authUrl}/${pin}/elevatedaccess`, body);
  }

  stopUserSimulation() {
    this.setAuthToken(this.getOrigAuthToken());
    this.isUserSimulated = false;
    this.user = null;
    this.coreAccessContext = null;
    this.validateUserToken();
    this.sumulationStatusChanged.next(false);
  }

  stopDateSimulation() {
    this.setAuthToken(this.getOrigAuthToken());
    this.user = null;
    this.coreAccessContext = null;
    this.validateUserToken();
  }

  isUserAuthorized(authorization: Authorization, participant?: Participant): boolean {
    // Do some basic checks to make sure we have the needed objects.
    if (this.authorizations == null) {
      return false;
    }

    if (this.user == null) {
      return false;
    }

    // First check if the authorization requested has been given to the user.
    if (this.authorizations.indexOf(authorization) < 0) {
      return false;
    }

    // NOTE!! If the Participant context is null it means we are just asking in a
    // generic sense, so skip looking at the context.
    if (participant == null) {
      return true;
    }

    // Assume the user does not have access.
    let isAuth = false;

    // Next check for special authorization checks where we need to look at the
    // PIN, office, agency, etc.
    switch (authorization) {
      // sec_ignore
      // case Authorization.canAccessInformalAssessment_Edit: {
      //   // We either need the user to be in the same agency as what
      //   // the participant is assigned to or the user needs to be from
      //   // the State.
      //   isAuth = participant.agencyCode == null || participant.agencyCode === '' || this.user.agencyCode === participant.agencyCode || this.user.agencyCode === 'WI';
      //   break;
      // }
      default: {
        // By default there is no special logic, so since they already have
        // the authorization from the login, the user does have access.
        isAuth = true;
        break;
      }
    }

    return isAuth;
  }

  public checkReadOnlyAccess(participant: Participant, programStatus: EnrolledProgramStatus[], hasAuth: boolean): boolean {
    let isReadOnly = false;

    if (this.user && this) {
      // First get the most recent programs the user has access to.
      let progs = participant.getMostRecentProgramsUserHasAccessTo(this.user, this);

      // Now filter those out so we only get Enrolled and Disenrolled.
      progs = EnrolledProgram.filterByStatuses(progs, programStatus);

      // If we dont have have any programs after filtering or if there is no edit access, then the worker can't edit.
      isReadOnly = progs.length === 0 || !hasAuth;
    }

    return isReadOnly;
  }

  public isUserEASupervisor(): boolean {
    return this.user.roles.indexOf('EA Supervisor') > -1;
  }

  public isUserEAWorker(): boolean {
    return this.user.roles.indexOf('EA Worker') > -1;
  }

  public isUserCFWorker(): boolean {
    return this.user.roles.indexOf('CF Case Manager') > -1;
  }

  public isUserHDWorker(): boolean {
    return this.user.roles.indexOf('Help Desk') > -1;
  }

  public isUserDCFMonitoring(): boolean {
    return this.user.roles.indexOf('DCF Staff – Monitoring') > -1;
  }

  public isUserAuthorizedToEditWorkHistory(participant: Participant): boolean {
    const hasAuth = this.isUserAuthorized(Authorization.canAccessWorkHistoryApp_Edit, participant);
    if (hasAuth === true) {
      return hasAuth;
    }

    return false;
  }

  public isUserAuthorizedToEditJobReadiness(participant: Participant): boolean {
    const hasAuth = this.isUserAuthorized(Authorization.canAccessJobReadiness_Edit, participant);
    if (hasAuth === true) {
      return hasAuth;
    }

    return false;
  }

  public isUserAuthorizedToViewKidsWHReport(participant: Participant): boolean {
    const hasAuth = this.isUserAuthorized(Authorization.canAccessKIDSWHReport, participant);
    if (hasAuth === true) {
      return hasAuth;
    }

    return false;
  }

  public isUserAuthorizedToDeleteTimeLimitsExtension(participant: Participant): boolean {
    //Only "W-2 Help Desk" role will be able to delete Time Limit Extension decisions
    const hasAuth = this.isUserAuthorized(Authorization.timeLimitsCanDeleteExtensionDecision, participant);
    if (hasAuth === true) {
      return hasAuth;
    }

    // If we get to this point, they're not authorized to edit.
    // console.log('TimeLimits EDIT nope');
    return false;
  }

  public isUserAuthorizedToPrintWorkHistory(participant: Participant): boolean {
    //Only "canPrintWH" authorized users will be able to Print Work History Items
    const hasAuth = this.isUserAuthorized(Authorization.canPrintWH, participant);
    if (hasAuth === true) {
      return hasAuth;
    }

    return false;
  }

  public isUserAuthorizedToEditTimeLimits(participant: Participant): boolean {
    // "W-2 Help Desk" role will be able to view and edit the Time Limit Application
    // including the Federal Indicator for all W-2 PINs regardless of the participant's
    // W-2 status
    let hasAuth = this.isUserAuthorized(Authorization.timeLimitsEditAll, participant);
    if (hasAuth === true) {
      // console.log('TimeLimits EDIT All');
      // Since they are authorized to Edit all, it doesn't matter what programs
      // the PIN has.
      return hasAuth;
    }
    // "W-2 Case Management Supervisor" and "W-2 QC Staff" roles will be able to edit
    // the Time Limit Application including the Federal Indicator for all W-2 "Referred, Enrolled, Disenrolled"
    // PINs within their agency
    hasAuth = this.isUserAuthorized(Authorization.timeLimitsEditInAgency, participant);
    if (hasAuth === true) {
      // console.log('TimeLimits EDIT Sup/Staff... checking Referred, Enrolled, Disenrolled');
      // We know the worker is authorized to edit PINs in the Agency, so we need to check
      // that they have a W-2 Enrolled PIN before returning true.
      let progs: EnrolledProgram[] = [];
      if (participant) {
        progs.push(participant.getMostRecentW2Program());
        progs = EnrolledProgram.filterByStatuses(progs, [EnrolledProgramStatus.referred, EnrolledProgramStatus.enrolled, EnrolledProgramStatus.disenrolled]);
      }

      if (progs.length > 0) {
        // console.log('TimeLimits EDIT Sup/Staff with ENROLL');
        return true;
      }
    }

    // "W-2 Case Management - FEP" and "W-2 Case Management Supervisor" roles will be able
    // to edit the Time Limit Application for W-2 PINs that they are assigned to for the
    // most recent instance of W-2
    hasAuth = this.isUserAuthorized(Authorization.timeLimitsEditMostRecentAssigned, participant);
    if (hasAuth === true) {
      // console.log('TimeLimits EDIT timeLimitsEditMostRecentAssigned');
      // We know the worker is authorized to edit PINs in the Agency, so we need to check
      // that here.
      if (participant) {
        const assignedW2Programs = participant.getMostRecentW2ProgramsUserIsAssignedTo(this.user, this);
        if (assignedW2Programs != null && assignedW2Programs.length > 0) {
          // console.log('TimeLimits EDIT timeLimitsEditMostRecentAssigned with ASSIGNED');
          return true;
        }
      }
    }

    // If we get to this point, they're not authorized to edit.
    // console.log('TimeLimits EDIT nope');
    return false;
  }

  public isUserAuthorizedToViewClientReg(participant: Participant): boolean {
    // NOTE: It is assumed that the logic that calls this will already have called the
    // Edit check (isUserAuthorizedToEditClientReg) before this so this method will skip
    // checking the view for those scenarios as it would be a duplicate check.

    // The following roles will have read access to Client Registration of all PINs:
    let hasAuth = this.isUserAuthorized(Authorization.canViewClientRegAll, participant);
    if (hasAuth === true) {
      console.log('ClientReg VIEW All');
      // Since they are authorized to Edit all, it doesn't matter what programs
      // the PIN has.
      return hasAuth;
    }

    // The following roles will have read access to Client Registration if the participant
    // is "Referred" or "Enrolled" for W-2 in their agency when co-enrolled with Children First.
    //
    // The following roles in an organization within Milwaukee county will have read access
    // to participants if "Referred" or "Enrolled" for W-2 within any Milwaukee County WP
    // office, when participant is co-enrolled with Children First

    hasAuth = this.isUserAuthorized(Authorization.canViewClientRegCoEnroll, participant);
    if (hasAuth === true) {
      console.log('ClientReg VIEW CoEnroll check');
      let w2Program = participant.getMostRecentW2ProgramByAgency(this.user.agencyCode);

      // If there wasn't a W2 program for the agency, look for the MKE condition.
      if (w2Program == null) {
        w2Program = participant.getMostRecentW2Program();
        if (w2Program != null) {
          console.log('ClientReg VIEW MKE W2 check');
          if (this.areBothAgenciesInMilwaukee(this.user.agencyCode, w2Program.agencyCode)) {
            console.log('ClientReg VIEW MKE both in W2');
          } else {
            // Since they both aren't in MKE, we'll null out the w2Program
            // variable so the logic below won't be invoked.
            w2Program = null;
          }
        }
      }

      if (w2Program != null) {
        console.log('ClientReg VIEW CoEnroll has W2');

        if (EnrolledProgram.isStatus(w2Program, EnrolledProgramStatus.enrolled) || EnrolledProgram.isStatus(w2Program, EnrolledProgramStatus.referred)) {
          console.log('ClientReg VIEW CoEnroll has W2 Enrolled/Referred');

          if (participant.isEnrolledInCF()) {
            console.log('ClientReg VIEW CoEnroll has W2 Enrolled/Referred + CF Enrolled');
            return true;
          }
        }
      }
    }

    // If we get to this point, they're not authorized to edit.
    console.log('ClientReg VIEW nope');
    return false;
  }

  /**
   * Checks  if user can Access Non Participant Details page
   */
  public isUserAuthorizedToViewNonParticipationDetails(participant: Participant): boolean {
    const hasAuth = this.isUserAuthorized(Authorization.canAccessNonParticipationDetails_View, participant);
    if (hasAuth === true) {
      return true;
    }
    return false;
  }

  /**
   * Checks  if user can Access Auxilary View page
   */
  public isUserAuthorizedToViewAuxilaryPage(participant: Participant): boolean {
    return this.isUserAuthorized(Authorization.canAccessAuxiliary_View, participant);
  }

  public isUserAuthorizedToEditClientReg(participant: Participant, isEditDemographicsMode: boolean, isEASameAgency = false): boolean {
    // The following roles will have read and update access to Client Registration for
    // all PINs when accessing Client Registration directly after Clearance:
    //
    // The following roles will have read and update access to Client Registration for
    // all PINs where most recent instance of the program the worker has access to is
    // within their agency and the program instance is in Referred or Enrolled status:

    let hasAuth = this.isUserAuthorized(Authorization.canAccessClientReg_Edit, participant);
    if (hasAuth === false) {
      console.log('ClientReg EDIT nope');
      // Since they are not authorized to Edit, it doesn't matter what programs
      // the PIN has or what state
      return hasAuth;
    }

    // At this point, we know they have the Edit Auth, but the PIN might not be in
    // referred or enrolled status.
    if (isEditDemographicsMode) {
      console.log('ClientReg isEditDemographicsMode');
      let programs = participant.getMostRecentReferredProgramsUserHasAccessTo(this.user, this);
      if (isEASameAgency || this.isUserHDWorker()) {
        hasAuth = true;
      } else if (programs == null || programs.length === 0) {
        console.log('ClientReg isEditDemographicsMode + no referred');
        // Most recent referred was empty, but let's check Enrolled before we set
        // their access to false.
        programs = participant.getCurrentEnrolledProgramsUserHasAccessTo(this.user, this);
        if (programs == null || programs.length === 0) {
          console.log('ClientReg isEditDemographicsMode + no enrolled');
          hasAuth = false;
        } else {
          console.log('ClientReg isEditDemographicsMode + with enrolled, EDIT OK');
          hasAuth = true;
        }
      }
    } else {
      console.log('ClientReg clearance OK');
    }

    return hasAuth;
  }

  /**
   * Filters an array of objects that have a program code and removes ones that the current
   * authorized user does not have access to.
   *
   * @param {EnrolledProgram[]} programs
   * @returns {EnrolledProgram[]}
   * @memberof AppService
   */
  filterProgramsForUserAuthorized<T extends HasProgramCode>(programs: Array<T>): Array<T> {
    return programs.filter(p => this.isUserAuthorizedForProgram(p));
  }

  filterProgramsForUserAssigned(programs: EnrolledProgram[]): EnrolledProgram[] {
    return programs.filter(p => this.isUserAssignedToProgram(p));
  }

  isUserAuthorizedForAnyProgram(programs: EnrolledProgram[]): boolean {
    // Some sanity checks.
    if (programs == null || programs.length === 0) {
      return false;
    }

    // They just have to be authorized for one of the programs in the array.
    for (const ep of programs) {
      if (this.isUserAuthorizedForProgram(ep)) {
        return true;
      }
    }

    // They weren't authorized for any, so return false.
    return false;
  }

  isUserAuthorizedForEdit(authFromRoute: Authorization): boolean {
    if (authFromRoute == null) {
      return false;
    }

    const authorizations = [Authorization.canAccessParticipantSummary, Authorization.canAccessTransactions_View, Authorization.canAccessPaymentDetails_View];
    if (authorizations.includes(authFromRoute)) return this.isUserAuthorized(authFromRoute, null);

    const authSplit = Authorization[authFromRoute].split('_');

    if (authSplit[1] === 'View') {
      const canEditAuth = authSplit[0] + '_Edit';
      return this.isUserAuthorized(Authorization[canEditAuth], null);
    } else if (authSplit[1] === 'Edit') {
      return true;
    } else {
      return false;
    }
  }

  isUserAuthorizedForProgram(program: HasProgramCode): boolean {
    // Some sanity checks.
    if (program == null) {
      return false;
    }

    // Figure out which Auth enum we should be using.
    const accessAuth: Authorization | undefined = (<any>Authorization)[`canAccessProgram_${program.programCd.trim()}`];

    if (accessAuth === undefined) {
      return false;
    }

    return this.isUserAuthorized(accessAuth, null);
  }

  isUserAuthorizedForProgramByCode(programCd: string): boolean {
    // Some sanity checks.
    if (programCd == null) {
      return false;
    }

    // Figure out which Auth enum we should be using.
    const accessAuth: Authorization | undefined = (<any>Authorization)[`canAccessProgram_${programCd.trim()}`];

    if (accessAuth === undefined) {
      return false;
    }

    return this.isUserAuthorized(accessAuth, null);
  }

  isUserAssignedToProgram(program: EnrolledProgram): boolean {
    // Some sanity checks.
    if (program == null) {
      return false;
    }

    return program.assignedWorker.wamsId === this.user.username;
  }

  isParticipantAccessibleIfConfidential(participant: Participant): boolean {
    if (participant.isConfidentialCase) {
      return participant.isConfidentialCase === participant.hasConfidentialAccess || participant.pin === this.user.elevatedAccessPin;
    }

    return true;
  }

  isParticipantEditable(participant: Participant): boolean {
    if (!this.isParticipantAccessibleIfConfidential(participant)) {
      return false;
    }

    let progs = participant.getMostRecentProgramsUserHasAccessTo(this.user, this);
    const enrolledProgs = [];
    // Now filter those out so we only get Enrolled .
    progs = EnrolledProgram.filterByStatuses(progs, [EnrolledProgramStatus.enrolled]);

    progs.forEach(prog => {
      if (prog.agencyCode == this.user.agencyCode) {
        enrolledProgs.push(prog);
      }
    });

    if (enrolledProgs.length > 0) {
      return true;
    }
  }

  checkForFCDP(programCds) {
    let hasOnlyFcdp = false;
    if (!programCds || (programCds && programCds.length === 0)) {
      return hasOnlyFcdp;
    }
    const progAuths = [];
    const nonFcdpPrograms = [];
    let fcdProgram = null;
    let program = null;
    if (this && this.user && this.user.authorizations) {
      this.user.authorizations.forEach(i => {
        if (i.indexOf('canAccessProgram') > -1) progAuths.push(i);
      });
      if (progAuths && progAuths.length > 0) {
        progAuths.forEach(i => {
          program = i.split('canAccessProgram_')[1].toUpperCase();
          if (program === 'FCD') fcdProgram = program;
          else nonFcdpPrograms.push(program);
        });
      }
      if (fcdProgram && nonFcdpPrograms.length === 0) hasOnlyFcdp = true;
      else if (fcdProgram && nonFcdpPrograms.length > 0) {
        if (!nonFcdpPrograms.some(i => programCds.indexOf(i) > -1)) hasOnlyFcdp = true;
      }
    }
    return hasOnlyFcdp;
  }

  public hideModifiedWarningDialog() {
    this.exitUrl = '';
    this.isDialogPresent = false;
    //this.componentDataModified.next({ dataModified: false });
    this.componentDataModifiedFromElasped = false;
  }

  public exitModifiedWarningDialog() {
    this.isEPUrlChangeBlocked = false;
    this.router.navigateByUrl(this.exitUrl).then(() => {
      this.exitUrl = '';
    });
    this.isDialogPresent = false;
    this.componentDataModifiedFromElasped = false;
    this.componentDataModified.next({ dataModified: false });
  }

  // private extractData(res: Response): AuthResponse {
  //   let ar: AuthResponse = null;

  //   // We only handle a 200 respinse where the response
  //   // is valid.
  //   if (res.status === 200) {
  //     ar = <AuthResponse>res.json();
  //   }

  //   return ar;
  // }

  // private handleError(error: Error | Response) {
  //   super.handleError(error)
  //   // // In a real world app, we might use a remote logging infrastructure
  //   // // We'd also dig deeper into the error to get a better message
  //   // // In a real world app, we might use a remote logging infrastructure
  //   // let errMsg: string;
  //   // if (error instanceof Response) {
  //   //   const body = error.json() || '';
  //   //   const err = body.error || JSON.stringify(body);
  //   //   errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
  //   // } else {
  //   //   errMsg = error.message ? error.message : error.toString();
  //   // }
  //   // return Observable.throw(errMsg);
  // }

  public validateToken(token: string): boolean {
    if (!token) {
      return false;
    }
    // HACK: Temp Auth!!!
    // return true;
    const expired = this.jwtHelper.isTokenExpired(token);
    // decode token and check anything else necessary
    const decodedToken = this.getDecodedtoken(token);
    if (decodedToken && decodedToken.sub) {
      // For localhost:4200 testing with expired tokens:
      // return true;
      return !expired;
    } else {
      return false;
    }
  }

  public isTokenExpired(token: string): boolean {
    return this.jwtHelper.isTokenExpired(token, this.jwtAuthConfig.expirationOffset);
  }

  private getUserFromToken(token: string): User {
    // HACK: Temp Auth!!!
    // const user = JSON.parse(token);
    // this.parseAuthorizations(user.authorizations);
    //
    // return user;

    const decodedToken = this.getDecodedtoken(token);
    if (decodedToken) {
      const user = new User();
      user.firstName = decodedToken.given_name;
      user.lastName = decodedToken.family_name;
      user.officeName = decodedToken.office_name;
      user.agencyCode = decodedToken.agency;
      user.username = decodedToken.sub;
      user.mainFrameId = decodedToken.mainframe_id;
      user.roles = decodedToken.roles;
      user.wiuid = decodedToken.wiuid;
      user.authorizations = [];

      if (user.username != null && user.username !== '') {
        localStorage.setItem('username', user.username);
      }
      if (typeof decodedToken.authorizations === 'string') decodedToken.authorizations = decodedToken.authorizations.split();
      if (decodedToken.authorizations && decodedToken.authorizations.length) {
        for (const auth of decodedToken.authorizations) {
          user.authorizations.push(auth);
        }
      }

      this.parseAuthorizations(user.authorizations);

      return user;
    }
  }

  private getDecodedtoken(t): any {
    try {
      return this.jwtHelper.decodeToken(t);
    } catch (ex) {
      // TODO:Log this
    }
    return null;
  }

  public getValidationError(errCode: ValidationCode): ValidationError {
    if (this.validationErrors == null) {
      this.validationErrors = JSON.parse(JSON.stringify(this.validationErrorsJson));
    }
    return this.validationErrors.find(ve => ve.code === errCode);
  }

  public getAuthToken(): string {
    return this.jwtAuthConfig.tokenGetter('token');
  }

  public setAuthToken(token: string) {
    this.jwtAuthConfig.tokenSetter('token', token);
  }

  public getOrigAuthToken(): string {
    return this.jwtAuthConfig.tokenGetter('origToken');
  }

  public setOrigAuthToken(token: string) {
    this.jwtAuthConfig.tokenSetter('origToken', token);
  }

  // this method sets up the initial values for the PB section visibility based on the user roles.
  public hasPBAccess(participant: Participant) {
    let result = false;
    const programs = participant.getMostRecentProgramsUserHasAccessTo(this.user, this);
    // DCF role is not in canAccessPB but should be shown the restricted access model.
    if (this.canAccessPB || this.isStateStaff) {
      this.PBSection.next({
        hasPBAccessBol: true,
        canRequestPBAccess: true,
        requestedElevatedAccess: false
      });
      if (this.user.elevatedAccessPin === participant.pin || participant.hasConfidentialAccess) {
        this.PBSection.next({
          hasPBAccessBol: true,
          canRequestPBAccess: false,
          requestedElevatedAccess: false
        });
        result = true;
        return true;
      } else if (programs.length > 0) {
        programs.some(p => {
          if (participant.hasConfidentialAccess) {
            this.PBSection.next({
              hasPBAccessBol: true,
              canRequestPBAccess: false,
              requestedElevatedAccess: false
            });
            result = true;
            return result;
          } else {
            this.PBSection.next({
              hasPBAccessBol: true,
              canRequestPBAccess: true,
              requestedElevatedAccess: false
            });
            return result;
          }
        });
      }
    } else {
      this.PBSection.next({
        hasPBAccessBol: false,
        canRequestPBAccess: false,
        requestedElevatedAccess: false
      });
    }
    return result;
  }

  public hasPHIAccess(participant: Participant) {
    let result = false;
    let programs = participant.getMostRecentProgramsUserHasAccessTo(this.user, this);
    programs = EnrolledProgram.filterByStatuses(programs, [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.referred]);
    if (this.canAccessPHI || this.isStateStaff) {
      this.FBSection.next({
        hasFBAccessBol: true,
        canRequestFBAccess: true,
        requestedElevatedAccess: false
      });
      if (this.user.elevatedAccessPin === participant.pin || participant.hasConfidentialAccess) {
        this.FBSection.next({
          hasFBAccessBol: true,
          canRequestFBAccess: false,
          requestedElevatedAccess: false
        });
        result = true;
        return true;
      } else if (programs.length > 0) {
        programs.some(p => {
          if (participant.hasConfidentialAccess) {
            this.FBSection.next({
              hasFBAccessBol: true,
              canRequestFBAccess: false,
              requestedElevatedAccess: false
            });
            result = true;
            return result;
          }
        });
      }
    }
    return result;
  }

  public filterTA(c, p): Contact[] {
    if (this.hasPHIAccess(p)) {
      return c;
    } else {
      return (c = c.filter(x => x.titleTypeName !== 'Treatment/Assessment Provider'));
    }
  }

  public filterOneTA(c, p): Contact | null {
    if (this.hasPHIAccess(p)) {
      return c;
    } else {
      if (c.titleTypeName === 'Treatment/Assessment Provider') {
        return (c = null);
      } else {
        return c;
      }
    }
  }
  public filterContactTypeDrop(contactTypesDrop: DropDownField[], p) {
    if (this.hasPHIAccess(p)) {
      return contactTypesDrop;
    } else {
      return (contactTypesDrop = contactTypesDrop.filter(x => x.name !== 'Treatment/Assessment Provider'));
    }
  }

  public isMostRecentProgramInSisterOrg(programs?: EnrolledProgram[]) {
    if (!this.coreAccessContext) {
      this.coreAccessContext = new CoreAccessContext();
    }
    for (const prog of programs) {
      // if (prog.isTmj) {
      //   if (this.isUserAuthorized(Authorization.canAccessProgram_TMJ, null)) {
      //      this.coreAccessContext.isMostRecentProgramInSisterOrg =  true;
      //     return true;
      //   }
      // }

      if (prog.isCF) {
        if (this.isUserAuthorized(Authorization.canAccessProgram_CF, null)) {
          this.coreAccessContext.isMostRecentProgramInSisterOrg = true;
          return true;
        }
      }

      if (prog.isFCDP) {
        if (this.isUserAuthorized(Authorization.canAccessProgram_FCD, null)) {
          this.coreAccessContext.isMostRecentProgramInSisterOrg = true;
          return true;
        }
      }

      if (prog.isWW) {
        if (this.areBothAgenciesInMilwaukee(this.user.agencyCode, prog.agencyCode)) {
          this.coreAccessContext.isMostRecentProgramInSisterOrg = true;
          return true;
        }
      }
    }

    this.coreAccessContext.isMostRecentProgramInSisterOrg = false;
    return false;
  }

  private isAgencyInMilwaukee(agencyCode: string): boolean {
    // TODO: Move the MKE indicator to the API so it doesn't break if the
    // Milwaukee orgs change.
    const milwaukeeOrgs = ['RS', 'AW', 'UMS', 'MAX'];
    return milwaukeeOrgs.indexOf(agencyCode) > -1;
  }

  private areBothAgenciesInMilwaukee(code1: string, code2: string): boolean {
    return this.isAgencyInMilwaukee(code1) && this.isAgencyInMilwaukee(code2);
  }
  public isProgramInMilwaukee(program: EnrolledProgram[]): boolean {
    return program[0].officeCounty.trim() === 'MILWAUKEE';
  }
  public getFamilyBarriersSectionAccess(participant: Participant) {
    // reset the access everytime this is called.
    this.FBSection.next({ access: AccessType.none });
    // write the logic here and call it on the page load of the informal Accessment;
    const programs = participant.getMostRecentProgramsUserHasAccessTo(this.user, this);
    const enrolledPrograms = EnrolledProgram.filterByStatus(programs, EnrolledProgramStatus.enrolled);
    if (this.canAccessFamilyBarrierSection_View || this.isStateStaff) {
      if (programs.length > 0) {
        if (this.user.elevatedAccessPin === participant.pin) {
          this.FBSection.next({ access: AccessType.view });
          programs.some(p => {
            if (participant.hasConfidentialAccess && this.canAccessFamilyBarrierSection_Edit) {
              this.FBSection.next({ access: AccessType.edit });
              return true;
            } else {
              this.FBSection.next({ access: AccessType.view });
              return true;
            }
          });
        }
      }
      if (enrolledPrograms.length > 0 && participant.hasConfidentialAccess) {
        enrolledPrograms.some(p => {
          if (this.canAccessFamilyBarrierSection_Edit) {
            this.FBSection.next({ access: AccessType.edit });
            return true;
          } else {
            this.FBSection.next({ access: AccessType.view });
          }
        });
      }
      if (this.isMostRecentProgramInSisterOrg && this.canAccessFamilyBarrierSection_View) {
        this.FBSection.next({ access: AccessType.view });
      } else {
        this.FBSection.next({ access: AccessType.none });
      }
    } else {
      this.FBSection.next({ access: AccessType.none });
    }
  }
  public isUserInAssociatedAgency(participant: Participant) {
    if (participant && participant.currentEnrolledProgram && participant.currentEnrolledProgram.associatedAgencyCodes)
      return participant.currentEnrolledProgram.associatedAgencyCodes.indexOf(this.user.agencyCode) > -1;
    else return false;
  }
}

export class ServerStatusContract {
  date: moment.Moment;
  version: SemanticVersion;
  environment: string;
}
