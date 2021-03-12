import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Goal } from '../../models/goal.model';

@Component({
  selector: 'app-goals-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class GoalsListComponent {
  @Input() goals: Goal[];
  @Input() isReadOnly = false;
  @Input() showControls = true;
  @Output() deleteGoal = new EventEmitter();
  @Output() goToSingleEntry = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  delete(a) {
    this.deleteGoal.emit(a);
  }

  singleEntry(a: Goal) {
    this.goToSingleEntry.emit(a);
  }
}
