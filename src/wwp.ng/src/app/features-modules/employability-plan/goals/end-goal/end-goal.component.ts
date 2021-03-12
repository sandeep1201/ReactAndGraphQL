import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Goal } from '../../models/goal.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-end-goal',
  templateUrl: './end-goal.component.html',
  styleUrls: ['./end-goal.component.scss']
})
export class EndGoalComponent implements OnInit, OnDestroy {
  constructor() {}

  @Input() goal: Goal;

  @Input() modelErrors;

  @Output() endGoalValidation = new EventEmitter();

  @Input() endGoalTypesDrop: DropDownField[];
  @Input() originalGoal;
  ngOnInit() {}

  ngOnDestroy() {}
}
