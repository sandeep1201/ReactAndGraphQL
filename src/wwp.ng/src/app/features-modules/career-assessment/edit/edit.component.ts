import { CareerAssessment } from './../models/career-assessment.model';
import { ParticipantService } from './../../../shared/services/participant.service';
import { Component, OnInit, Input } from '@angular/core';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ValidationManager } from '../../../shared/models/validation';
import { CareerAssessmentService } from './../services/career-assessment.service';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { DateMmDdYyyyPipe } from '../../../shared/pipes/date-mm-dd-yyyy.pipe';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { switchMap, take, concatMap } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';
@Component({
  selector: 'app-career-assessment-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class CareerAssessmentEditComponent extends BaseParticipantComponent implements OnInit {
  @Input() careerAssessmentId: number;

  numberOfDaysCanBackDate: any;
  public isSaving = false;
  public hasSaveError = false;
  public mode = 'Add';
  public participationStatusTypesDrop;
  public isLoaded = false;
  public selectedProgramNameDisabled = false;
  public isSectionValid = true;
  public isSectionModified = false;
  public hasTriedSave = false;
  public hadSaveError = false;
  public isEnding = false;
  public selectedStatusName: string;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public currentProgram: any;
  public careerAssessment: CareerAssessment;
  public cachedModel: CareerAssessment = new CareerAssessment();
  public element: DropDownField[];
  public isReadOnly = false;
  public pin: string;
  public modelErrors: ModelErrors = {};

  constructor(
    private appService: AppService,
    partService: ParticipantService,
    private careerAssessmentService: CareerAssessmentService,
    route: ActivatedRoute,
    router: Router,
    private fdService: FieldDataService
  ) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
  }
  onParticipantInit() {
    this.initCareerAssessmentModel();
  }
  initCareerAssessmentModel() {
    this.careerAssessmentService.modeForCareerAssessment
      // we are calling the career assessment only if the id is > 0 i.e, an already created assessment if not i am returning an Observable of null
      .pipe(
        take(1),
        concatMap(res => {
          this.isReadOnly = res.readonly;
          return res.id > 0 ? this.careerAssessmentService.getCareerAssessment(this.participant.pin, res.id) : of(null);
        })
      )
      .subscribe(data => {
        if (data) {
          this.careerAssessment = data;
          const pipe = new DateMmDdYyyyPipe();
          this.careerAssessment.completionDate = pipe.transform(this.careerAssessment.completionDate);
          this.initElementDrop();
          CareerAssessment.clone(this.careerAssessment, this.cachedModel);
        } else {
          this.careerAssessment = new CareerAssessment();
          this.careerAssessment.id = 0;
          this.initElementDrop();
          CareerAssessment.clone(this.careerAssessment, this.cachedModel);
        }
      });
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.careerAssessment.validate(this.validationManager, this.participant.dateOfBirth);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  exitCAEditIgnoreChanges(e) {
    this.careerAssessmentService.modeForCareerAssessment.next({ readOnly: false, isInEditMode: false });
  }

  initElementDrop() {
    this.fdService.getElement().subscribe(res => {
      this.element = res;
      this.isLoaded = true;
    });
  }
  public exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.careerAssessmentService.modeForCareerAssessment.next({ readOnly: false, isInEditMode: false });
    }
  }

  save() {
    if (this.isSectionValid) {
      this.isSaving = true;
      this.careerAssessmentService.saveCareerAssessment(this.careerAssessment, this.pin).subscribe(
        res => {
          this.careerAssessmentService.modeForCareerAssessment.next({ readOnly: false, isInEditMode: false });
          this.isSaving = false;
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }
  saveAndExit() {
    this.hasTriedSave = true;
    this.validate();
    this.save();
  }
}
