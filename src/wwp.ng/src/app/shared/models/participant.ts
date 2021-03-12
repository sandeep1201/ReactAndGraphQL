import { EARequest } from './../../features-modules/emergency-assistance/models/ea-request.model';
// tslint:disable: no-conditional-assignment
// tslint:disable: only-arrow-functions
// tslint:disable: no-redundant-jsdoc
// tslint:disable: adjacent-overload-signatures
// tslint:disable: variable-name
// tslint:disable: no-use-before-declare
//import { baseBuildCommandOptions } from '@angular/cli/commands/build';
import { AppService } from 'src/app/core/services/app.service';
import { EnrolledProgram } from './enrolled-program.model';
import { EnrolledProgramCode } from '../enums/enrolled-program-code.enum';
import { User } from './user';
import { Utilities } from '../utilities';

import * as moment from 'moment';
import * as _ from 'lodash';

export class Participant {
  public id: number;
  public pin: string;
  public firstName: string;
  public lastName: string;
  public middleInitialName: string;
  public nameSuffix: string;
  public dateOfBirth: string;
  public genderIndicator: string;
  public enrolledDate: string;
  public disenrolledDate: string;
  public address: string;
  public groupOrder: number;
  public sortOrder: number;
  public hasConfidentialAccess: boolean;
  public isConfidentialCase: boolean;
  public countyOfResidenceId: number;
  public programs: EnrolledProgram[];
  public mciId: number;
  public hasBeenThroughClientReg: boolean;
  public cutOverDate: string;
  public totalLifeTimeSubsidizedHours: string;
  public totalLifeTimeHoursDate: string;
  public eaRequests: EARequest[];
  public is60DaysVerified: boolean;

  // sorting based on the sortOrder and groupOrder provided by BE
  public static sortBySortAndGroupOrder(participants: Participant[], order?: string): any {
    let sorted: Participant[];
    if (order === 'desc') {
      sorted = _.orderBy(participants, ['groupOrder', 'sortOrder'], ['asc', 'desc']);
    } else if ((order = 'asc')) {
      sorted = _.orderBy(participants, ['groupOrder', 'sortOrder'], ['asc', 'asc']);
    } else {
      sorted = _.orderBy(participants, ['groupOrder', 'sortOrder'], ['asc', 'asc']);
    }
    return sorted;
  }

  public static sortByDate(participants) {
    participants.sort(function(a, b) {
      const aa: any = new Date(a.programs[0].statusDate);
      const bb: any = new Date(b.programs[0].statusDate);

      if (aa !== bb) {
        if (aa > bb) {
          return -1;
        }
        if (aa < bb) {
          return 1;
        }
      }
      return aa - bb;
    });
  }
  public static sortRefPart(participants: Participant[]): Participant[] {
    participants.forEach(function(participant) {
      if (participant.programs.length === 1 && participant.programs[0].isPending) {
        participant.programs[0].status = 'Pending';
      } else if (participant.programs.length === 1 && participant.programs[0].isTransfer) {
        participant.programs[0].status = 'Transfers';
      } else {
        return participants;
      }
    });

    return this.sortBySortAndGroupOrder(participants);
  }

  get displayName(): string {
    if (this.firstName != null) {
      return Utilities.formatDisplayPersonName(this.firstName, this.middleInitialName, this.lastName, this.nameSuffix, true);
    }
  }
  get firstAndLastName(): string {
    return this.firstName + ' ' + this.middleInitialName + ' ' + this.lastName;
  }

  get pinStringified(): string {
    if (this.pin == null) {
      return '';
    }
    return this.pin.toString();
  }

  public static clone(input: any, instance: Participant) {
    instance.id = input.id;
    instance.pin = input.formatPin(input.pin);
    instance.firstName = input.firstName;
    instance.lastName = input.lastName;
    instance.middleInitialName = input.middleInitialName;
    instance.nameSuffix = input.nameSuffix;
    instance.dateOfBirth = input.dateOfBirth;
    instance.genderIndicator = input.genderIndicator;
    instance.enrolledDate = input.enrolledDate;
    instance.disenrolledDate = input.disenrolledDate;
    instance.address = input.address;
    instance.hasConfidentialAccess = input.hasConfidentialAccess;
    instance.isConfidentialCase = input.isConfidentialCase;
    instance.countyOfResidenceId = input.countyOfResidenceId;
    instance.programs = Utilities.deserilizeChildren(input.programs, EnrolledProgram, 0);
    instance.mciId = input.mciId;
    instance.hasBeenThroughClientReg = input.hasBeenThroughClientReg;
    instance.cutOverDate = input.cutOverDate;
    instance.totalLifeTimeSubsidizedHours = input.totalLifeTimeSubsidizedHours;
    instance.totalLifeTimeHoursDate = input.totalLifeTimeHoursDate;
    instance.eaRequests = Utilities.deserilizeChildren(input.eaRequests, EARequest, 0);
    instance.is60DaysVerified = input.is60DaysVerified;
  }

  public clone() {
    const cloneObj = new Participant();
    cloneObj.id = this.id;
    cloneObj.pin = this.pin;
    cloneObj.firstName = this.firstName;
    cloneObj.lastName = this.lastName;
    cloneObj.middleInitialName = this.middleInitialName;
    cloneObj.nameSuffix = this.nameSuffix;
    cloneObj.dateOfBirth = this.dateOfBirth;
    cloneObj.genderIndicator = this.genderIndicator;
    cloneObj.enrolledDate = this.enrolledDate;
    cloneObj.disenrolledDate = this.disenrolledDate;
    cloneObj.address = this.address;
    cloneObj.sortOrder = this.sortOrder;
    cloneObj.groupOrder = this.groupOrder;
    cloneObj.hasConfidentialAccess = this.hasConfidentialAccess;
    cloneObj.isConfidentialCase = this.isConfidentialCase;
    cloneObj.countyOfResidenceId = this.countyOfResidenceId;
    cloneObj.programs = Array.from(this.programs);
    cloneObj.mciId = this.mciId;
    cloneObj.hasBeenThroughClientReg = this.hasBeenThroughClientReg;
    cloneObj.cutOverDate = this.cutOverDate;
    cloneObj.totalLifeTimeSubsidizedHours = this.totalLifeTimeSubsidizedHours;
    cloneObj.totalLifeTimeHoursDate = this.totalLifeTimeHoursDate;
    cloneObj.eaRequests = Array.from(this.eaRequests);
    cloneObj.is60DaysVerified = this.is60DaysVerified;

    return cloneObj;
  }

  deserialize(input: any) {
    this.id = input.id;
    this.pin = this.formatPin(input.pin);
    this.firstName = input.firstName;
    this.lastName = input.lastName;
    this.middleInitialName = input.middleInitialName;
    this.nameSuffix = input.suffixName;
    this.dateOfBirth = input.dateOfBirth;
    this.genderIndicator = input.genderIndicator;
    this.enrolledDate = input.enrolledDate;
    this.disenrolledDate = input.disenrolledDate;
    this.address = input.address;
    this.sortOrder = input.sortOrder;
    this.groupOrder = input.groupOrder;
    this.hasConfidentialAccess = input.hasConfidentialAccess;
    this.isConfidentialCase = input.isConfidentialCase;
    this.countyOfResidenceId = input.countyOfResidenceId;
    this.programs = Utilities.deserilizeChildren(input.programs, EnrolledProgram, 0);
    this.mciId = input.mciId;
    this.hasBeenThroughClientReg = input.hasBeenThroughClientReg;
    this.cutOverDate = input.cutOverDate;
    this.totalLifeTimeSubsidizedHours = input.totalLifeTimeSubsidizedHours;
    this.totalLifeTimeHoursDate = input.totalLifeTimeHoursDate;
    this.eaRequests = Utilities.deserilizeChildren(input.eaRequests, EARequest, 0);
    this.is60DaysVerified = input.is60DaysVerified;

    return this;
  }
  /**
   * Deprecated Method due to co enrollment.
   *
   * @memberof Participant
   */
  get currentEnrolledProgram() {
    if (this.programs != null) {
      return this.programs[0];
    } else {
      this.programs = [];
      this.programs[0] = new EnrolledProgram();
    }
  }

  programById(id: number) {
    return this.programs.find(x => x.id === id);
  }

  programByCode(code: string) {
    return this.programs.find(x => x.programCode === code);
  }

  /**
   * Deprecated Method due to not being agency based.
   *
   * @memberof Participant
   */

  get enrolledTmjProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepTmjEnrolled);
    }
  }

  get enrolledLFProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepLfEnrolled);
    }
  }

  get enrolledTJProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepTjEnrolled);
    }
  }

  get enrolledW2Program(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepW2Enrolled);
    }
  }

  get enrolledCFProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepCfEnrolled);
    }
  }
  get enrolledFCDPProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepFcdpEnrolled);
    }
  }

  get referredTmjProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepTmjReferred);
    }
  }

  get referredLFProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepLfReferred);
    }
  }

  get referredTJProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepTjReferred);
    }
  }

  get referredW2Program(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.isThisPepW2Referred);
    }
  }

  get referredCFProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.IsThisPepCfReferred);
    }
  }
  get referredFCDPProgram(): EnrolledProgram {
    if (this.programs == null) {
      return;
    } else {
      return this.programs.find(this.IsThisPepFcdpReferred);
    }
  }

  // These are callbacks.
  private isThisPepTmjEnrolled(ep: EnrolledProgram) {
    return ep.isTmj && ep.isEnrolled;
  }
  private isThisPepTjEnrolled(ep: EnrolledProgram) {
    return ep.isTj && ep.isEnrolled;
  }
  private isThisPepLfEnrolled(ep: EnrolledProgram) {
    return ep.isLF && ep.isEnrolled;
  }
  private isThisPepW2Enrolled(ep: EnrolledProgram) {
    return ep.isW2 && ep.isEnrolled;
  }
  private isThisPepCfEnrolled(ep: EnrolledProgram) {
    return ep.isCF && ep.isEnrolled;
  }
  private isThisPepFcdpEnrolled(ep: EnrolledProgram) {
    return ep.isFCDP && ep.isEnrolled;
  }

  // These are callbacks.
  private isThisPepTmjReferred(ep: EnrolledProgram) {
    return ep.isTmj && ep.isReferred;
  }
  private isThisPepTjReferred(ep: EnrolledProgram) {
    return ep.isTj && ep.isReferred;
  }
  private isThisPepLfReferred(ep: EnrolledProgram) {
    return ep.isLF && ep.isReferred;
  }
  private isThisPepW2Referred(ep: EnrolledProgram) {
    return ep.isW2 && ep.isReferred;
  }
  private IsThisPepCfReferred(ep: EnrolledProgram) {
    return ep.isCF && ep.isReferred;
  }
  private IsThisPepFcdpReferred(ep: EnrolledProgram) {
    return ep.isFCDP && ep.isReferred;
  }
  /**
   * A worker's available Peps are agency based thus we have this method.
   *
   * @param {string} agencyCode
   * @returns {EnrolledProgram[]}
   * @memberof Participant
   */
  getCurrentEnrolledProgramsByAgency(agencyCode: string): EnrolledProgram[] {
    if (this.programs == null || this.programs.length === 0) {
      return this.programs;
    }

    const eps = EnrolledProgram.filterByStatus(this.programs, EnrolledProgramStatus.enrolled.toLowerCase());
    return EnrolledProgram.filterByAgencyCode(eps, agencyCode);
  }

  public getMostRecentProgramsByAgency(agencyCode: string): EnrolledProgram[] {
    if (this.programs == null || this.programs.length === 0) {
      return this.programs;
    }

    return EnrolledProgram.filterByAgencyCode(this.programs, agencyCode);
  }

  getMostRecentW2ProgramByAgency(agencyCode: string): EnrolledProgram {
    let programs = this.getMostRecentProgramsByAgency(agencyCode);

    if (programs == null || programs.length === 0) {
      return null;
    }

    programs = programs.filter(p => p.isW2);
    if (programs == null || programs.length === 0) {
      return null;
    }

    // There should only be one, so return the 0 indexed item.
    return programs[0];
  }

  getMostRecentW2Program(): EnrolledProgram {
    if (this.programs == null || this.programs.length === 0) {
      return null;
    }

    const w2Programs = this.programs.filter(p => p.isW2);
    if (w2Programs == null || w2Programs.length === 0) {
      return null;
    }

    // There should only be one, so return the 0 indexed item.
    return w2Programs[0];
  }

  isEnrolledInCF(): boolean {
    if (this.programs == null || this.programs.length === 0) {
      return false;
    }

    const filteredPrograms = EnrolledProgram.filterByStatus(this.cfPrograms, EnrolledProgramStatus.enrolled);

    return filteredPrograms != null && filteredPrograms.length > 0;
  }

  getMostRecentW2ProgramsUserIsAssignedTo(user: User, appService: AppService): EnrolledProgram[] {
    let mostRecentPrograms = this.getMostRecentProgramsUserHasAccessTo(user, appService);

    if (mostRecentPrograms != null && mostRecentPrograms.length > 0) {
      mostRecentPrograms = appService.filterProgramsForUserAssigned(mostRecentPrograms);
      if (mostRecentPrograms != null && mostRecentPrograms.length > 0) {
        mostRecentPrograms = mostRecentPrograms.filter(p => p.isW2);
      }
    }

    return mostRecentPrograms;
  }

  getCurrentEnrolledProgramsUserHasAccessTo(user: User, appService: AppService): EnrolledProgram[] {
    let currentEnrolledPrograms = this.getCurrentEnrolledProgramsByAgency(user.agencyCode);

    if (currentEnrolledPrograms != null && currentEnrolledPrograms.length > 0) {
      currentEnrolledPrograms = appService.filterProgramsForUserAuthorized<EnrolledProgram>(currentEnrolledPrograms);
    }

    return currentEnrolledPrograms;
  }
  getMostRecentReferredProgramsUserHasAccessTo(user: User, appService: AppService): EnrolledProgram[] {
    let referredPrograms = this.getMostRecentProgramsByAgency(user.agencyCode);

    if (referredPrograms != null && referredPrograms.length > 0) {
      referredPrograms = appService.filterProgramsForUserAuthorized<EnrolledProgram>(referredPrograms);
      referredPrograms = EnrolledProgram.filterByStatus(referredPrograms, EnrolledProgramStatus.referred.toLowerCase());
    }

    return referredPrograms;
  }

  /**
* A worker's available Peps are agency based thus we have this method.
*
* @param {User} user
  @param {AppService} appService
* @returns {EnrolledProgram[]}
* @memberof Participant
*/
  getMostRecentProgramsUserHasAccessTo(user: User, appService: AppService): EnrolledProgram[] {
    let mostRecentEnrolledPrograms = this.getMostRecentProgramsByAgency(user.agencyCode);

    if (mostRecentEnrolledPrograms != null && mostRecentEnrolledPrograms.length > 0) {
      mostRecentEnrolledPrograms = appService.filterProgramsForUserAuthorized<EnrolledProgram>(mostRecentEnrolledPrograms);
    }

    return mostRecentEnrolledPrograms;
  }

  /**
   * A worker's available Peps are agency based thus we have this method.
   *
   * @param {string} agencyCode
   * @returns {EnrolledProgram[]}
   * @memberof Participant
   */
  getCurrentReferredProgramsByAgency(agencyCode: string): EnrolledProgram[] {
    let peps = EnrolledProgram.filterByStatus(this.programs, EnrolledProgramStatus.referred.toLowerCase());
    peps = EnrolledProgram.filterByAgencyCode(peps, agencyCode);
    return peps;
  }

  /**
   * Returns Transferable peps based on a user's agency code.
   *
   * @param {string} agencyCode
   * @returns {EnrolledProgram[]}
   * @memberof Participant
   */
  getTransferableProgramsByAgency(agencyCode: string): EnrolledProgram[] {
    const enrolledPeps = this.getCurrentEnrolledProgramsByAgency(agencyCode);
    return enrolledPeps;
  }
  // TODO: remove this method.
  currentReferredProgramByName(name: string) {
    if (this.programs != null) {
      for (const pro of this.programs) {
        if (pro.programCode === name && pro.status.toLowerCase() === EnrolledProgramStatus.referred.toString().toLowerCase()) {
          return pro;
        }
      }
    } else {
      this.programs = [];
      this.programs[0] = new EnrolledProgram();
    }
  }

  /**
   *  // TODO: remove this method.
   * Returns the latest enrolled program by sorting enrollment dates.
   *
   * @param {string} programName
   * @returns {EnrolledProgram}
   * @memberof Participant
   */
  currentEnrolledProgramByName(programName: string): EnrolledProgram {
    // For some reason if we dont have programs which can happen.
    if (this.programs == null) {
      return;
    }

    // Get all the programs by name in the pep array.
    const typeOfPeps = EnrolledProgram.filterByName(this.programs, programName);

    // From the array above get enrolled programs only.
    const enrolledPeps = EnrolledProgram.filterByStatus(typeOfPeps, EnrolledProgramStatus.enrolled);

    // Sort by date.
    enrolledPeps.sort(function(a, b) {
      // Turn your strings into dates, and then subtract them
      // to get a value that is either negative, positive, or zero.
      return new Date(b.enrollmentDate).getTime() - new Date(a.enrollmentDate).getTime();
    });

    return enrolledPeps[0];
  }

  get anyProgramsReferred() {
    if (this.programs === null) {
      return null;
    }
    for (const pro of this.programs) {
      if (pro.status.toLocaleLowerCase() === EnrolledProgramStatus.referred) {
        return true;
      }
    }

    return false;
  }

  get anyProgramsEnrolled() {
    if (this.programs === null) {
      return null;
    }
    for (const pro of this.programs) {
      if (pro.status.toLocaleLowerCase() === EnrolledProgramStatus.enrolled) {
        return true;
      }
    }

    return false;
  }

  set currentEnrolledProgram(value: EnrolledProgram) {
    this.programs[0] = value;
  }

  formatPin(pin: any) {
    if (pin == null) {
      return '';
    }

    let str = pin.toString();
    if (str.length >= 10) {
      return str;
    }

    str = '000000000' + str;
    return str.substring(str.length - 10);
  }

  public isCurrentlyEnrolled(agencyCode: string): boolean {
    const eps = this.getCurrentEnrolledProgramsByAgency(agencyCode);
    return eps.length > 0;
  }

  get participantAge(): number {
    const dob = moment(this.dateOfBirth, moment.ISO_8601);
    if (dob.isValid()) {
      return Utilities.currentDate.diff(dob, 'years');
    } else {
      return 0;
    }
  }

  get nontmjTjCfPrograms(): EnrolledProgram[] {
    if (this.programs == null) {
      return [];
    }

    const filteredList = [];
    for (const p of this.programs) {
      if (!p.isCFTmjTJProgram) {
        filteredList.push(p);
      }
    }
    return filteredList;
  }

  get tmjTjCfPrograms(): EnrolledProgram[] {
    if (this.programs == null) {
      return [];
    }

    const filteredList = [];
    for (const p of this.programs) {
      if (p.isCFTmjTJProgram) {
        filteredList.push(p);
      }
    }
    return filteredList;
  }
  // These methods return all the instances of the programs i.e. enrolled, referred and disenrolled
  get tmjPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.tmj);
  }

  get tjPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.tj);
  }

  get cfPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.cf);
  }
  get wwPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.ww);
  }
  get lfPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.lf);
  }
  get fcdpPrograms(): EnrolledProgram[] {
    return EnrolledProgram.filterByCode(this.programs, EnrolledProgramCode.fcdp);
  }

  // For now we use to see timelimits access.
  public isParticipantServedByWorker(wiuid: string) {
    let somePepServedByWorker = false;
    if (this.programs == null || wiuid == null || wiuid.trim() === '') {
      return somePepServedByWorker;
    }

    for (const x of this.programs) {
      if (x.assignedWorker != null && x.assignedWorker.wiuid != null && x.assignedWorker.wiuid.trim().toLowerCase() === wiuid.trim().toLowerCase()) {
        somePepServedByWorker = true;
        break;
      }
    }

    return somePepServedByWorker;
  }

  // For now we use to see timelimits access.
  public isParticipantServedByAgency(agencyCode: string) {
    let somePepContainsAgency = false;
    if (this.programs == null || (agencyCode && agencyCode.trim() === '')) {
      return somePepContainsAgency;
    }

    for (const x of this.programs) {
      if (x.agencyCode && x.agencyCode.trim().toLowerCase() === agencyCode.trim().toLowerCase()) {
        somePepContainsAgency = true;
        break;
      }
    }

    return somePepContainsAgency;
  }
}
export class ParticipantDetails {
  officeTransferId: number;
  officeTransferNumber: number;
  addressInfo: AddressInfo;
  basicInfo: BasicInfo;
  enrolledProgramInfo: EnrolledProgramInfo;
  officeCountyInfo: OfficeCountyInfo;
  relatedPersons: RelatedPerson[];
  cwwTransferDetails: CwwTransferDetails;
  mostRecentFEPFromDB2_Result: MostRecentFepFromDB2Details;
  w2EligibilityInfo: W2EligibilityInfo;
  otherDemographicInformation: OtherDemographicInformation;
}

export class AddressInfo {
  addressLine1: string;
  addressLine2: string;
  alternateAddress1: string;
  alternateAddress2: string;
  alternateCity: string;
  alternatePrimaryPhoneNumber: string;
  alternateState: string;
  alternateZipCode: string;
  city: string;
  emailAddress: string;
  livingArrangement: string;
  primaryPhoneNumber: string;
  state: string;
  zipCode: string;
}

export class BasicInfo {
  caseNumber: number;
  countryOfOrigin: string;
  dateOfBirth: string;
  age: number;
  isHispanic: boolean;
  firstName: string;
  middleInitialName: string;
  lastName: string;
  suffixName: string;
  genderIndicator: string;
  mfWorkerId: number;
  pinNumber: number;
  raceCode: string;
  refugeeCode: string;
  refugeeEntryDate: string;
}

export class EnrolledProgramInfo {
  id: number;
  assignedWorker: string;
  canDisenroll: boolean;
  disenrollmentDate: string;
  enrolledProgramId: number;
  enrollmentDate: string;
  officeCounty: string;
  participantId: number;
  programCode: string;
  subProgramCode: string;
  referralDate: string;
  rfaNumber: number;
  status: string;
  statusDate: string;
}
export class OfficeCountyInfo {
  countyNumber: number;
  officeNumber: number;
  wpGeoArea: number;
}

export class W2EligibilityInfo {
  agFailureReasonCode1: string;
  agFailureReasonCode2: string;
  agFailureReasonCode3: string;
  agSequenceNumber: number;
  agStatuseCode: string;
  ccAgOpen: string;
  daysInPlacement: number;
  eligibilityBeginDate: string;
  eligibilityEndDate: string;
  epReviewDueDate: string;
  fpwAgOpen: string;
  fsAgOpen: string;
  maAgOpen: string;
  paymentBeginDate: string;
  paymentEndDate: string;
  placementCode: string;
  stateLifeTimeLimit: string;
  fsetStatus: string;
  childSupportStatus: boolean;
  twoParentStatus: boolean;
  learnFareStatus: boolean;
  reviewDueDate: string;
  moreThanSixIndv: boolean;
}

export class OtherDemographicInformation {
  isInterpreterNeeded: boolean;
  homeLessIndicator: boolean;
  homeLanguageName: string;
  hasAlias: boolean;
  isRufugee: boolean;
  monthOfEntry: string;
  countryOfOriginName: string;
  isTribalMember: boolean;
  tribeId: number;
  tribeName: string;
  tribeDetails: string;
  countyOfResidenceName: string;
  householdAddress: PlaceAddress;
  mailingAddress: PlaceAddress;
  primaryPhoneNumber: number;
  secondaryPhoneNumber: number;
  emailAddress: string;
}
export class PlaceAddress {
  streetAddress: string;
  aptUnit: number;
  city: string;
  state: string;
  country: string;
  zipCode: number;
}
export class RelatedPerson {
  pin: number;
  firstName: string;
  relationship: string;
  lastName: string;
  dateOfBirth: string;
  age: number;
}
export class CwwTransferDetails {
  fepOutOfSync: boolean;
  newFepId: string;
}
export class MostRecentFepFromDB2Details {
  mostRecentMFFepId: string;
  id: number;
}

enum EnrolledProgramStatus {
  enrolled = 'enrolled',
  disenrolled = 'disenrolled',
  referred = 'referred'
}
