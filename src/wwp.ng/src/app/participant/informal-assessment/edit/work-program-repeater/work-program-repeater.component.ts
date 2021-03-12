// tslint:disable: no-output-on-prefix
// tslint:disable: no-use-before-declare
import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import * as moment from 'moment';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { WorkProgram } from '../../../../shared/models/work-programs-section';
import { Utilities } from '../../../../shared/utilities';

const noop = () => {};

export const WORK_PROGRAM_REPEATER_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => WorkProgramRepeaterComponent),
  multi: true
};
@Component({
  selector: 'app-work-program-repeater',
  templateUrl: './work-program-repeater.component.html',
  styleUrls: ['./work-program-repeater.component.css'],
  providers: [WORK_PROGRAM_REPEATER_CONTROL_VALUE_ACCESSOR]
})
export class WorkProgramRepeaterComponent extends BaseRepeaterComponent<WorkProgram> implements ControlValueAccessor {
  public workStatusPastId;
  public workStatusCurrentId;
  public workStatusWaitlistId;

  public workProgramStatusDropList: DropDownField[];

  public workProgramNamesDropList: DropDownField[];

  public otherWorkProgramId: number;

  @Output() onDirty = new EventEmitter<boolean>();
  @Output() onCheckState = new EventEmitter<boolean>();

  @Input()
  set workProgramStatuses(data: DropDownField[]) {
    this.workProgramStatusDropList = data;
    if (this.workStatusPastId == null || this.workStatusCurrentId == null || this.workStatusWaitlistId == null) {
      this.workStatusPastId = Utilities.idByFieldDataName('Past', data);
      this.workStatusCurrentId = Utilities.idByFieldDataName('Current', data);
      this.workStatusWaitlistId = Utilities.idByFieldDataName('Waitlist', data);
    }
  }

  @Input()
  set workProgramNames(data: DropDownField[]) {
    this.workProgramNamesDropList = data;
    if (this.otherWorkProgramId == null) {
      this.otherWorkProgramId = Utilities.idByFieldDataName('Other', data);
    }
  }

  @Input() participantDOBMMmYyyy: moment.Moment;
  @Input() pin: string;

  @Input()
  set dropDownList(data: DropDownField[]) {
    this.workProgramStatusDropList = data;
  }

  constructor() {
    super(WorkProgram.create);
  }

  isStartDateRequired(wp: WorkProgram) {
    return wp.isStartDateRequired(this.workStatusPastId, this.workStatusCurrentId, this.workStatusWaitlistId);
  }

  isEndDateRequired(wp: WorkProgram) {
    return wp.isEndDateRequired(this.workStatusPastId, this.workStatusCurrentId, this.workStatusWaitlistId);
  }

  isWorkStatusNullOrEmpty(i: number) {
    if (this.innerValue[i].workStatus == null || (this.innerValue[i].workStatus != null && this.innerValue[i].workStatus.toString().trim() === '')) {
      return true;
    } else {
      return false;
    }
  }

  isWorkProgramNameNullOrEmpty(i: number) {
    if (this.innerValue[i].workProgram == null || (this.innerValue[i].workProgram != null && this.innerValue[i].workProgram.toString().trim() === '')) {
      return true;
    } else {
      return false;
    }
  }

  isRequired(i: number): boolean {
    if (this.innerValue[i] != null) {
      if (
        (this.innerValue[i].startDate != null && this.innerValue[i].startDate.length > 0) ||
        (this.innerValue[i].endDate != null && this.innerValue[i].endDate.length > 0) ||
        this.innerValue[i].details != null ||
        !this.isWorkStatusNullOrEmpty(i) ||
        !this.isWorkProgramNameNullOrEmpty(i) ||
        this.innerValue[i].location != null ||
        i === 0
      ) {
        return true;
      }
    }
    return false;
  }

  // Checks for dirty states.
  checkState() {
    this.onDirty.emit(true);
  }
}
