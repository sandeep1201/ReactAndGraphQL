import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Observable, Subscription, forkJoin, observable, of } from 'rxjs';
import { SubSink } from 'subsink';
import { take, concatMap } from 'rxjs/operators';
import { ValidationManager } from 'src/app/shared/models/validation';
import { Participant } from 'src/app/shared/models/participant';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';
import { AppService } from 'src/app/core/services/app.service';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { Utilities } from 'src/app/shared/utilities';
import { FeatureToggleTypes } from 'src/app/shared/enums/feature-toggle-types.enum';

@Component({
  selector: 'app-employment-plan',
  templateUrl: './employment-plan.component.html',
  styleUrls: ['./employment-plan.component.scss']
})
export class EmploymentPlanComponent implements OnInit, OnDestroy {
  setDate: string;
  compareBeginDate: string;
  subtractFromCurrentDate: any;
  public recordId = 0;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public participant: Participant;
  public programDrop: DropDownField[] = [];
  public employabilityPlan: EmployabilityPlan;
  public selectedProgramNameDisabled = false;
  public isSectionModified = false;
  public hasTriedSave = false;
  public isSectionValid = false;
  public modelErrors: ModelErrors = {};
  public epId: number;
  public epSub: Subscription;
  public isLoaded = false;
  public hadSaveError = false;
  public isSaving = false;
  pin: string;
  employabilityPlanId: string;
  public errorMsg: string;

  public currentProgram: any;
  public disableFields = false;
  public employabilityPlanStatusTypes: DropDownField[] = [];

  public originalEmployabilityPlan: EmployabilityPlan = new EmployabilityPlan();
  public subsequentEPId: number;
  public precheck: WhyReason = new WhyReason();
  public inProgressEps: EmployabilityPlan[];
  private subs = new SubSink();
  private lastEp: any;
  public canSaveWithWarnings = false;
  private pullDownDates: DropDownMultiField[] = [];
  public employabilityPlans: EmployabilityPlan[];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private employabilityPlanService: EmployabilityPlanService,
    private participantService: ParticipantService,
    private fdService: FieldDataService,
    private location: Location
  ) {}

  ngOnInit() {
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.subs.add(
      this.route.params.subscribe(params => {
        this.employabilityPlanId = params.id;
      })
    );

    this.subs.add(
      this.requestDataFromMultipleSources()
        .pipe(take(1))
        .subscribe(results => {
          this.initEmployabilityPlan(results);
        })
    );
  }

  public requestDataFromMultipleSources(): Observable<any[]> {
    const response1 = this.route.parent.params.pipe(take(1));
    const response2 = this.route.params.pipe(take(1));
    const response3 = this.appService.employabilityPlan.pipe(take(1));
    const response4 = this.participantService.getCurrentParticipant().pipe(take(1));
    const response5 = this.fdService.getFieldDataByField(FieldDataTypes.EmployabilityPlanStatustypes).pipe(take(1));
    const response6 = this.fdService.getFieldDataByField(FieldDataTypes.PullDownDates).pipe(take(1));
    return forkJoin([response1, response2, response3, response4, response5, response6]);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }
  private initParticipant(data) {
    this.participant = data[3];
    this.initProgramsDrop(data[2]);
  }
  private initEmployabilityPlan(data) {
    this.employabilityPlanService.getEmployabilityPlans(this.pin).subscribe(res => {
      this.inProgressEps = res.filter(ep => ep.employabilityPlanStatusTypeName === 'In Progress');
      this.employabilityPlans = res;
    });
    if (+data[1].id === 0) {
      this.epId = data[1].id;
      this.newEmployabilityPlan();
      EmployabilityPlan.clone(this.employabilityPlan, this.originalEmployabilityPlan);
      this.initParticipant(data);
    } else {
      if (+data[1].id > 0) {
        this.subs.add(
          this.employabilityPlanService.getEpById(this.pin, data[1].id).subscribe(ep => {
            this.employabilityPlan = ep;
            if (this.employabilityPlan.employabilityPlanStatusTypeName === 'In Progress') {
              this.disableFields = true;
              this.selectedProgramNameDisabled = true;
            }
            EmployabilityPlan.clone(this.employabilityPlan, this.originalEmployabilityPlan);
            this.initParticipant(data);
          })
        );
      }
    }
    this.lastEp = data[2];
    this.employabilityPlanStatusTypes = data[4];
    this.pullDownDates = data[5];
  }
  private newEmployabilityPlan() {
    const ep = new EmployabilityPlan();
    ep.id = 0;
    this.employabilityPlan = ep;
    this.employabilityPlan.canSaveWithoutActivity = false;
  }

  disableNotes() {
    if (this.employabilityPlan.employabilityPlanStatusTypeName === 'In Progress' || this.employabilityPlan.id === 0) return false;
    else return true;
  }
  assignEnrolledProgramId() {
    this.currentProgram = this.participant.programs.find(x => +x.enrolledProgramId === +this.employabilityPlan.enrolledProgramId);
  }

  CleanseModelForApi() {
    if (this.employabilityPlan.notes != null && this.employabilityPlan.notes.trim() === '') {
      this.employabilityPlan.notes = null;
    }
    if (!this.employabilityPlan.canSaveWithoutActivity) {
      this.employabilityPlan.canSaveWithoutActivityDetails = null;
    }
  }

  private initProgramsDrop(data) {
    this.programDrop = [];
    if (this.participant != null && this.participant.programs != null) {
      let refPrograms = this.participant.getCurrentEnrolledProgramsByAgency(this.appService.user.agencyCode);
      refPrograms = this.appService.filterProgramsForUserAuthorized<EnrolledProgram>(refPrograms);

      if (refPrograms != null) {
        for (const pro of refPrograms) {
          const x = new DropDownField();
          x.id = pro.enrolledProgramId;
          x.name = pro.programCode;
          this.programDrop.push(x);
        }

        if (this.programDrop.length === 1) {
          this.selectedProgramNameDisabled = true;
          this.employabilityPlan.enrolledProgramId = this.programDrop[0].id;
        }
      }
    }

    if (
      this.employabilityPlan &&
      this.employabilityPlan.id === 0 &&
      this.programDrop &&
      this.programDrop.length === 1 &&
      data &&
      data.submittedEps &&
      data.submittedEps.length === 1 &&
      this.programDrop[0].id === data.submittedEps[0].enrolledProgramId
    ) {
      this.employabilityPlan.notes = data.submittedEps[0].notes;
    }

    this.isLoaded = true;
    this.currentProgram = this.participant.programs.find(x => +x.enrolledProgramId === +this.employabilityPlan.enrolledProgramId);
  }

  public validate(cont) {
    this.isSectionModified = true;
    this.appService.isEPUrlChangeBlocked = this.isSectionModified;
    this.appService.componentDataModified.next({ dataModified: true });
    this.precheck.errors = null;
    this.precheck.warnings = null;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      // We should always have a current enrollment but in case we dont, fail validation.
      let enrollmentDate = '';
      if (this.currentSelectedEnrolledProgram == null) {
        // Setting to a low date.
        enrollmentDate = '01/01/0001';
      } else {
        enrollmentDate = this.currentSelectedEnrolledProgram.enrollmentDateMmDdYyyy;
      }
      //Find the program type selected in the drop down to get the program code and send it to the API call for getting the maxDaysCanBackDate value
      this.employabilityPlan.pepId = this.currentProgram.id;
      const enrolledProgramName = Utilities.fieldDataNameById(this.employabilityPlan.enrolledProgramId, this.programDrop, true);
      const result = this.employabilityPlan.validate(
        this.validationManager,
        enrollmentDate,
        this.compareBeginDate,
        this.subtractFromCurrentDate,
        this.currentProgram.enrollmentDate,
        this.disableFields,
        this.pullDownDates,
        this.employabilityPlans,
        this.participant,
        this.appService.getFeatureToggleDate(FeatureToggleTypes.EPCutOverValidationDate),
        this.appService.getFeatureToggleValue(FeatureToggleTypes.EPGoLive),
        this.inProgressEps,
        Utilities.currentDate,
        enrolledProgramName
      );

      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) {
        this.hasTriedSave = false;
      }
    }
  }

  get currentSelectedEnrolledProgram(): EnrolledProgram {
    if (this.participant != null && this.participant.programs != null) {
      return this.participant.programs.find(x => x.id === this.employabilityPlan.enrolledProgramId);
    }
  }

  public clickSave(cont: boolean, fromWarning = false) {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.hadSaveError = false;
    // Get all submitted Ep's and filter by current enrolled program and sort by EP begin date
    if (this.employabilityPlan.id === 0) {
      if (this.lastEp.submittedEps && this.lastEp.submittedEps !== null && this.lastEp.submittedEps.length > 0) {
        this.lastEp = this.lastEp.submittedEps
          .filter(item => item.enrolledProgramCd.trim() === this.currentProgram.programCd.trim())
          .sort((first: any, second: any) => {
            const a: Date = new Date(first.beginDate);
            const b: Date = new Date(second.beginDate);
            return a.getTime() - b.getTime();
          });
        if (this.lastEp && this.lastEp.length > 0) {
          this.compareBeginDate = this.lastEp[this.lastEp.length - 1].beginDate;
          this.subsequentEPId = this.lastEp[this.lastEp.length - 1].id;
        } else {
          this.compareBeginDate = '01/01/0001';
        }
      }
    }
    this.employabilityPlanService.getDateForBackDating(this.currentProgram.programCd).subscribe(results => {
      this.subtractFromCurrentDate = results[0].maxDaysCanBackDate;
      this.validate(cont);

      if (this.isSectionValid === true) {
        this.saveEmployabilityPlan(cont, fromWarning);
        this.appService.componentDataModified.next({ dataModified: false });
      } else {
        this.isSaving = false;
      }
    });
  }

  private saveEmployabilityPlan(cont: boolean, fromWarning: boolean) {
    this.CleanseModelForApi();
    this.employabilityPlan.employabilityPlanStatusTypeId = Utilities.idByFieldDataName('In Progress', this.employabilityPlanStatusTypes);
    if (this.subsequentEPId === undefined || this.subsequentEPId == null) {
      this.subsequentEPId = 0;
    }

    const obs1 = this.employabilityPlanService.canSaveEP(this.participant.pin, this.participant.id, false, this.employabilityPlan);
    const obs2 = this.employabilityPlanService.saveEmployabilityPlan(this.participant.pin, this.employabilityPlan, this.subsequentEPId);

    if (!fromWarning) {
      obs1
        .pipe(
          concatMap(res => {
            if (res && this.preCheck(res)) return obs2;
            else return of(null);
          })
        )
        .subscribe(
          data => {
            this.saveEP(data, cont);
          },
          error => {
            this.error();
          }
        );
    } else {
      this.precheck.warnings = [];
      obs2.subscribe(
        data => {
          this.saveEP(data, cont);
        },
        error => {
          this.error();
        }
      );
    }
  }

  private preCheck(res: WhyReason) {
    this.precheck.errors = res.errors;
    this.precheck.warnings = res.warnings;
    return res.status === true && this.precheck.warnings && this.precheck.warnings.length === 0;
  }

  private saveEP(data: EmployabilityPlan, cont: boolean) {
    if (data && !data.errorMessage) {
      this.hadSaveError = false;
      this.isSaving = false;
      this.employabilityPlan = data;
      this.originalEmployabilityPlan = new EmployabilityPlan();
      EmployabilityPlan.clone(this.employabilityPlan, this.originalEmployabilityPlan);
      this.appService.employabilityPlan.next(this.employabilityPlan);

      if (cont) {
        this.router.navigateByUrl(`/pin/${this.participant.pin}/employability-plan/goals/${this.employabilityPlan.id}`);
      }
      this.hasTriedSave = false;
      this.appService.isEPUrlChangeBlocked = false;
      this.location.replaceState(`/pin/${this.participant.pin}/employability-plan/${this.employabilityPlan.id}`);
      this.disableFields = true;
      this.selectedProgramNameDisabled = true;
      this.isSaving = false;
    } else {
      this.precheck.errors = data ? (this.precheck.errors ? [...this.precheck.errors, data.errorMessage] : [data.errorMessage]) : this.precheck.errors;
      this.hadSaveError = false;
      this.isSaving = false;
    }
  }

  private error() {
    this.hadSaveError = true;
    this.isSaving = false;
  }

  // handleFileInput(Image) {
  //   this.employabilityPlanService.uploadDocument(Image).subscribe(
  //     data => {
  //       console.log('done');
  //     }
  //   );
  // }
}
