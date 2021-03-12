// tslint:disable use-life-cycle-interface
import { Component, Input, forwardRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { GoalStep } from '../../models/goal-step.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Utilities } from 'src/app/shared/utilities';
import { Goal } from '../../models/goal.model';

@Component({
  selector: 'app-goal-step-repeater',
  templateUrl: './goal-step-repeater.component.html',
  styleUrls: ['./goal-step-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GoalStepRepeaterComponent),
      multi: true
    }
  ]
})
export class GoalStepRepeaterComponent extends BaseRepeaterComponent<GoalStep> implements ControlValueAccessor, OnChanges {
  @Input() goalTypeDrop: DropDownField[];

  @Input() originalModel: GoalStep[];

  @Input() isReadOnly: boolean;

  @Input() isDisabled: boolean;
  @Input() goal: Goal;
  @Input() canEditCompleted = false;
  public maxNumberOfItems = 10;
  private primaryEmploymentId: number;
  private secondaryEmploymentId: number;
  private educationGoalId: number;

  constructor() {
    super(GoalStep.create);
  }

  ngOnChanges() {}
  ngOnInit() {
    this.goal.goalSteps.forEach(goalStep => {
      //Adding goalStepCheck on the fly because, for a carry over goal, the goal step completed checkbox should be editable if it is not checked.
      //And the checkbox shouldn't be editable if it is already checked. If we use the isGoalStepCompleted property, the checkbox won't be editable once you check it even before save.
      goalStep.goalStepCheck = goalStep.isGoalStepCompleted;
      if (this.isReadOnly) {
        if (!!goalStep.isGoalStepCompleted === false && this.canEditCompleted) {
          goalStep.disabledCompleteButton = false;
        } else {
          goalStep.disabledCompleteButton = true;
        }
      }
    });

    this.primaryEmploymentId = Utilities.idByFieldDataName('Primary Employment Goal', this.goalTypeDrop);
    this.secondaryEmploymentId = Utilities.idByFieldDataName('Secondary Employment Goal', this.goalTypeDrop);
    this.educationGoalId = Utilities.idByFieldDataName('Education Goal', this.goalTypeDrop);
  }

  public getInitialModel(gsId: number) {
    if (this.originalModel.length > 0) {
      const model = this.originalModel.find(x => x.id === gsId);
      let details: string;
      if (model != null) {
        details = model.details;
      }
      return details;
    } else return null;
  }

  isRequired(i: number): boolean {
    if (this.innerValue[i] != null) {
      if ((this.goal.goalTypeId === this.primaryEmploymentId || this.goal.goalTypeId === this.educationGoalId) && i === 0) {
        return true;
      }
    }
    return false;
  }

  isDetailsNullOrEmpty(i: number) {
    if (this.innerValue[i].details == null || (this.innerValue[i].details != null && this.innerValue[i].details.toString().trim() === '')) {
      return true;
    } else {
      return false;
    }
  }
}
