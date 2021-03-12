// tslint:disable: import-blacklist
// tslint:disable: no-shadowed-variable
// tslint:disable: no-output-on-prefix
import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { timeValues } from '../../shared/dropdown-values/time-values';
import { DropDownField } from '../../shared/models/dropdown-field';
import { JobReadiness } from './models/job-readiness.model';
import { JobReadinessService } from './services/job-readiness.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FieldDataService } from '../../shared/services/field-data.service';
import { ValidationManager } from '../../shared/models/validation';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { Participant } from '../../shared/models/participant';
import { ParticipantService } from '../../shared/services/participant.service';
import { forkJoin } from 'rxjs';
import { Utilities } from '../../shared/utilities';
import { EnrolledProgramStatus } from '../../shared/enums/enrolled-program-status.enum';
import { ActionNeeded } from '../actions-needed/models/action-needed';
import { JRWorkPreferences } from '../../shared/models/jr-work-preferences.model';
import { Authorization } from '../../shared/models/authorization';
import { take, concatMap } from 'rxjs/operators';
import { AppService } from '../../core/services/app.service';

@Component({
  selector: 'app-job-readiness',
  templateUrl: './job-readiness.component.html',
  styleUrls: ['./job-readiness.component.scss']
})
export class JobReadinessComponent implements OnInit {
  @Output() onExitEditMode = new EventEmitter<boolean>();
  public amPmTab: { id: number; name: string }[];
  public hours: DropDownField[] = [];
  public minutes: DropDownField[] = [];
  public model: JobReadiness;
  public modelString: string;
  public originalModel = new JobReadiness();
  public originalModelString: string;
  public pin: string;
  public participant: Participant;
  public isLoaded = false;
  public polarDrop: DropDownField[] = [];
  public isSectionModified = false;
  public hasTriedSave = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public anValidationManager: ValidationManager = new ValidationManager(this.appService);
  public isSectionValid = true;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public anModelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public continueWithErrors = false;
  public goBackUrl: string;
  public isBeginTimeRequired = false;
  public isEndTimeRequired = false;
  public isReadOnly = true;
  public isSaving = false;
  public actionNeededs: ActionNeeded[];
  public actionRequired = false;
  public workShiftDrop: DropDownField[] = [];

  constructor(
    public jobReadinessService: JobReadinessService,
    private participantService: ParticipantService,
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    public appService: AppService,
    private router: Router
  ) {}
  ngOnInit() {
    forkJoin(
      this.participantService.getCurrentParticipant().pipe(take(1)),
      this.fdService.getPolarUnknown().pipe(take(1)),
      this.route.params.pipe(take(1)),
      this.fdService.getJobReadinessActionNeeded().pipe(take(1)),
      this.fdService.getFieldDataByField('jr-work-shifts').pipe(take(1))
    )
      .pipe(
        take(1),
        concatMap(results => {
          this.initParticipant(results[0]);
          this.polarDrop = results[1];
          this.pin = results[2].pin;
          this.initJobReadinessActionNeeded(results[3]);
          this.goBackUrl = '/pin/' + this.pin;
          this.workShiftDrop = results[4];

          return this.jobReadinessService.getJobReadinessData(this.pin);
        })
      )
      .subscribe(res => {
        if (res) this.initModel(res);
        else this.newJobReadiness();
      });

    this.hours = timeValues.hours;
    this.minutes = timeValues.minutes;
    this.amPmTab = [
      {
        id: 1,
        name: 'AM'
      },
      {
        id: 2,
        name: 'PM'
      }
    ];
  }

  private initParticipant(data) {
    this.participant = data;
    this.onParticipantInit();
  }
  initJobReadinessActionNeeded(data) {
    this.actionNeededs = data;
  }

  public onParticipantInit() {
    if (this.participant) {
      let hasAuth = false;
      hasAuth = this.appService.isUserAuthorized(Authorization.canAccessJobReadiness_Edit, this.participant);
      this.isReadOnly = this.appService.checkReadOnlyAccess(this.participant, [EnrolledProgramStatus.enrolled], hasAuth);
    }
  }

  public initModel(data) {
    this.model = data;
    JobReadiness.clone(this.model, this.originalModel);
    this.setModelString();
    this.onSelectTime();
    if (this.model.id !== 0) this.validate(false, true);
    this.isLoaded = true;
  }

  newJobReadiness() {
    const jobReadiness = new JobReadiness();
    jobReadiness.id = 0;
    this.model = jobReadiness;
    JobReadiness.clone(this.model, this.originalModel);
    this.setModelString();
    this.onSelectTime();
    this.isLoaded = true;
  }

  setModelString() {
    this.modelString = JSON.stringify(this.model);
    this.originalModelString = JSON.stringify(this.originalModel);
  }

  onSelectTime() {
    this.isBeginTimeRequired = this.model.jrWorkPreferences.isBeginTimeRequired();
    this.isEndTimeRequired = this.model.jrWorkPreferences.isEndTimeRequired();
  }

  validateActionNeeded() {
    this.setModelString();
    this.isSectionModified = this.modelString !== this.originalModelString;
    if (!this.isSectionValid) this.validate();
  }

  validate(isOnSave = false, isOnInit = false) {
    this.setModelString();
    this.isSectionModified = this.modelString !== this.originalModelString;
    this.actionRequired = this.isActionRequired();

    if (this.hasTriedSave === true || isOnInit || !this.isSectionValid) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, this.actionRequired);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
    }
    if (!this.isSectionValid && !isOnInit) this.continueWithErrors = true;
    else this.continueWithErrors = false;
    if (this.isSectionModified && !isOnSave) this.appService.isUrlChangeBlocked = true;
    else this.appService.isUrlChangeBlocked = false;
  }

  private isActionRequired() {
    if (
      this.model.jrWorkPreferences.kindOfJobDetails &&
      this.model.jrWorkPreferences.jobInterestDetails &&
      this.model.jrWorkPreferences.trainingNeededForJobDetails &&
      (this.model.jrWorkPreferences.someOtherPlacesJobAvailableDetails || this.model.jrWorkPreferences.someOtherPlacesJobAvailableUnknown) &&
      this.model.jrWorkPreferences.situationsToAvoidDetails &&
      this.model.jrWorkPreferences.beginHour &&
      (this.model.jrWorkPreferences.beginMinute || this.model.jrWorkPreferences.beginMinute === 0) &&
      this.model.jrWorkPreferences.beginAmPm &&
      this.model.jrWorkPreferences.endHour &&
      (this.model.jrWorkPreferences.endMinute || this.model.jrWorkPreferences.endMinute === 0) &&
      this.model.jrWorkPreferences.endAmPm &&
      this.model.jrWorkPreferences.travelTimeToWork &&
      this.model.jrWorkPreferences.distanceHomeToWork &&
      this.model.jrHistoryInfo.lastJobDetails &&
      this.model.jrHistoryInfo.accomplishmentDetails &&
      this.model.jrHistoryInfo.strengthDetails &&
      this.model.jrHistoryInfo.areasNeedImprove &&
      this.model.jrApplicationInfo.canSubmitOnline !== undefined &&
      this.model.jrApplicationInfo.canSubmitOnline !== null &&
      this.model.jrApplicationInfo.haveCurrentResume !== undefined &&
      this.model.jrApplicationInfo.haveCurrentResume !== null &&
      this.model.jrApplicationInfo.haveProfessionalReference !== undefined &&
      this.model.jrApplicationInfo.haveProfessionalReference !== null &&
      this.model.jrApplicationInfo.needDocumentLookupId &&
      this.model.jrInterviewInfo.lastInterviewDetails &&
      this.model.jrInterviewInfo.canLookAtSocialMedia !== undefined &&
      this.model.jrInterviewInfo.canLookAtSocialMedia !== null &&
      this.model.jrInterviewInfo.haveOutfit !== undefined &&
      this.model.jrInterviewInfo.haveOutfit !== null &&
      this.model.jrContactInfo.canYourPhoneNumberUsed !== undefined &&
      this.model.jrContactInfo.canYourPhoneNumberUsed !== null &&
      this.model.jrContactInfo.haveAccessToVoiceMailOrTextMessages !== undefined &&
      this.model.jrContactInfo.haveAccessToVoiceMailOrTextMessages !== null &&
      this.model.jrContactInfo.haveEmailAddress !== undefined &&
      this.model.jrContactInfo.haveEmailAddress !== null &&
      this.model.jrContactInfo.haveAccessDailyToEmail !== undefined &&
      this.model.jrContactInfo.haveAccessDailyToEmail !== null
    )
      return true;
    else return false;
  }

  cleanseModelForApi() {
    this.model.jrApplicationInfo = Utilities.cleanseModelForApi(this.model.jrApplicationInfo);
    this.model.jrContactInfo = Utilities.cleanseModelForApi(this.model.jrContactInfo);
    this.model.jrHistoryInfo = Utilities.cleanseModelForApi(this.model.jrHistoryInfo);
    this.model.jrInterviewInfo = Utilities.cleanseModelForApi(this.model.jrInterviewInfo);
    this.model.jrWorkPreferences = Utilities.cleanseModelForApi(this.model.jrWorkPreferences);
  }

  save() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.cleanseModelForApi();
    this.validate(true);
    if (this.isSectionValid === true) {
      this.saveJobReadiness();
    } else {
      this.isSaving = false;
    }
  }

  saveWithErrors() {
    this.isSaving = true;
    this.appService.isUrlChangeBlocked = false;
    this.cleanseModelForApi();
    this.saveJobReadiness(true);
  }

  saveJobReadiness(hasSaveErrors = false) {
    this.jobReadinessService.saveJobReadiness(this.pin, this.model.id, this.model, hasSaveErrors).subscribe(
      data => {
        this.model = data;
        JobReadiness.clone(this.model, this.originalModel);
        this.setModelString();
        this.router.navigateByUrl(`/pin/${this.pin}`);
        this.isSaving = false;
        this.onExitEditMode.emit();
      },
      error => {
        this.isSaving = false;
      }
    );
  }

  cancel() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = true;
      this.appService.isDialogPresent = true;
    } else {
      this.onExitEditMode.emit();
      this.router.navigateByUrl(`/pin/${this.pin}`);
    }
  }

  exit(e) {
    if (e === true) {
      this.onExitEditMode.emit();
    }
  }
  exitJobReadinessIgnoreChanges($event) {
    this.onExitEditMode.emit();
    this.appService.isUrlChangeBlocked = false;
    this.router.navigateByUrl(`/pin/${this.pin}`);
  }
}
