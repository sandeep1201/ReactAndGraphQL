import { AppService } from './../../../core/services/app.service';
import { Component, OnInit, OnDestroy, Output, Input, EventEmitter, ComponentRef } from '@angular/core';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { IMultiSelectOption } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { OfficeService } from '../../../shared/services/office.service';
import { PadPipe } from '../../../shared/pipes/pad.pipe';
import { Participant } from '../../../shared/models/participant';
import { ParticipantService } from '../../../shared/services/participant.service';
import { RFAProgram } from '../../../shared/models/rfa.model';
import { RfaChild } from '../../../shared/models/rfa-child.model';
import { RfaEligibility } from '../../../shared/models/server-message.model';
import { RfaService } from '../../../shared/services/rfa.service';
import { Subscription, Observable, forkJoin, of } from 'rxjs';
import { tap, take, concatMap } from 'rxjs/operators';
import { UserPreferencesService } from '../../../shared/services/user-preferences.service';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModalService } from 'src/app/core/modal/modal.service';
import { WarningModalComponent } from '../../../shared/components/warning-modal/warning-modal.component';
import { Office } from '../../../shared/models/office';
import { WhyReason } from '../../../shared/models/why-reasons.model';

import * as moment from 'moment';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { EnrolledProgramStatus } from '../../../shared/enums/enrolled-program-status.enum';
import * as _ from 'lodash';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';

@Component({
  selector: 'app-rfa-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [FieldDataService, OfficeService]
})
export class RfaEditComponent implements OnInit, OnDestroy {
  fcdpCourtOrderId = 0;
  @Output() close = new EventEmitter();
  @Input() id: number;
  @Input() pin: number;
  public fcdpCourtOrderDrop: DropDownField[] = [];
  public isSaving = false;
  public isCollapsed = false;
  public hadSaveError = false;
  public contractorDrop: DropDownField[] = [];
  public countyDrop: DropDownField[] = [];
  public countyTribeDrop: DropDownField[] = [];
  public genderDrop: DropDownField[] = [];
  public programDrop: DropDownField[] = [];
  public saveTabs: DropDownField[] = [];
  public wpOfficesDrop: DropDownField[] = [];
  public populationGroupDrops: IMultiSelectOption[] = [];
  public rfaEligibility: RfaEligibility;
  public model: RFAProgram = new RFAProgram();
  public originalModel: RFAProgram;
  public participant: Participant;
  public hasLoaded = false;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public hasEligibilityBeenVerified = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSectionValid: boolean;
  public _isFormDisplayed = false;
  public precheck: WhyReason;
  public checkPopulationTypeEligibility = false;

  public validationMsgs: WhyReason;
  public isProgramDisabled = false;
  public hasTriedSaving = false;

  get isFormDisplayed(): boolean {
    if (this._isFormDisplayed && this.model.isInProgressStatus) {
      return true;
    } else if (this._isFormDisplayed && this.precheck != null && this.precheck.status === true) {
      return true;
    } else {
      return false;
    }
  }

  set isFormDisplayed(value: boolean) {
    this._isFormDisplayed = value;
  }

  private eligibilityModelString: string;
  private conSub: Subscription;
  private wpSub: Subscription;
  private couSub: Subscription;
  private couTriSub: Subscription;
  private genSub: Subscription;
  private popSub: Subscription;
  private proSub: Subscription;
  private preCheckSub: Subscription;
  private isSectionModified = false;
  private workProgramOffices: Office[];
  private defaultWorkProgramOfficeId: number;
  private tempModalRef: ComponentRef<WarningModalComponent>;
  private defaultOffice: Office;
  contractorName: string;

  constructor(
    private fdService: FieldDataService,
    private modalService: ModalService,
    private userPreferencesService: UserPreferencesService,
    private rfaService: RfaService,
    private appService: AppService,
    private offService: OfficeService,
    private participantService: ParticipantService
  ) {}

  ngOnInit() {
    // Lets load all the field data first so that we start acting on the model right when it loads.
    forkJoin(
      this.fdService.getRfaPrograms().pipe(take(1)),
      this.fdService.getCounties('numeric').pipe(take(1)),
      this.fdService.getGenders().pipe(take(1)),
      this.fdService.getCountiesAndTribes('numeric').pipe(take(1)),
      this.participantService.getParticipant(this.pin.toString()).pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initRfaProgramsDrop(results[0]);
        this.initCountiesDrop(results[1]);
        this.initGenderDrop(results[2]);
        this.initCountiesAndTribesDrop(results[3]);
        this.initParticipant(results[4]);
        this.loadRfaProgramModel();
      });
    this.fcdpCourtOrderDrop = [
      {
        id: 1,
        name: 'Court Ordered'
      },
      {
        id: 2,
        name: 'Voluntary'
      }
    ];
    this.isCollapsed = this.userPreferencesService.isRfaSidebarCollapsed;
  }

  private loadWorkProgramOffices(): void {
    const foundProgram = this.programDrop.find(x => x.id === this.model.programId);
    if (foundProgram) {
      const name = foundProgram.name.replace(/\s/g, '').toLowerCase();
      const query = EnrolledProgram.getProgramCodeByName(name);
      this.wpSub = this.offService.getOfficesByProgramAndWIUID(query, this.appService.user.wiuid).subscribe(data => {
        this.initWorkProgramOffices(data);
        // If the form is displayed lets also set the default office. We do not set default from server on cf.
        if (this.isFormDisplayed && !this.model.isCFProgram) {
          this.loadDefaultWorkProgramOffice();
        } else if (this.isFormDisplayed && this.model.isCFProgram) {
          // For Children first dont load a default office. Just set the default when there is only 1.
          // this.setDefaultWPOffice();
        }
      });
    }
  }

  private loadContractors(): void {
    const foundProgram = this.programDrop.find(x => x.id === this.model.programId);
    if (foundProgram) {
      const name = foundProgram.name.replace(/\s/g, '').toLowerCase();
      const query = EnrolledProgram.getProgramCodeByName(name);
      this.conSub = this.fdService.getContractorsByProgram(query).subscribe(data => this.initContractorDrop(data));
    }
  }

  public programOrCountyChange() {
    this.validationMsgs = null;
    this.preCheckSub = this.preCheckRfaCreation().subscribe(data => {
      if (this.precheck.status === true) {
        this.initFormFromProgram();
      }
    });
  }

  preCheckRfaCreation(): Observable<WhyReason> {
    this.precheck = null;
    if (this.model != null && this.model.programId != null && (!this.model.isInProgressStatus || this.model.id === 0)) {
      return this.rfaService.preCheckRfaCreation(this.pin, this.model).pipe(
        tap(data => {
          this.precheck = data;
          return data;
        })
      );
    } else {
      // We have an in-progress rfa thus it has passed and the type is locked.
      this.precheck = new WhyReason();
      this.precheck.status = true;
      return of(this.precheck);
    }
  }

  getProgramCodeFromName(name: string) {
    switch (name) {
      case 'transformmilwaukeejobs': {
        return 'tmj';
      }
      case 'transitionaljobs': {
        return 'tj';
      }
      case 'childrenfirst': {
        return 'cf';
      }
      default: {
        break;
      }
    }
  }

  get getProgramCodeFromCurrentModel() {
    if (this.model.isTmjProgram) {
      return 'tmj';
    } else if (this.model.isTJProgram) {
      return 'tj';
    } else if (this.model.isCFProgram) {
      return 'cf';
    }
  }

  private loadRfaProgramModel(): void {
    // New Rfa.
    if (+this.id === 0) {
      this.initRfaProgramModel(RFAProgram.create());
      this.hasLoaded = true;
    } else {
      // Loading an old Rfa.
      this.rfaService.setPin(this.pin.toString());
      this.rfaService.getRfaById(this.id.toString()).subscribe(data => {
        this.initRfaProgramModel(data);
        this._isFormDisplayed = this.checkIfFormShouldBeDisplayed();
        if (!data.isCFProgram) {
          this.loadContractors();
        }

        this.loadWorkProgramOffices();

        this.hasLoaded = true;
        this.isProgramDisabled = true;
      });
    }
  }

  // New RFA which is program and county dependent.
  public initFormFromProgram(): void {
    if (this.precheck == null || this.precheck.status !== true || !this.checkIfFormShouldBeDisplayed()) {
      return;
    }
    this._isFormDisplayed = this.checkIfFormShouldBeDisplayed();
    this.model.workProgramOfficeId = 0;
    const foundProgram = this.programDrop.find(x => x.id === this.model.programId);
    if (foundProgram) {
      const name = foundProgram.name.replace(/\s/g, '').toLowerCase();
      const programCode = EnrolledProgram.getProgramCodeByName(name);

      if (!this.model.isCFProgram && !this.model.isFCDPProgram) {
        forkJoin(
          this.offService.getOfficesByProgramAndWIUID(programCode, this.appService.user.wiuid).pipe(take(1)),
          this.offService.getOfficeForProgramAndCounty(programCode, this.participant.countyOfResidenceId).pipe(take(1))
        )
          .pipe(take(1))
          .subscribe(results => {
            this.initWorkProgramOffices(results[0]);
            this.initDefaultWorkOffice(results[1]);
          });
      } else {
        forkJoin(this.offService.getOfficesByProgramAndWIUID(programCode, this.appService.user.wiuid).pipe(take(1)))
          .pipe(take(1))
          .subscribe(results => {
            this.initWorkProgramOffices(results[0]);
            this.setDefaultWPOffice();
          });
      }
    }

    this.initSaveTabs();

    if (!this.model.isCFProgram) {
      this.loadContractors();
    }
  }

  public validateRfa(): void {
    if (this.model == null || this.model.workProgramOfficeId == null) {
      return;
    } else {
      this.model.workProgramOfficeNumber = this.workProgramOffices.find(x => x.id === this.model.workProgramOfficeId).officeNumber;
      this.rfaService.validateRfa(this.pin, this.model).subscribe(data => this.initValidationStatus(data));
    }
  }

  initValidationStatus(data): void {
    this.validationMsgs = data;
  }

  private loadDefaultWorkProgramOffice(): void {
    const program = this.programDrop.find(x => x.id === this.model.programId);
    this.offService
      .getOfficeForProgramAndCounty(EnrolledProgram.getProgramCodeByName(program.name), this.participant.countyOfResidenceId)
      .subscribe(data => this.initDefaultWorkOffice(data));
  }

  private setDefaultWPOffice(): void {
    this.model.workProgramOfficeId = null;
    this.originalModel.workProgramOfficeId = null;
    if (this.model.isCFProgram || this.model.isFCDPProgram) {
      if (this.wpOfficesDrop != null && this.wpOfficesDrop.length === 1) {
        this.defaultWorkProgramOfficeId = +this.wpOfficesDrop[0].id;
      } else if (this.participant.wwPrograms) {
        // If filtered by referred or enrolled only one instance exists
        const program = EnrolledProgram.filterByStatuses(this.participant.wwPrograms, [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.referred]);
        if (program.length === 0 || !this.appService.isProgramInMilwaukee(program)) {
          this.setWPOfficeForCF();
        }
      }
    } else {
      for (const wpo of this.workProgramOffices) {
        if (+wpo.officeNumber === +this.defaultOffice.officeNumber) {
          this.defaultWorkProgramOfficeId = +wpo.id;
          break;
        }
      }
    }

    this.model.workProgramOfficeId = this.defaultWorkProgramOfficeId;
    this.originalModel.workProgramOfficeId = this.defaultWorkProgramOfficeId;

    // If we TRY to set the default office, lets run the precheck.
    this.validateRfa();
  }

  private setWPOfficeForCF(): void {
    // finding the entry for the Milwaukee - 8040 and assigning it to milwaukee8040Drop
    if (this.wpOfficesDrop.length > 0) {
      const milwaukee8040Drop = this.wpOfficesDrop.find(x => x.name === 'Milwaukee - 8040');
      // wpOfficesDrop may contain other offices than Mil we need to remove the mil offices other than 8040 only
      if (milwaukee8040Drop) {
        this.wpOfficesDrop = this.filterMilwaukeeOffice(this.wpOfficesDrop);
        this.wpOfficesDrop.push(milwaukee8040Drop);
        this.defaultWorkProgramOfficeId = +milwaukee8040Drop.id;
      }
    }
  }
  private filterMilwaukeeOffice(arr: DropDownField[]) {
    return _.remove(arr, function(value: DropDownField) {
      return value.code !== 'Milwaukee';
    });
  }

  private initDefaultWorkOffice(data): void {
    if (data == null) {
      return;
    }
    this.defaultOffice = data;
    this.setDefaultWPOffice();
  }
  private initParticipant(part: Participant): void {
    this.participant = part;
  }

  private initRfaProgramModel(model: RFAProgram): void {
    if (model.children == null || model.children.length === 0) {
      model.children = [];
      const c = new RfaChild();
      model.children.push(c);
    }

    model.tjId = Utilities.idByFieldDataName(EnrolledProgramCode.tj, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    model.childrenFirstId = Utilities.idByFieldDataName(EnrolledProgramCode.cf, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    model.tmjId = Utilities.idByFieldDataName(EnrolledProgramCode.tmj, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    model.fcdpId = Utilities.idByFieldDataName(EnrolledProgramCode.fcdp, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');

    this.model = model;
    this.model.countyOfResidenceId = this.participant.countyOfResidenceId;
    this.originalModel = new RFAProgram();
    this.originalModel.countyOfResidenceId = this.participant.countyOfResidenceId;
    this.originalModel.fcdpTabId = this.fcdpCourtOrderId;
    RFAProgram.clone(model, this.originalModel);
  }

  private initRfaProgramsDrop(data): void {
    this.programDrop = data;
  }

  displayFcdpCourtDetails(e) {
    if (e) {
      if (e === 1) {
        return true;
      } else {
        return false;
      }
    }
  }

  private initContractorDrop(data): void {
    const defaultContractorId = Utilities.idByFieldDataName(this.appService.user.agencyCode, data, false, 'code');
    this.contractorDrop = data;
    if (this.hasLoaded === false || !this.model.contractorId) {
      // Set Default Contractor.
      this.model.contractorId = defaultContractorId;
      this.contractorName = Utilities.fieldDataNameById(this.model.contractorId, data, true);
    }
  }

  private initCountiesAndTribesDrop(data): void {
    this.countyTribeDrop = data;
  }

  private initCountiesDrop(data): void {
    this.countyDrop = data;
  }

  private initGenderDrop(data): void {
    this.genderDrop = data;
  }

  private initPopulationGroupDrops(data: any[]): void {
    const f1Id = Utilities.idByFieldDataName('F1 - In out-of-home care or transitioning to independent living', data);
    const g0Id = Utilities.idByFieldDataName('G0 - Adult youth with no children', data);

    const g1Id = Utilities.idByFieldDataName('G1 - Parent with a child support order', data);
    const g2Id = Utilities.idByFieldDataName('G2 - Parent under a reunification plan', data);
    const g3Id = Utilities.idByFieldDataName('G3 - Parent who is an ex-offender', data);
    this.model.doesNotMeetCriteriaPopTypeId = Utilities.idByFieldDataName('Does not meet criteria for target population', data);
    this.populationGroupDrops = [];
    for (const d of data) {
      const pg: IMultiSelectOption = {
        id: d.id,
        name: d.name,
        isDisabled: d.isDisabled,
        isPermanentlyDisabled: false,
        disablesOthers: d.disablesOthers,
        disabledIds: d.disabledIds
      };

      if (pg.id === f1Id && (this.participant.participantAge < 18 || this.participant.participantAge >= 25)) {
        // Disable F1.
        pg.isPermanentlyDisabled = true;
      } else if (pg.id === g0Id && (this.participant.participantAge < 18 || this.participant.participantAge >= 25 || this.model.hasChildren === true)) {
        // Disable G0.
        pg.isPermanentlyDisabled = true;
      } else if (pg.id === g1Id && (this.participant.participantAge < 18 || this.model.hasChildren === false)) {
        // Disable G1.
        pg.isPermanentlyDisabled = true;
      } else if (pg.id === g2Id && (this.participant.participantAge < 18 || this.model.hasChildren === false)) {
        // Disable G2
        pg.isPermanentlyDisabled = true;
      } else if (pg.id === g3Id && (this.participant.participantAge < 18 || this.model.hasChildren === false)) {
        // Disable G3.
        pg.isPermanentlyDisabled = true;
      }
      this.populationGroupDrops.push(pg);
    }

    this.selectOnlyPopulationType();
  }

  private selectOnlyPopulationType() {
    this.model.populationTypesIds = [];

    if (this.populationGroupDrops != null) {
      for (const pd of this.populationGroupDrops) {
        if (pd.isPermanentlyDisabled !== true) {
          this.model.populationTypesIds.push(pd.id);
        }
      }
    }
  }

  private initWorkProgramOffices(data): void {
    if (data != null) {
      this.workProgramOffices = [];
      this.wpOfficesDrop = [];
      this.workProgramOffices = data;
      for (const o of data) {
        const dd = new DropDownField();
        dd.id = o.id;
        dd.code = o.countyName;
        const officePadded = new PadPipe().transform(o.officeNumber, 4);
        if (officePadded) dd.name = o.countyName + ' - ' + officePadded.toString();
        else dd.name = o.countyName;
        this.wpOfficesDrop.push(dd);
      }
    }
  }

  public initSaveTabs(): void {
    this.saveTabs = [];
    const s = new DropDownField();
    s.name = 'Save';
    this.saveTabs.push(s);
    if (this.model.programId !== this.model.childrenFirstId) {
      const d = new DropDownField();
      d.name = 'Deny';
      this.saveTabs.push(d);
    }
    const r = new DropDownField();
    r.name = 'Refer';
    this.saveTabs.push(r);
  }

  clickDetermineEligibility(): void {
    this.validate();
    if (this.isSectionValid && this.canDetermineEligibility) {
      this.determineEligibility();
      this.isProgramDisabled = true;
      this.model.workProgramOfficeId = this.defaultWorkProgramOfficeId;
      this.originalModel.workProgramOfficeId = this.defaultWorkProgramOfficeId;
    }
  }

  get canDetermineEligibility(): boolean {
    if (this.model.canDetermineEligibility && this.hasEligibilityBeenVerified === false) {
      this.rfaEligibility = null;
      return true;
    } else {
      return false;
    }
  }

  private determineEligibility(): void {
    let query = this.getProgramCodeFromCurrentModel;
    if (this.model.isTmjProgram) {
      query += `/${this.model.contractorId}`;
    }

    this.fdService
      .getPopulationGroupsByProgramOrContractor(query)
      .pipe(
        concatMap(data => {
          this.initPopulationGroupDrops(data);

          return this.rfaService.determineProgramEligibility(this.model, this.pin.toString());
        })
      )
      .subscribe(res => {
        this.initRfaEligibility(res);
        this.hasEligibilityBeenVerified = true;
        this.eligibilityModelString = JSON.stringify(this.model);
        if (this.model.populationTypesIds[0] !== 5) this.model.populationTypesIds = [];
      });
  }

  get isChildrenListEmpty(): boolean {
    if (this.model != null && this.model.children != null && this.model.children.length > 0) {
      for (const c of this.model.children) {
        if (!c.isEmpty()) {
          return false;
        }
      }
      return true;
    }
  }

  private validate(isFromRefer?: boolean): void {
    //isFromRefer is set true only when called from refer method
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, moment(this.participant.dateOfBirth).format('MM/DD/YYYY'), this.fcdpCourtOrderId, isFromRefer);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
  }

  public checkState(): void {
    this.isSectionModified = true;
    // ANY Change sets the hasEligibilityBeenVerified to false because we want user to
    // determine eligibility anytime the form is changed.
    this.rfaEligibility = null;
    this.hasEligibilityBeenVerified = false;
    if (this.isSectionValid != null && this.isSectionValid !== true) {
      this.validate(true);
    }
  }

  private initRfaEligibility(data: RfaEligibility): void {
    this.rfaEligibility = data;
    if (this.rfaEligibility != null && this.rfaEligibility.serverMessages != null) {
      this.model.isEligible = this.rfaEligibility.isEligible;

      // We clear out previous eligibilityCodes on each load.
      this.model.eligibilityCodes = null;
      this.model.eligibilityCodes = [];

      for (const sm of this.rfaEligibility.serverMessages) {
        this.model.eligibilityCodes.push(sm.code);
      }
    }
  }

  get canDisplayReferDeny(): boolean {
    return !(this.model.programId === this.model.childrenFirstId || this.model.programId === this.model.fcdpId);
  }

  get canRefer(): boolean {
    if (
      this.model.isTJTmjProgram &&
      this.rfaEligibility != null &&
      this.rfaEligibility.isEligible &&
      this.model.populationTypesIds != null &&
      this.model.populationTypesIds.length > 0 &&
      this.model.populationTypesIds.indexOf(this.model.doesNotMeetCriteriaPopTypeId) === -1 &&
      this.validationMsgs != null &&
      this.validationMsgs.status === true
    ) {
      return true;
    } else if (
      this.model.isCFProgram &&
      this.model.canDetermineEligibility &&
      !Utilities.isStringEmptyOrNull(this.model.courtOrderCountyTribeId.toString()) &&
      !Utilities.isStringEmptyOrNull(this.model.courtOrderEffectiveDateMmDdYyyy) &&
      this.validationMsgs != null &&
      this.validationMsgs.status === true
    ) {
      return true;
    } else if (
      this.model.isFCDPProgram &&
      ((this.fcdpCourtOrderId === 1 && this.model.courtOrderCountyTribeId > 0 && !Utilities.isStringEmptyOrNull(this.model.courtOrderEffectiveDateMmDdYyyy)) ||
        this.fcdpCourtOrderId === 2) &&
      this.model.canDetermineEligibility &&
      this.validationMsgs != null &&
      this.validationMsgs.status === true &&
      this.model.referralSource &&
      this.model.referralSource.trim() !== ''
    ) {
      return true;
    } else {
      return false;
    }
  }

  get canDeny(): boolean {
    if (
      (this.rfaEligibility != null && this.rfaEligibility.isEligible === false && this.model.populationTypesIds != null && this.model.populationTypesIds.length > 0) ||
      (this.rfaEligibility != null && this.model.populationTypesIds != null && this.model.populationTypesIds.indexOf(this.model.doesNotMeetCriteriaPopTypeId) > -1)
    ) {
      if (this.model.populationTypesIds.indexOf(this.model.doesNotMeetCriteriaPopTypeId) > -1) {
        if (this.model.isPopulationDetailsDisplayed && this.model.isPopulationDetailsRequired && !Utilities.isStringEmptyOrNull(this.model.populationTypeDetails)) {
          return true;
        }
      } else {
        return true;
      }
    } else {
      return false;
    }
  }

  get isWpDisabled(): boolean {
    if (this.model.isTJTmjProgram) {
      return true;
    } else {
      return false;
    }
  }

  private checkIfFormShouldBeDisplayed(): boolean {
    if (this.model.programId != null && this.model.programId !== 0 && this.participant.countyOfResidenceId != null && this.participant.countyOfResidenceId !== 0) {
      return true;
    } else {
      return false;
    }
  }

  /**
   * This cleanse of the model is for both saving and refer/deny.
   *
   * @private
   * @memberof RfaEditComponent
   */
  private generalCleanse() {
    if (this.model != null && !this.model.ishasWorked16HoursLessDisplayed) {
      this.model.hasWorked16HoursLess = null;
    }
  }

  private cleanseModelForSaveAndExit() {
    this.model.populationTypesIds = [];
    this.model.populationTypeDetails = null;
    this.model.workProgramOfficeId = null;
    this.model.isEligible = null;
    this.model.eligibilityCodes = null;

    this.generalCleanse();
  }

  private cleanseModelForReferDeny() {
    if (this.model != null && this.model.populationTypesIds != null && this.model.populationTypesIds.indexOf(this.model.doesNotMeetCriteriaPopTypeId) === -1) {
      this.model.populationTypeDetails = null;
    }
    this.generalCleanse();
  }

  clickDeny(): void {
    this.validate();
    // Only to deny if valid.
    if (this.isSectionValid) {
      this.hasTriedSaving = true;
      this.cleanseModelForReferDeny();
      this.save().subscribe(data => {
        this.model.id = data.id;
        this.deny().subscribe(rData => {
          this.isSectionModified = false;
          this.exit();
        });
      });
    }
  }

  clickRefer(): void {
    this.validate(true);
    // Only to refer if valid.
    if (this.isSectionValid) {
      this.hasTriedSaving = true;
      this.cleanseModelForReferDeny();
      if (this.fcdpCourtOrderId === 1) {
        this.model.isVoluntary = false;
      } else {
        this.model.isVoluntary = true;
      }
      this.save().subscribe(data => {
        this.model.id = data.id;
        this.refer().subscribe(rData => {
          this.isSectionModified = false;
          this.exit();
        });
      });
    }
  }

  saveAndExit() {
    this.validate();
    this.cleanseModelForSaveAndExit();
    if (this.isSectionValid) {
      this.isSectionModified = false;
      this.save().subscribe(data => this.exit());
    }
  }

  private deny(): Observable<RFAProgram> {
    return this.rfaService.denyRfaById(this.model.id.toString(), this.pin.toString());
  }

  private refer(): Observable<RFAProgram> {
    return this.rfaService.referRfaById(this.model.id.toString(), this.pin.toString());
  }

  private save(): Observable<RFAProgram> {
    this.isSaving = true;
    // if (this.rfaEligibility != null && this.rfaEligibility.serverMessages != null) {
    //   this.model.isEligible = this.rfaEligibility.isEligible;
    //   const codes = [];
    //   for (const sm of this.rfaEligibility.serverMessages) {
    //     codes.push(sm.code);
    //   }
    //   this.model.eligibilityCodes = codes.join(',');
    // }
    return this.rfaService.saveRfa(this.model, this.pin.toString());
  }

  exit(): void {
    if (this.isSectionModified === true) {
      if (this.tempModalRef && this.tempModalRef.instance) {
        this.tempModalRef.instance.destroy();
      }
      this.modalService.create<WarningModalComponent>(WarningModalComponent).subscribe(x => {
        this.tempModalRef = x;
        this.tempModalRef.hostView.onDestroy(() => {
          if (this.modalService.allowAction === true) {
            this.close.emit();
          }
        });
      });
    } else {
      this.close.emit();
    }
  }

  sidebarToggle(): boolean {
    this.isCollapsed = !this.isCollapsed;
    this.userPreferencesService.isRfaSidebarCollapsed = this.isCollapsed;
    return this.isCollapsed;
  }

  ngOnDestroy(): void {
    if (this.proSub != null) {
      this.proSub.unsubscribe();
    }
    if (this.conSub != null) {
      this.conSub.unsubscribe();
    }
    if (this.couSub != null) {
      this.couSub.unsubscribe();
    }
    if (this.couTriSub != null) {
      this.couTriSub.unsubscribe();
    }
    if (this.genSub != null) {
      this.genSub.unsubscribe();
    }
    if (this.popSub != null) {
      this.popSub.unsubscribe();
    }
    if (this.wpSub != null) {
      this.wpSub.unsubscribe();
    }
  }
}
