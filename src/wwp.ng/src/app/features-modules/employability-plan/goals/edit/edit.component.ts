import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { Component, EventEmitter, OnDestroy, OnInit, Output, Input } from '@angular/core';
// tslint:disable-next-line: import-blacklist
import { Observable, forkJoin } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { take, concatMap } from 'rxjs/operators';
import { Goal } from '../../models/goal.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { EmployabilityPlan } from '../../models/employability-plan.model';
import { GoalStep } from '../../models/goal-step.model';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { ValidationManager } from 'src/app/shared/models/validation';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { Dictionary } from 'src/app/shared/dictionary';
import { AppService } from 'src/app/core/services/app.service';
import { EmployabilityPlanStatus } from '../../enums/employability-plan-status.enum';

@Component({
  selector: 'app-goals-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class GoalsEditComponent implements OnInit, OnDestroy {
  showEndReasons: boolean;
  showControl: boolean;
  // tslint:disable: no-output-on-prefix
  @Output() onExitEditMode = new EventEmitter<boolean>();
  @Output() onExit = new EventEmitter();
  @Input() goalInput: Goal;
  @Input() isReadOnly: boolean;
  @Input() inHistory = false;
  @Input() goalsInput: Goal[] = [];

  @Output() endGoalTypesDrop: DropDownField[] = [];
  @Output() originalGoal: Goal = new Goal();

  public ep: EmployabilityPlan;
  public pin: string;
  public employabilityPlanId: string;
  public epBeginDate;
  public epEnrolledProgramId: string;
  public goal: Goal;
  cleansedGoalSteps: GoalStep[] = [];
  public hasTriedSave = false;
  public isSectionModified = false;
  public isLoaded = false;
  public enrolledProgramCode: string;
  public programDrop: DropDownField[] = [];
  public isSectionValid = true;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public goalTypesDrop: DropDownField[] = [];
  //public originalGoal: Goal = new Goal();
  public cachedGoals: Goal[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSaving = false;
  public canSaveOnReadOnly = false;

  public canEditCompleted = false;

  constructor(
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private router: Router,
    public appService: AppService,
    private employabilityPlanService: EmployabilityPlanService
  ) {}
  ngOnInit() {
    // this.pin = this.route.parent.snapshot.paramMap.get('pin');
    // this.route.params.subscribe(params => {
    //   this.employabilityPlanId = params.id;
    // });
    // this.appService.employabilityPlan
    //   .switchMap(ep => {
    //     this.ep = ep;
    //     return this.fdService.getGoalTypes(this.ep.enrolledProgramId);
    //   })
    //   .subscribe(results => {
    //     this.newGoal();
    //     Goal.clone(this.goal, this.originalGoal);
    //     this.initGoalTypesDrop(results);
    //     this.isLoaded = true;
    //   });
    this.employabilityPlanService.EditGoalSection.subscribe(goalObj => {
      this.showControl = !goalObj.showControl;
      this.inHistory = goalObj.inHistory;
    });
    this.requestDatafromMultipleSources()
      .pipe(take(1))
      .subscribe(results => {
        this.pin = results[0].pin;
        this.employabilityPlanId = results[1].id;
        this.employabilityPlanService
          .getEpById(this.pin, this.employabilityPlanId)
          .pipe(
            concatMap(res => {
              this.ep = res;
              return this.fdService.getGoalTypes(this.ep.enrolledProgramId);
            })
          )
          .subscribe(data => {
            if (this.goalInput) {
              this.employabilityPlanService.getGoal(this.pin, this.goalInput.id.toString()).subscribe(goalRes => {
                this.goal = goalRes;
                this.initGoalTypesDrop(data);
                Goal.clone(this.goal, this.originalGoal);
                this.spliceGoal();
                // this.isLoaded = true;
              });
            } else {
              this.newGoal();
              this.goal.goalSteps = [];
              Goal.clone(this.goal, this.originalGoal);
              this.spliceGoal();
              this.initGoalTypesDrop(data);
              // this.isLoaded = true;
            }
          });
      });
    // Dropdown values for end goals
    //To-DO: move this call to end-goal component (PA)
    this.fdService.getFieldDataByField(FieldDataTypes.GoalEndReasons).subscribe(result => {
      this.endGoalTypesDrop = result;
    });
  }

  private requestDatafromMultipleSources() {
    return forkJoin([this.route.parent.params.pipe(take(1)), this.route.params.pipe(take(1))]);
  }

  initBeginDate() {
    this.goal.beginDate = this.ep.beginDate;
  }

  initGoalTypesDrop(data) {
    this.goalTypesDrop = data;
  }

  newGoal() {
    const goal = new Goal();
    goal.id = 0;
    this.goal = goal;
    this.initBeginDate();
  }

  spliceGoal() {
    this.cachedGoals = this.goalsInput.slice(0);
    const index: number = this.cachedGoals.findIndex(i => i.id === this.goal.id);
    if (index !== -1) {
      this.cachedGoals.splice(index, 1);
    }
    this.appService.employabilityPlanInfo.subscribe(data => {
      if (
        data &&
        data.results &&
        data.results[0].every(ep => ep.employabilityPlanStatusTypeName !== EmployabilityPlanStatus.inProgress) &&
        data.results[0].some(ep => ep.employabilityPlanStatusTypeName === EmployabilityPlanStatus.submitted)
      ) {
        if (this.ep.employabilityPlanStatusTypeName === EmployabilityPlanStatus.submitted && this.goal.goalSteps.some(s => !!s.isGoalStepCompleted === false)) {
          this.canSaveOnReadOnly = true;
          this.canEditCompleted = true;
        }
      }
      this.isLoaded = true;
    });
  }

  showEndGoalReason() {
    this.showEndReasons = true;
    const endDate = moment(this.ep.beginDate).subtract(1, 'day');
    this.goal.endDate = endDate.format('MM/DD/YYYY');
  }

  save() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid === true) {
      this.saveGoal();
    } else {
      this.isSaving = false;
    }
  }

  private saveGoal() {
    this.CleanseModelForApi();
    this.employabilityPlanService.saveGoals(this.pin, this.ep.id, this.goal).subscribe(
      data => {
        this.goal = data;
        this.cachedGoals.push(this.goal);
        Goal.clone(this.goal, this.originalGoal);
        this.isSaving = false;
        // isReadOnly will be true only if you come from singleEntry view.
        this.onExitEditMode.emit(this.isReadOnly);
      },
      error => {
        this.isSaving = false;
      }
    );
  }

  private countGoalsByGoalType(): Dictionary<string, number> {
    if (this.cachedGoals) {
      const countDictionary: Dictionary<string, number> = new Dictionary<string, number>();

      for (const goal of this.cachedGoals) {
        if (countDictionary.containsKey(goal.goalTypeName)) {
          let countDictionaryValue = countDictionary.getValue(goal.goalTypeName);
          countDictionaryValue++;
          countDictionary.setValue(goal.goalTypeName, countDictionaryValue);
        } else {
          countDictionary.setValue(goal.goalTypeName, 1);
        }
      }
      return countDictionary;
    }
  }

  validate(): void {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.goal.validate(this.validationManager, this.countGoalsByGoalType(), this.goalTypesDrop, this.endGoalTypesDrop, this.showEndReasons);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
    }
  }

  CleanseModelForApi() {
    this.goal.goalSteps.forEach(originalGoalStep => {
      if (originalGoalStep.details != null && originalGoalStep.details.trim() !== '') {
        this.cleansedGoalSteps.push(originalGoalStep);
      }
    });

    if (this.cleansedGoalSteps.length > 0) this.goal.goalSteps = this.cleansedGoalSteps;
    else this.goal.goalSteps = [];
  }

  cancel() {
    const inHistory = this.inHistory;
    if (this.isSectionModified) {
      this.appService.isEPUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.onExitEditMode.emit();
    }

    if (this.isReadOnly && !inHistory) {
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlanId}`);
    }

    if (inHistory) {
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/historical-goals`);
      this.employabilityPlanService.EditGoalSection.next({ inHistory: false });
    }
  }
  exit(e) {
    if (e === true) {
      this.onExitEditMode.emit();
    }
  }
  exitGoalEditIgnoreChanges($event) {
    this.onExitEditMode.emit();
  }
  ngOnDestroy() {}
}
