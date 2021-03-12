import { POPClaimStatusTypes } from './../../pop-claims/enums/pop-claim-status-types.enum';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { HelpService } from './../../../shared/services/help.service';
import { AuxiliaryStatusTypes } from './../enums/auxiliary-status-types.enums';
import { Auxiliary, AuxiliaryStatusCodes } from '../models/auxiliary.model';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { AuxiliaryService } from '../services/auxiliary.service';
import { Component, OnInit, Input } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Participant } from 'src/app/shared/models/participant';
import { take, concatMap } from 'rxjs/operators';
import { of, forkJoin } from 'rxjs';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { Utilities } from 'src/app/shared/utilities';
import { PadPipe } from 'src/app/shared/pipes/pad.pipe';
import * as util from 'src/app/shared/utilities';

@Component({
  selector: 'app-auxiliary-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class AuxiliaryEditComponent implements OnInit {
  public isLoaded = false;
  public hadSaveError = false;
  public isSaving = false;
  public isSectionValid = true;
  public yearsDrop: DropDownField[] = [];
  public participationPeriodDrop: DropDownField[] = [];
  public auxiliaryReasonsDrop: DropDownField[] = [];
  public auxiliaryStatusTypesDrop: DropDownField[] = [];
  public filteredStatusTypesDrop: DropDownField[] = [];
  @Input() participant: Participant;
  @Input() participationDetails: any;
  @Input() allAuxiliaries: Auxiliary[];
  public isReadOnly: boolean;
  public canApproveAux = false;
  public isStatusRequired = false;
  public auxiliary: Auxiliary = new Auxiliary();
  public cachedModel: Auxiliary = new Auxiliary();
  public hasTriedSave: boolean;
  public isSectionModified: boolean;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};

  public canViewStatusesbol = false;
  public canWithDrawbool = false;
  public isReturnedAuxiliarybool = false;
  public auxSheetUrl: string;
  public isDb2InfoLoaded = false;
  public pullDownDates: DropDownField[];
  public participationPeriod: string;
  public totalW2PaymentForPP: number;
  public showCalDetails = false;
  public maxW2Payment = 673;
  public cutOverDate = null;
  public isParticipationPeriodValid = true;
  public showOriginalPayment = true;
  constructor(
    private auxService: AuxiliaryService,
    private fdService: FieldDataService,
    private appService: AppService,
    private helpService: HelpService,
    private padPipe: PadPipe
  ) {}

  ngOnInit() {
    this.initAuxiliaryModel();
    if (this.participant) {
      this.cutOverDate = this.participant.cutOverDate || null;
    }
    this.getPullDownDate();
  }
  requestDataFromMultipleSources() {
    return forkJoin(this.fdService.getFieldDataByField(FieldDataTypes.ParticipationPeriod).pipe(take(1)), this.fdService.getFieldDataByField('auxiliaryreasons').pipe(take(1)));
  }
  initYearDrop() {
    const max = new Date().getFullYear();
    const drop = [];
    for (let year = max; year >= 1996; year--) {
      const m = new DropDownField();
      m.id = year;
      m.name = year.toString();
      drop.push(m);
    }
    return drop;
  }
  initAuxiliaryStatusTypes() {
    this.fdService.getFieldDataByField('auxiliarystatustypes').subscribe(res => {
      this.auxiliaryStatusTypesDrop = res;
      this.filteredStatusTypesDrop = this.auxiliaryStatusTypesDrop.filter(x => x.code === AuxiliaryStatusTypes.APPROVE || x.code === AuxiliaryStatusTypes.DENY);
      this.isLoaded = true;
      this.isDb2InfoLoaded = true;
    });
  }
  initAuxiliaryModel() {
    forkJoin(
      this.auxService.modeForAuxiliary.pipe(
        take(1),
        concatMap(res => {
          this.isReadOnly = res.readonly;
          return res.aux ? of(res) : of(null);
        })
      ),
      this.helpService.getHelpUrl('auxiliary-worksheet')
    ).subscribe(data => {
      if (data[0]) {
        this.auxiliary = data[0].aux;
        this.canApproveAux = data[0].canApprove;
        this.auxiliary.pinNumber = this.padPipe.transform(this.auxiliary.pinNumber, 10);
        this.auxiliary.caseNumber = this.padPipe.transform(this.auxiliary.caseNumber, 10);
        this.auxiliary.details = null;
        this.isStatusRequired = this.auxiliary.isStatusRequired(this.auxiliary.auxiliaryStatusTypeCode, data[0].canApprove);
        const obj = new DropDownField(this.auxiliary.participationPeriodId, this.auxiliary.participationPeriodName);
        this.participationPeriodDrop.push(obj);
        const obj1 = new DropDownField(this.auxiliary.auxiliaryReasonId, this.auxiliary.auxiliaryReasonName);
        this.auxiliaryReasonsDrop.push(obj1);
        const obj2 = new DropDownField(this.auxiliary.participationPeriodYear, this.auxiliary.participationPeriodYear.toString());
        this.yearsDrop.push(obj2);
        this.participationPeriod = Utilities.fieldDataNameById(this.auxiliary.participationPeriodId, this.participationPeriodDrop, true);
        Auxiliary.clone(this.auxiliary, this.cachedModel);
        this.isReturnedAuxiliarybool = this.isReturnedAuxiliary();
        this.showOriginalPayment = this.auxiliary && this.auxiliary.auxiliaryStatusTypeCode !== AuxiliaryStatusCodes.SystemGenerated;
        this.initAuxiliaryStatusTypes();
      } else {
        this.auxiliary.id = 0;
        this.auxiliary.pinNumber = this.padPipe.transform(this.participant.pin, 10);
        this.auxiliary.caseNumber = this.padPipe.transform(this.auxiliary.caseNumber, 10);
        this.auxiliary.participantName = this.participant.displayName;
        this.auxiliary.participantId = this.participant.id;
        Auxiliary.clone(this.auxiliary, this.cachedModel);
        this.requestDataFromMultipleSources().subscribe(results => {
          this.participationPeriodDrop = results[0];
          this.auxiliaryReasonsDrop = results[1];
          this.yearsDrop = this.initYearDrop();
          this.isLoaded = true;
        });
      }
      this.canWithDrawbool = this.canWithDraw();
      this.auxSheetUrl = data[1];
    });
  }
  getPullDownDate() {
    this.fdService.getFieldDataByField(FieldDataTypes.PullDownDates).subscribe(res => {
      this.pullDownDates = res;
    });
  }
  calculateTotalW2Payment() {
    return this.auxiliary.calculateTotalW2Payment(this.allAuxiliaries, this.participationPeriod, this.auxiliary.participationPeriodYear);
  }
  validateForParticipationPeriod() {
    this.isSectionModified = true;
    this.validationManager.resetErrors();
    this.hasTriedSave = false;
    const result = this.auxiliary.validationsForParticipationPeriod(
      this.validationManager,
      this.allAuxiliaries,
      this.participationPeriod,
      this.auxiliary.participationPeriodYear,
      this.pullDownDates,
      true,
      this.appService.getFeatureToggleDate('Auxiliary')
    );
    this.isParticipationPeriodValid = result.isValid;
    this.totalW2PaymentForPP = this.calculateTotalW2Payment();
    this.modelErrors = result.errors;
    if (this.modelErrors) {
      this.isSectionValid = false;
    }
    if (this.isParticipationPeriodValid) {
      this.isSectionValid = true;
      this.isDb2InfoLoaded = true;
    }
  }
  getDetailsBasedonParticipationPeriod() {
    this.isDb2InfoLoaded = false;
    if (this.auxiliary.participationPeriodId && this.auxiliary.participationPeriodYear) {
      this.participationPeriod = Utilities.fieldDataNameById(this.auxiliary.participationPeriodId, this.participationPeriodDrop, true);
      this.auxService
        .getDetailsBasedonParticipationPeriod(this.participant.pin, this.participant.id, this.participationPeriod, this.auxiliary.participationPeriodYear)
        .subscribe(res => {
          this.auxiliary.caseNumber = res.caseNumber;
          this.auxiliary.originalPayment = res.originalPayment;
          this.auxiliary.countyName = res.countyName;
          this.auxiliary.countyNumber = res.countyNumber;
          this.auxiliary.officeNumber = this.padPipe.transform(res.officeNumber, 4);
          this.auxiliary.programCd = res.programCd;
          this.auxiliary.subProgramCd = res.subProgramCd;
          this.auxiliary.agSequenceNumber = res.agSequenceNumber;
          this.auxiliary.isAllowed = res.isAllowed;
          this.auxiliary.overPayAmount = !res.overPayAmount ? '0' : res.overPayAmount;
          this.auxiliary.recoupmentAmount = !res.recoupmentAmount ? 0 : res.recoupmentAmount;
          this.validateForParticipationPeriod();
        });
    } else {
      this.totalW2PaymentForPP = 0;
      this.auxiliary.caseNumber = null;
      this.auxiliary.originalPayment = null;
      this.auxiliary.countyName = null;
      this.auxiliary.countyNumber = null;
      this.auxiliary.officeNumber = null;
      this.auxiliary.programCd = null;
      this.auxiliary.subProgramCd = null;
      this.auxiliary.agSequenceNumber = null;
      this.auxiliary.isAllowed = null;
      this.auxiliary.overPayAmount = null;
      this.isDb2InfoLoaded = false;
    }
    if (this.isParticipationPeriodValid && this.hasTriedSave) {
      this.validate();
    }
  }

  isReturnedAuxiliary() {
    return this.auxiliary.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.RETURN;
  }
  exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.auxService.modeForAuxiliary.next({ readOnly: false, isInEditMode: false });
    }
  }
  exitAuxiliaryEditIgnoreChanges(e) {
    this.auxService.modeForAuxiliary.next({ readOnly: false, isInEditMode: false });
  }
  auxWithSubmitStatus() {
    return this.allAuxiliaries.some(
      aux =>
        aux.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.SUBMIT &&
        aux.participationPeriodName === this.participationPeriod &&
        aux.participationPeriodYear === this.auxiliary.participationPeriodYear
    );
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      const currentDate = Utilities.currentDate.clone();
      this.validationManager.resetErrors();
      const result = this.auxiliary.validate(
        this.validationManager,
        this.canApproveAux,
        this.cutOverDate,
        this.allAuxiliaries,
        this.participationPeriod,
        this.pullDownDates,
        currentDate,
        this.appService.getFeatureToggleDate('Auxiliary'),
        this.auxiliaryStatusTypesDrop,
        this.isParticipationPeriodValid,
        this.appService.user
      );
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) {
        if (util.isUndefined(this.totalW2PaymentForPP)) {
          this.totalW2PaymentForPP = this.calculateTotalW2Payment();
        }
        this.isSaving = false;
      }
    }
  }
  save() {
    if (this.isSectionValid) {
      this.hadSaveError = false;
      this.isSaving = true;
      let pin;
      if (this.participant) {
        pin = this.participant.pin;
      } else {
        pin = this.auxiliary.pinNumber;
      }
      this.auxService.saveAuxiliary(this.auxiliary, pin).subscribe(
        res => {
          this.auxService.modeForAuxiliary.next({ readOnly: false, isInEditMode: false });
          this.isSaving = false;
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }

  showHideCalDetails() {
    this.showCalDetails = !this.showCalDetails;
  }
  changeStatus() {
    if (this.auxiliary.auxiliaryStatusTypeId === null) {
      this.auxiliary.auxiliaryStatusTypeCode = AuxiliaryStatusTypes.SUBMIT;
    } else {
      this.auxiliary.auxiliaryStatusTypeName = null;
    }
  }
  canWithDraw() {
    if (this.auxiliary.id > 0) {
      return this.isReturnedAuxiliarybool && !this.canApproveAux;
    } else {
      return false;
    }
  }

  submit() {
    this.auxiliary.isSubmit = true;
    this.auxiliary.isWithDraw = false;
    this.saveAndExit();
  }
  withDraw() {
    this.auxiliary.isSubmit = false;
    this.auxiliary.isWithDraw = true;
    this.saveAndExit();
  }

  saveAndExit() {
    this.hasTriedSave = true;
    this.validate();
    this.save();
  }
}
