import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { EmployabilityPlan } from '../../models/employability-plan.model';

@Component({
  selector: 'app-employability-plan-overview',
  templateUrl: './employability-plan-overview.component.html',
  styleUrls: ['./employability-plan-overview.component.scss']
})
export class EmployabilityPlanOverviewComponent implements OnInit {
  @Input() employabilityPlan: EmployabilityPlan;
  @Output() edit = new EventEmitter();
  @Input() showEdit = true;
  constructor() {}

  ngOnInit() {}

  editSection() {
    this.edit.emit('');
  }
}
