import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
// tslint:disable-next-line: import-blacklist
import { Subject } from 'rxjs';
import { take, switchMap } from 'rxjs/operators';
import { EmployabilityPlan } from '../../models/employability-plan.model';
import { Goal } from '../../models/goal.model';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { EmployabilityPlanService } from '../../services/employability-plan.service';

@Component({
  selector: 'app-goals-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class GoalsPageComponent implements OnInit {
  compareEpDate: string;
  @Input() isReadOnly = true;
  @Input() isHistory = false;
  employabilityPlanId: string;
  public employabilityPlan: EmployabilityPlan;
  goals: Goal[];
  goal: Goal;
  goalsLoaded = false;
  inEditView = false;
  pin: string;
  selectedIdForDeleting: number;
  inConfirmDeleteView = false;
  employeePlanObj: any;
  public hadSaveError = false;
  public isSaving = false;
  public errorMsg: string;

  destroy$ = new Subject();
  constructor(private participantService: ParticipantService, private employabilityPlanService: EmployabilityPlanService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
      this.employabilityPlanId = params.id;
    });
    this.employabilityPlanService
      .getEpById(this.pin, this.employabilityPlanId)
      .pipe(take(1))
      .subscribe(result => {
        this.compareEpDate = result.beginDate;
        this.getGoals();
        this.employabilityPlan = result;
      });

    this.employabilityPlanService.EditGoalSection.subscribe(res => {
      if (res.goal) {
        this.goal = res.goal;
      }
      this.inEditView = res.inEditView;
      this.isReadOnly = res.readOnly;
      this.isHistory = res.isHistory;
    });
  }
  add() {
    if (this.goal) {
      this.goal = null;
    }
    this.employabilityPlanService.EditGoalSection.next({ readOnly: false, inEditView: true, showControl: true, isHistory: false });
  }

  singleEntry(a) {
    this.employabilityPlanService.EditGoalSection.next({ readOnly: false, inEditView: true, showControl: a.showControl, isHistory: false });
    this.goal = a;
  }

  deleteGoal(e) {
    this.selectedIdForDeleting = e.id;
    this.inConfirmDeleteView = true;
  }

  onConfirmDelete() {
    this.employabilityPlanService.deleteGoal(this.pin, this.selectedIdForDeleting, this.employabilityPlanId).subscribe(res => {
      this.getGoals();
    });
    this.inConfirmDeleteView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  exitEditView(fromSingleEntry: boolean) {
    this.employabilityPlanService.EditGoalSection.next({ readOnly: false, inEditView: false });
    this.goalsLoaded = false;
    this.getGoals();
    if (fromSingleEntry) {
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlanId}`);
    }
  }

  getGoals() {
    if (+this.employabilityPlanId > 0) {
      this.participantService
        .getCurrentParticipant()
        .pipe(
          switchMap(data => {
            this.pin = data.pin;
            return this.employabilityPlanService.getGoals(data.pin, this.employabilityPlanId);
          })
        )
        .subscribe(data => {
          this.goals = data;
          this.goals.forEach(goal => {
            goal['showControl'] = goal.beginDate === this.compareEpDate;
          });
          this.goalsLoaded = true;
        });
    }
  }

  saveAndContinue() {
    this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/employments/${this.employabilityPlanId}`);
  }

  // tslint:disable-next-line: use-life-cycle-interface
  ngOnDestroy() {
    this.destroy$.next();
  }
}
