import { Component, Input, forwardRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { ActivitySchedule } from '../../../models/activity-schedule.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { EmployabilityPlan } from '../../../models/employability-plan.model';
import { Activity } from '../../../models/activity.model';
import { Utilities } from 'src/app/shared/utilities';
import { IMultiSelectSettings } from 'src/app/shared/components/multi-select-dropdown/multi-select-dropdown.component';

@Component({
  selector: 'app-schedule-repeater',
  templateUrl: './schedule-repeater.component.html',
  styleUrls: ['./schedule-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ScheduleRepeaterComponent),
      multi: true
    }
  ]
})
export class ScheduleRepeaterComponent extends BaseRepeaterComponent<ActivitySchedule> implements ControlValueAccessor, OnChanges {
  @Input() amPmTab: { id: number; name: string }[];
  @Input() endDateRequired: boolean;
  public frequencyRequired: boolean;
  @Input() frequencyDropDown: DropDownField[];
  @Input() daysDropDown: DropDownField[];

  @Input() isReadOnly: boolean;
  @Input() isReadOnlySchedule: boolean;
  @Input() isDisabled: boolean;

  @Input() monthlyRecurrenceDropDown: DropDownField[];
  @Input() hours: DropDownField[];
  @Input() minutes: DropDownField[];
  @Input() set updateActivityScheduler(obj: any) {
    this.writeValue(obj);
  }
  @Input() activityScheduleInit: ActivitySchedule[];
  @Input() employabilityPlan: EmployabilityPlan;
  @Input() activity: Activity;

  public isItemChanged = true;
  private frequencyTypeId: number;
  private originalActivity: Activity = new Activity();
  private originalDays: Array<{ id: number; days: any[] }> = [];
  constructor() {
    super(ActivitySchedule.create);
  }

  // tslint:disable-next-line: use-life-cycle-interface
  ngOnInit() {
    if (this.activity && this.activity.id > 0) Activity.clone(this.activity, this.originalActivity);

    if (this.originalActivity && this.originalActivity.activitySchedules) {
      this.originalActivity.activitySchedules.forEach(schedule => {
        this.originalDays.push({ id: schedule.id, days: schedule.wkFrequencyIds });
      });
    }

    if (this.activityScheduleInit) {
      this.activityScheduleInit.forEach(item => {
        this.onSelectTime(item);
      });
    }
  }

  ngOnChanges() {
    if (this.frequencyDropDown != null) {
      this.frequencyTypeId = Utilities.idByFieldDataName('Other', this.frequencyDropDown);
    }
  }
  onSelectFrequency(item) {
    this.isItemChanged = false;
    const originalActivity = this.cachedInitialModels.find(activity => activity.id === item.id);
    if (originalActivity !== undefined) {
      if (item.frequencyTypeId === originalActivity.frequencyTypeId) {
        item.wkFrequencyIds = originalActivity.wkFrequencyIds;
        item.wkFrequencyId = originalActivity.wkFrequencyId;
      } else {
        item.wkFrequencyIds = [];
        item.wkFrequencyId = null;
      }
      setTimeout(() => {
        this.isItemChanged = true;
      }, 0);
    } else {
      item.wkFrequencyIds = [];
      item.wkFrequencyId = null;
    }
  }

  checkState() {
    this.onDirty.emit(true);
  }
  onSelectTime(item) {
    item.isTimeRequired = false;
    // tslint:disable-next-line: triple-equals
    if (item.beginHour != undefined || item.beginMinute != undefined || item.endHour != undefined || item.endMinute != undefined) {
      item.isTimeRequired = true;
    } else {
      item.beginAmPm = null;
      item.endAmPm = null;
    }
  }

  getCachedDays(id: number) {
    let origDays = [];
    if (this.originalDays) {
      const originalDay = this.originalDays.filter(i => i.id === id)[0];
      if (originalDay && originalDay.days) origDays = originalDay.days;
    }
    return origDays;
  }
}
