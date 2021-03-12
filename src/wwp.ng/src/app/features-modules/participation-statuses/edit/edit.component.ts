import { FieldDataTypes } from './../../../shared/enums/field-data-types.enum';
import { ParticipationStatusTypes } from '../../../shared/enums/participation-status-types.enum';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { Component, OnInit, Input } from '@angular/core';
import { ParticipationStatus } from '../../../shared/models/participation-statuses.model';
import { AppService } from '../../../core/services/app.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { forkJoin } from 'rxjs';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { Utilities } from '../../../shared/utilities';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { DateMmDdYyyyPipe } from '../../../shared/pipes/date-mm-dd-yyyy.pipe';
import { WhyReason } from '../../../shared/models/why-reasons.model';
import * as _ from 'lodash';
import { switchMap, take, distinctUntilChanged } from 'rxjs/operators';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';

@Component({
  selector: 'app-participation-statuses-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
  providers: [FieldDataService]
})
export class ParticipationStatusEditComponent extends BaseParticipantComponent implements OnInit {
  numberOfDaysCanBackDate: any;
  numberOfDaysCanBackDateForEndDate: string;
  public isSaving = false;
  public hasSaveError = false;
  public mode = 'Add';
  public participationStatusTypesDrop;
  public model: ParticipationStatus;
  public cachedModel: ParticipationStatus = new ParticipationStatus();
  public modelErrors: ModelErrors = {};
  public isLoaded = false;
  public programDrop: DropDownField[];
  public selectedProgramNameDisabled = false;
  public isSectionValid = true;
  public isSectionModified = false;
  public hasTriedSave = false;
  public hadSaveError = false;
  public isEnding = false;
  public canAddPSStatus: WhyReason;
  @Input() participationStatuses: ParticipationStatus[];
  @Input() currentlyEnrolledPrograms: EnrolledProgram[];
  public selectedStatusName: string;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public currentProgram: EnrolledProgram;
  private pullDownDates: DropDownMultiField[] = [];
  public modelIds: any = {};
  public enrolledProgramCd: string;
  public baseUrlForFieldData = this.appService.apiServer + 'api/fielddata/';
  constructor(private fdService: FieldDataService, route: ActivatedRoute, router: Router, private appService: AppService, partService: ParticipantService) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
  }

  onParticipantInit() {
    this.initParticipationStatusModel();
  }
  initParticipationStatusModel() {
    this.partService.modeForParticipationStatuses
      .pipe(
        switchMap(res =>
          forkJoin(this.partService.getPSStatusById(this.pin, res.id), this.fdService.getMultiFieldData(`${this.baseUrlForFieldData}${FieldDataTypes.PullDownDates}`))
        )
      )
      .subscribe(data => {
        if (data[0] && data[0].id > 0) {
          this.model = data[0];
          if (this.model.beginDate) {
            const pipe = new DateMmDdYyyyPipe();
            this.model.beginDate = pipe.transform(this.model.beginDate);
          }
          if (this.participant && this.participant.programs && this.model && this.model.enrolledProgramId) {
            this.currentProgram = this.participant.programs.find(x => +x.enrolledProgramId === +this.model.enrolledProgramId);
            if (this.currentProgram) {
              const enrolledProgramCd = this.currentProgram.programCd;
              if (enrolledProgramCd) this.getDaysForValidatingBackDate(enrolledProgramCd);
            }
          }
          ParticipationStatus.clone(this.model, this.cachedModel);
          this.pullDownDates = data[1];
          this.isEnding = true;
          this.isLoaded = true;
        } else {
          this.model = new ParticipationStatus();
          this.model.id = 0;
          this.initProgramDrop();
          this.pullDownDates = data[1];
          ParticipationStatus.clone(this.model, this.cachedModel);
        }
      });
  }
  // selectedStatusCode() {
  //   for (let i = 0; i < this.participationStatusTypesDrop.length; i++) {
  //     if (this.participationStatusTypesDrop[i].id === this.model.statusId) {
  //       this.selectedStatusName = this.participationStatusTypesDrop[i].name;
  //       return;
  //     }
  //   }
  // }

  selectedStatusCode() {
    if (this.participationStatusTypesDrop) {
      this.participationStatusTypesDrop.reduce((hasElement, element) => {
        if (element.id === this.model.statusId) {
          hasElement = element.name;
        }
        return (this.selectedStatusName = hasElement);
      }, '');
    }
  }
  afterdataLoaded() {
    this.modelIds.TAId = Utilities.fieldDataIdByCode(ParticipationStatusTypes.TA, this.participationStatusTypesDrop);
    this.modelIds.TEId = Utilities.fieldDataIdByCode(ParticipationStatusTypes.TE, this.participationStatusTypesDrop);
  }
  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      this.selectedStatusCode();
      const statusCd = Utilities.fieldDataCodeById(this.model.statusId, this.participationStatusTypesDrop);
      const result = this.model.validate(
        this.validationManager,
        this.participant.dateOfBirth,
        this.participationStatuses,
        this.selectedStatusName,
        this.numberOfDaysCanBackDate,
        this.numberOfDaysCanBackDateForEndDate,
        this.currentlyEnrolledPrograms,
        this.model.id,
        this.currentProgram,
        this.modelIds,
        this.pullDownDates,
        this.enrolledProgramCd,
        statusCd
      );
      this.isSectionValid = result.isValid;
      if (this.isSectionValid) this.hasTriedSave = false;
      this.modelErrors = result.errors;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }
  preCheckToAddPS() {
    this.isSectionValid = true;
    if (this.currentProgram.isTj || this.currentProgram.isTmj) {
      // this.callPreCheckForTATEPS();
      this.partService.canAddPS(this.pin, this.model).subscribe(
        res => {
          this.canAddPSStatus = _.merge(res, this.canAddPSStatus);
          return;
        },
        error => console.log(error),
        // This will be called once the above subscription is complete and validate and save are called only when we dont have any errors from precheck
        () => {
          if (this.canAddPSStatus.status !== true) {
            this.isSaving = false;
            if (this.isSectionValid) this.isSectionValid = false;
            return;
          }
        }
      );
    } else {
      return;
    }
  }
  private initParticipationStatusTypes() {
    this.currentProgram = this.participant.programs.find(x => +x.enrolledProgramId === +this.model.enrolledProgramId);
    if (this.currentProgram) {
      this.enrolledProgramCd = Utilities.fieldDataCodeById(this.currentProgram.enrolledProgramId, this.programDrop);
      this.getDaysForValidatingBackDate(this.enrolledProgramCd);
      this.fdService
        .getFieldDataByField(FieldDataTypes.ParticipationStatuses, this.enrolledProgramCd)
        .pipe(take(1))
        .pipe(distinctUntilChanged())
        .subscribe(res => {
          this.participationStatusTypesDrop = res;
          this.isLoaded = true;
          this.afterdataLoaded();
        });
    }
    this.participationStatusTypesDrop = [];
    this.isLoaded = true;
    this.afterdataLoaded();
  }

  private initProgramDrop() {
    this.programDrop = [];
    if (this.participant != null && this.participant.programs != null) {
      let refPrograms = this.participant.getCurrentEnrolledProgramsByAgency(this.appService.user.agencyCode);
      refPrograms = this.appService.filterProgramsForUserAuthorized<EnrolledProgram>(refPrograms);

      if (refPrograms != null) {
        for (const pro of refPrograms) {
          const x = new DropDownField();
          x.id = pro.enrolledProgramId;
          x.name = pro.programCode;
          x.code = pro.programCd;
          this.programDrop.push(x);
        }

        if (this.programDrop.length === 1 && !this.model.enrolledProgramId) {
          this.selectedProgramNameDisabled = true;
          this.model.enrolledProgramId = this.programDrop[0].id;
        }
      }
    }
    this.initParticipationStatusTypes();
  }

  getDaysForValidatingBackDate(enrolledProgramCd: string) {
    this.partService.getDaysForBackDating(enrolledProgramCd).subscribe(data => {
      this.numberOfDaysCanBackDate = data[0].maxDaysCanBackDate;
      this.numberOfDaysCanBackDateForEndDate = data[0].maxDaysCanBackDatePS;
    });
  }

  public exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.partService.modeForParticipationStatuses.next({ readOnly: false, inEditMode: false });
    }
  }
  exitPSEditIgnoreChanges() {
    this.partService.modeForParticipationStatuses.next({ readOnly: false, inEditMode: false });
  }
  save() {
    if (this.isSectionValid) {
      this.model.participantId = this.participant.id;
      if (this.model.statusId && this.participationStatusTypesDrop) this.model.statusName = Utilities.fieldDataNameById(this.model.statusId, this.participationStatusTypesDrop);
      if (!this.model.statusCode) this.model.statusCode = Utilities.fieldDataCodeById(this.model.statusId, this.participationStatusTypesDrop);
      if (this.model.isCurrent) this.model.endDate = null;
      if (this.model.endDate) this.model.isCurrent = false;
      if (this.model.id > 0) {
        this.partService.updateStatus(this.pin, this.model).subscribe(res => {
          this.partService.modeForParticipationStatuses.next({ readOnly: false, inEditMode: false });
        });
      } else {
        this.partService.addStatus(this.pin, this.model).subscribe(res => {
          this.partService.modeForParticipationStatuses.next({ readOnly: false, inEditMode: false });
        });
      }
    }
  }
  saveAndExit() {
    this.hasTriedSave = true;
    this.isSaving = true;
    this.validate();
    this.save();
  }
}
