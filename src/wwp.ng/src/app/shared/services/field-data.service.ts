// tslint:disable: no-shadowed-variable
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { ActionNeededTask } from '../../features-modules/actions-needed/models/action-needed-new';
import { AppService } from './../../core/services/app.service';
import { DropDownField } from '../models/dropdown-field';
import { DropDownMultiField } from '../models/dropdown-multi-field';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { Utilities } from '../utilities';

@Injectable()
export class FieldDataService {
  // If we want to use a caching mech. to limit api hits for our fielddata service set flag to true.
  public useCache = false;
  private baseUrl: string;
  private accommodationsUrl: string;
  private actionNeededAssigneeUrl: string;
  private actionNeededPagesUrl: string;
  private actionNeededPriorityUrl: string;
  private actionNeededUrl: string;
  private aliasTypesUrl: string;

  private certificateIssuersUrl: string;
  private childCareArrangementsUrl: string;
  private contactIntervalsUrl: string;
  private contactNcprIntervalsUrl: string;
  private contactTypesUrl: string;
  private rfaContractorsUrl: string;
  private completionReasonsUrl: string;
  private countiesUrl: string;
  private countiesTribesUrl: string;
  private countriesUrl: string;
  private degreeTypesUrl: string;
  private deleteReasonsUrl: string;
  private driverLicenseStatesUrl: string;
  private driversLicenseInvalidReasonsUrl: string;
  private educationDiplomaTypesUrl: string;
  private educationTestStatusesUrl: string;
  private educationTestTypesUrl: string;
  private employerOfRecordTypesUrl: string;
  private employmentPreventionFactorsUrl: string;
  private examSubjectsByExamUrl: string;
  private gendersUrl: string;
  private goalTypesUrl: string;
  private housingSituationUrl: string;
  private intervalTypesUrl: string;
  private jobAbsenceReasonsUrl: string;
  private jobBenefitsOfferedUrl: string;
  private jobFoundMethodsUrl: string;
  private jobLeavingReasonsUrl: string;
  private jobSectorsUrl: string;
  private jobTypesUrl: string;
  private jobsActionsUrl: string;
  private languagesUrl: string;
  private licenseTypesUrl: string;
  private militaryBranchesUrl: string;
  private militaryDischargeTypesUrl: string;
  private militaryRanksUrl: string;
  private namesTypesUrl: string;
  private ncpRelationshipsUrl: string;
  private nrsTypesUrl: string;
  private participantBarriersUrl: string;
  private pendingTypesUrl: string;
  private polarInputUrl: string;
  private polarSkipUrl: string;
  private polarRefusedUrl: string;
  private polarUnknownUrl: string;
  private populationTypesUrl: string;
  private prioritiesUrl: string;
  private rfaProgramsUrl: string;
  public allRfaProgramsUrl: string;
  private relationshipsUrl: string;
  private schoolGradesUrl: string;
  private splTypesUrl: string;
  private ssiApplicationStatusesUrl: string;
  private ssnTypesUrl: string;
  private statesUrl: string;
  private suffixTypeUrl: string;
  private symptomsUrl: string;
  private transportationTypesUrl: string;
  private tribesUrl: string;
  private wageTypesUrl: string;
  private workHistoryEmploymentStatusesUrl: string;
  private workProgramStatusesUrl: string;
  private workProgramsUrl: string;
  private elevatedAccessReasonUrl: string;
  private elementUrl: string;
  private pinCommentTypesUrl: string;
  public frequencyType: string;
  public weeklyFrequency: string;
  public monthlyFrequency: string;
  [key: string]: any;

  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.baseUrl = this.appService.apiServer + 'api/fielddata/';
    this.accommodationsUrl = this.appService.apiServer + 'api/fielddata/accommodations';
    this.actionNeededUrl = this.appService.apiServer + 'api/fielddata/action-needed/';
    this.actionNeededAssigneeUrl = this.appService.apiServer + 'api/fielddata/action-needed-assignees';
    this.actionNeededPagesUrl = this.appService.apiServer + 'api/fielddata/action-needed-pages';
    this.actionNeededPriorityUrl = this.appService.apiServer + 'api/fielddata/action-needed-priorities';
    this.aliasTypesUrl = this.appService.apiServer + 'api/fielddata/alias-types';
    this.certificateIssuersUrl = this.appService.apiServer + 'api/fielddata/certificateIssuers';
    this.childCareArrangementsUrl = this.appService.apiServer + 'api/fielddata/childCareArrangements';
    this.completionReasonsUrl = this.appService.apiServer + 'api/fielddata/completion-reasons/';
    this.contactTypesUrl = this.appService.apiServer + 'api/fielddata/contactTypes';
    this.contactNcprIntervalsUrl = this.appService.apiServer + 'api/fielddata/referral-contact-interval-types';
    this.contactIntervalsUrl = this.appService.apiServer + 'api/fielddata/contactIntervalTypes';
    this.rfaContractorsUrl = this.appService.apiServer + 'api/fielddata/rfa-contractors';
    this.countiesUrl = this.appService.apiServer + 'api/fielddata/counties';
    this.countiesTribesUrl = this.appService.apiServer + 'api/fielddata/counties-tribes';
    this.countriesUrl = this.appService.apiServer + 'api/fielddata/countries';
    this.degreeTypesUrl = this.appService.apiServer + 'api/fielddata/degreeTypes';
    this.deleteReasonsUrl = this.appService.apiServer + 'api/fielddata/delete-reasons/';
    this.driversLicenseInvalidReasonsUrl = this.appService.apiServer + 'api/fielddata/drivers-license-invalid-reasons';
    this.driverLicenseStatesUrl = this.appService.apiServer + 'api/fielddata/drivers-license-states';
    this.educationDiplomaTypesUrl = this.appService.apiServer + 'api/fielddata/schoolGraduationStatusTypes';
    this.educationTestTypesUrl = this.appService.apiServer + 'api/fielddata/education-test-types';
    this.educationTestStatusesUrl = this.appService.apiServer + 'api/fielddata/education-test-statuses';
    this.elementUrl = this.appService.apiServer + 'api/fielddata/element';
    this.elevatedAccessReasonUrl = this.appService.apiServer + 'api/fielddata/elevatedAccessReason';
    this.employerOfRecordTypesUrl = this.appService.apiServer + 'api/fielddata/employerofrecordtypes';
    this.employmentPreventionFactorsUrl = this.appService.apiServer + 'api/fielddata/employment-prevention-factors';
    this.examSubjectsByExamUrl = this.appService.apiServer + 'api/fielddata/exam-subjects/';
    this.gendersUrl = this.appService.apiServer + 'api/fielddata/genders';
    this.goalTypesUrl = this.appService.apiServer + 'api/fielddata/goaltypes';
    this.housingSituationUrl = this.appService.apiServer + 'api/fielddata/housingSituations';
    this.intervalTypesUrl = this.appService.apiServer + 'api/fielddata/intervalTypes';
    this.jobAbsenceReasonsUrl = this.appService.apiServer + 'api/fielddata/absenceReasons';
    this.jobBenefitsOfferedUrl = this.appService.apiServer + 'api/fielddata/jobBenefitsOffered';
    this.jobFoundMethodsUrl = this.appService.apiServer + 'api/fielddata/jobFoundMethods';
    this.jobLeavingReasonsUrl = this.appService.apiServer + 'api/fielddata/leavingReasons';
    this.jobSectorsUrl = this.appService.apiServer + 'api/fielddata/jobSectors';
    this.jobTypesUrl = this.appService.apiServer + 'api/fielddata/jobTypes';
    this.jobsActionsUrl = this.appService.apiServer + 'api/job-action/';
    this.languagesUrl = this.appService.apiServer + 'api/fielddata/languages';
    this.licenseTypesUrl = this.appService.apiServer + 'api/fielddata/licenseTypes';
    this.militaryBranchesUrl = this.appService.apiServer + 'api/fielddata/militaryBranches';
    this.militaryDischargeTypesUrl = this.appService.apiServer + 'api/fielddata/militaryDischargeTypes';
    this.militaryRanksUrl = this.appService.apiServer + 'api/fielddata/militaryRanks';
    this.namesTypesUrl = this.appService.apiServer + 'api/fielddata/name-types';
    this.ncpRelationshipsUrl = this.appService.apiServer + 'api/fielddata/ncpRelationshipTypes';
    this.nrsTypesUrl = this.appService.apiServer + 'api/fielddata/nrs-types';
    this.pinCommentTypesUrl = this.appService.apiServer + 'api/fielddata/pin-comment-types';
    this.participantBarriersUrl = this.appService.apiServer + 'api/fielddata/barrierTypes';
    this.pendingTypesUrl = this.appService.apiServer + 'api/fielddata/pendingTypes';
    this.polarInputUrl = this.appService.apiServer + 'api/fielddata/polarInput';
    this.polarSkipUrl = this.appService.apiServer + 'api/fielddata/yes-no-skip-types';
    this.polarUnknownUrl = this.appService.apiServer + 'api/fielddata/yes-no-unknown-types';
    this.polarRefusedUrl = this.appService.apiServer + 'api/fielddata/yes-no-refused-types';
    this.populationTypesUrl = this.appService.apiServer + 'api/fielddata/population-types/';
    this.rfaProgramsUrl = this.appService.apiServer + 'api/fielddata/rfa-programs';
    this.allRfaProgramsUrl = this.appService.apiServer + 'api/fielddata/all-rfa-programs';
    this.relationshipsUrl = this.appService.apiServer + 'api/fielddata/relationships';
    this.schoolGradesUrl = this.appService.apiServer + 'api/fielddata/schoolGrades';
    this.ssiApplicationStatusesUrl = this.appService.apiServer + 'api/fielddata/ssi-application-statuses';
    this.ssnTypesUrl = this.appService.apiServer + 'api/fielddata/ssn-types';
    this.splTypesUrl = this.appService.apiServer + 'api/fielddata/spl-types';
    this.statesUrl = this.appService.apiServer + 'api/fielddata/states';
    this.suffixTypeUrl = this.appService.apiServer + 'api/fielddata/suffix-types';
    this.symptomsUrl = this.appService.apiServer + 'api/fielddata/symptoms';
    this.transportationTypesUrl = this.appService.apiServer + 'api/fielddata/transportation-types';
    this.tribesUrl = this.appService.apiServer + 'api/fielddata/tribes';
    this.wageTypesUrl = this.appService.apiServer + 'api/fielddata/wageTypes';
    this.workHistoryEmploymentStatusesUrl = this.appService.apiServer + 'api/fielddata/employmentStatusTypes';
    this.workProgramStatusesUrl = this.appService.apiServer + 'api/fielddata/workProgramStatuses';
    this.workProgramsUrl = this.appService.apiServer + 'api/fielddata/workPrograms';
  }

  private getFieldDataCache(url: string): DropDownField[] {
    let prop: string;
    for (const p of Object.keys(this)) {
      if (this[p] === url) {
        prop = p;
        break;
      }
    }
    // Cached is a suffix that is appeneded to property.
    // We do this to give uniqueness to the new prop because we
    // dont want it to get mixed with the URL link or any new
    // props we may add.
    prop = prop.replace('Url', 'Cached');
    return this[prop];
  }

  private setFieldDataCache(url: string, model: DropDownField[]): void {
    for (const p of Object.keys(this)) {
      if (this[p] === url) {
        url = p;
      }
    }
    const p = url.replace('Url', 'Cached');
    this[p] = model;
  }

  // Looks for fieldData in memory before making get request.
  private getFieldData(url: string): Observable<DropDownField[]> {
    if (this.useCache) {
      const cachedFieldData = this.getFieldDataCache(url);
      if (cachedFieldData != null) {
        return of(cachedFieldData);
      }
    }
    return this.http.get(url).pipe(
      map(resp => this.extractFieldData(resp, url)),
      catchError(this.handleError)
    );
  }

  public getFieldDataByField(fieldName: string, optionalParam?: string): Observable<DropDownField[]> {
    let url = '';
    if (optionalParam) {
      url = `${this.baseUrl}${fieldName}/${optionalParam}`;
    } else {
      url = `${this.baseUrl}${fieldName}`;
    }

    if (this.useCache) {
      const cachedFieldData = this.getFieldDataCache(url);
      if (cachedFieldData != null) {
        return of(cachedFieldData);
      }
    }
    return this.http.get(url).pipe(
      map(resp => this.extractFieldData(resp, url)),
      catchError(this.handleError)
    );
  }

  public getMultiFieldData(url: string): Observable<DropDownMultiField[]> {
    return this.http.get(url).pipe(map(this.extractMultiFieldData), catchError(this.handleError));
  }

  private extractFieldData(res: DropDownField[], url: string) {
    const body = res as DropDownField[];
    // We only extract if cache was missed thus we now set the cache.
    if (this.useCache) {
      this.setFieldDataCache(url, body);
    }
    return body || [];
  }

  private extractMultiFieldData(res: DropDownMultiField[]) {
    const body = res as DropDownMultiField[];
    return body || [];
  }

  getAccommodations(): Observable<DropDownField[]> {
    return this.getFieldData(this.accommodationsUrl);
  }

  getActionNeededAssignee(): Observable<DropDownField[]> {
    return this.getFieldData(this.actionNeededAssigneeUrl);
  }

  getAliasTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.aliasTypesUrl);
  }

  getActionNeededPriorities(): Observable<DropDownField[]> {
    return this.getFieldData(this.actionNeededPriorityUrl);
  }
  getCertificateIssuers(): Observable<DropDownField[]> {
    return this.getFieldData(this.certificateIssuersUrl);
  }

  getChildCareArrangements(): Observable<DropDownField[]> {
    return this.getFieldData(this.childCareArrangementsUrl);
  }

  getCompletionReasons(type: string) {
    return this.getFieldData(this.completionReasonsUrl + type);
  }

  getElement(): Observable<DropDownField[]> {
    return this.getFieldData(this.elementUrl);
  }

  getPinCommentTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.pinCommentTypesUrl);
  }

  /**
   *  For disenrollment reasons pass program code.
   *
   * @param {string} type
   * @returns
   * @memberof FieldDataService
   */

  getContactTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.contactTypesUrl);
  }

  getContactIntervals(): Observable<DropDownField[]> {
    return this.getFieldData(this.contactIntervalsUrl);
  }

  getContactNcprIntervals(): Observable<DropDownField[]> {
    return this.getFieldData(this.contactNcprIntervalsUrl);
  }

  getCounties(param = ''): Observable<DropDownField[]> {
    if (!Utilities.isStringEmptyOrNull(param)) {
      return this.getFieldData(this.countiesUrl + '/' + param);
    } else {
      return this.getFieldData(this.countiesUrl);
    }
  }
  /**
   *  Pass numeric to get counties and tribes with their numeber prefixed.
   *
   * @param {string} [param='']
   * @returns {Observable<DropDownField[]>}
   * @memberof FieldDataService
   */
  getCountiesAndTribes(param = ''): Observable<DropDownField[]> {
    if (!Utilities.isStringEmptyOrNull(param)) {
      return this.getFieldData(this.countiesTribesUrl + '/' + param);
    } else {
      return this.getFieldData(this.countiesTribesUrl);
    }
  }

  getCountries(): Observable<DropDownField[]> {
    return this.getFieldData(this.countriesUrl);
  }

  getDegreeTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.degreeTypesUrl);
  }

  getDriverLicenseStates(): Observable<DropDownField[]> {
    return this.getFieldData(this.driverLicenseStatesUrl);
  }

  getDriverLicenseStatuses(): Observable<DropDownField[]> {
    return this.getFieldData(this.driversLicenseInvalidReasonsUrl);
  }

  getEducationDiplomaTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.educationDiplomaTypesUrl);
  }

  getEducationTestStatuses(): Observable<DropDownField[]> {
    return this.getFieldData(this.educationTestStatusesUrl);
  }

  getEducationTestTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.educationTestTypesUrl);
  }
  getEmployerOfRecordTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.employerOfRecordTypesUrl);
  }
  getElevatedAccessReason(): Observable<DropDownField[]> {
    return this.getFieldData(this.elevatedAccessReasonUrl);
  }
  getEmploymentPreventionFactors(): Observable<DropDownField[]> {
    return this.getFieldData(this.employmentPreventionFactorsUrl);
  }

  getGenders(): Observable<DropDownField[]> {
    return this.getFieldData(this.gendersUrl);
  }

  getGoalTypes(enrolledProgramId: number): Observable<DropDownField[]> {
    // Our lookup table maps W-2 to 11 but ids 1-8 are also valid W-2 ids
    // Set the enrolledProgramId to 11 to return Goal Types for W-2
    if (enrolledProgramId > 0 && enrolledProgramId <= 8) {
      enrolledProgramId = 11;
    }

    const enrolledProgramIdString = enrolledProgramId.toString();

    return this.getFieldData(`${this.goalTypesUrl}/${enrolledProgramIdString}`);
  }

  getHousingSituations(): Observable<DropDownField[]> {
    return this.getFieldData(this.housingSituationUrl);
  }

  getIntervalTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.intervalTypesUrl);
  }

  getJobAbsenceReasons(): Observable<DropDownField[]> {
    return this.getFieldData(this.jobAbsenceReasonsUrl);
  }

  getJobFoundMethods(): Observable<DropDownField[]> {
    return this.getFieldData(this.jobFoundMethodsUrl);
  }

  getJobLeavingReasons(): Observable<DropDownField[]> {
    return this.getFieldData(this.jobLeavingReasonsUrl);
  }

  getJobSectors(): Observable<DropDownField[]> {
    return this.getFieldData(this.jobSectorsUrl);
  }

  getJobTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.jobTypesUrl);
  }

  getLanguages(): Observable<DropDownField[]> {
    return this.getFieldData(this.languagesUrl);
  }

  getLicenseTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.licenseTypesUrl);
  }

  getMilitaryBranches(): Observable<DropDownField[]> {
    return this.getFieldData(this.militaryBranchesUrl);
  }

  getMilitaryDischargeTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.militaryDischargeTypesUrl);
  }

  getMilitaryRanks(): Observable<DropDownField[]> {
    return this.getFieldData(this.militaryRanksUrl);
  }

  getNcpRelationships(): Observable<DropDownField[]> {
    return this.getFieldData(this.ncpRelationshipsUrl);
  }

  getNrsTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.nrsTypesUrl);
  }

  getSplTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.splTypesUrl);
  }

  getContractorsByProgram(program: string): Observable<DropDownField[]> {
    return this.getFieldData(this.rfaContractorsUrl + '/' + program);
  }

  getPendingTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.pendingTypesUrl);
  }

  getPolarInput(): Observable<DropDownField[]> {
    return this.getFieldData(this.polarInputUrl);
  }

  getPolarSkip(): Observable<DropDownField[]> {
    return this.getFieldData(this.polarSkipUrl);
  }

  getPolarUnknown(): Observable<DropDownField[]> {
    return this.getFieldData(this.polarUnknownUrl);
  }

  getPolarRefused(): Observable<DropDownField[]> {
    return this.getFieldData(this.polarRefusedUrl);
  }

  getSchoolGrades(): Observable<DropDownField[]> {
    return this.getFieldData(this.schoolGradesUrl);
  }

  getRfaPrograms(): Observable<DropDownField[]> {
    return this.getFieldData(this.rfaProgramsUrl);
  }

  getAllRfaPrograms(): Observable<DropDownField[]> {
    return this.getFieldData(this.allRfaProgramsUrl);
  }

  getRelationships(): Observable<DropDownField[]> {
    return this.getFieldData(this.relationshipsUrl);
  }

  getSsiApplicationStatuses(): Observable<DropDownField[]> {
    return this.getFieldData(this.ssiApplicationStatusesUrl);
  }

  getSsnTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.ssnTypesUrl);
  }

  getNamesTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.namesTypesUrl);
  }

  getStates(): Observable<DropDownField[]> {
    return this.getFieldData(this.statesUrl);
  }

  getSocialRelationships(): Observable<DropDownField[]> {
    return this.getFieldData(this.ncpRelationshipsUrl);
  }

  getSuffixTypes(): Observable<DropDownField[]> {
    return this.getFieldData(this.suffixTypeUrl);
  }

  getSymptoms(): Observable<DropDownField[]> {
    return this.getFieldData(this.symptomsUrl);
  }

  getTribeNames(): Observable<DropDownField[]> {
    return this.getFieldData(this.tribesUrl);
  }

  getWorkHistoryEmploymentStatuses(): Observable<DropDownField[]> {
    return this.getFieldData(this.workHistoryEmploymentStatusesUrl);
  }

  getWorkPrograms(): Observable<DropDownField[]> {
    return this.getFieldData(this.workProgramsUrl);
  }

  getWorkProgramStatuses(): Observable<DropDownField[]> {
    return this.getFieldData(this.workProgramStatusesUrl);
  }

  // getPriorities(): Observable<DropDownField[]> {
  //   return this.getFieldData(this.prioritiesUrl);
  // }
  getActionNeededPages(): Observable<DropDownField[]> {
    return this.getFieldData(this.actionNeededPagesUrl);
  }

  getWageTypes(): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.wageTypesUrl);
  }

  getJobBenefitsOffered(): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.jobBenefitsOfferedUrl);
  }

  getParticipantBarriers(): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.participantBarriersUrl);
  }

  getPopulationGroupsByProgramOrContractor(type: string): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.populationTypesUrl + type);
  }

  getTransportationTypes(): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.transportationTypesUrl);
  }

  getExamSubjectsByExamUrl(type: string): Observable<DropDownMultiField[]> {
    return this.getMultiFieldData(this.examSubjectsByExamUrl + type);
  }

  getAllActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('all');
  }

  getChildYouthSupportsActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('child-youth-supports');
  }

  getHousingActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('housing');
  }

  getLegaIssuesActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('legal-issues');
  }

  getWorkHistoryActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('work-history');
  }

  getFamilyBarriersActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('family-barriers');
  }

  getNcpActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('non-custodial-parents');
  }

  getTransportationActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('transportation');
  }
  getJobReadinessActionNeeded(): Observable<DropDownField[]> {
    return this.getActionNeeded('job-readiness');
  }

  getActionNeededByPage(pageName: string): Observable<DropDownField[]> {
    return this.getActionNeeded(pageName);
  }

  // TODO: Use Caching; Instead of looking at prop name lets start looking at the url instead.
  public getDeleteReasons(type: string): Observable<DropDownMultiField[]> {
    const url = this.deleteReasonsUrl + type;

    return this.http.get(url).pipe(map(this.extractMultiFieldData), catchError(this.handleError));
  }

  private getActionNeeded(section: string) {
    // TODO: Finish Action Needed change.
    // return this.getFieldData(this.actionNeededUrl);
    //  const actionNeededs: ActionNeeded[] = [];
    // return of(actionNeededs);
    return this.http.get(this.actionNeededUrl + section).pipe(map(this.extractActionNeeded), catchError(this.handleError));
  }

  private extractActionNeeded(res: DropDownField[]) {
    const body = res as DropDownField[];
    const actionNeededs: DropDownField[] = [];
    for (const an of body) {
      actionNeededs.push(an);
    }
    return actionNeededs || null;
  }

  private handleError(error: any) {
    // In a real world app, we might use a remote logging infrastructure
    // We'd also dig deeper into the error to get a better message
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
    console.error(errMsg); // log to console instead
    return throwError(errMsg);
  }
}
