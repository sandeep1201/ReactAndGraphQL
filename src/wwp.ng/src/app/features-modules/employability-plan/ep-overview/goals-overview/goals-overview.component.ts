import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Goal } from '../../models/goal.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';

@Component({
  selector: 'app-goals-overview',
  templateUrl: './goals-overview.component.html',
  styleUrls: ['./goals-overview.component.scss']
})
export class GoalsOverviewComponent implements OnInit {
  @Input() pin: number;
  @Input() epId: number;
  @Input() goals: Goal[];
  @Output() edit = new EventEmitter<string>();
  @Input() showEdit = true;
  employabilityPlanId: number;
  constructor(private route: ActivatedRoute, private router: Router, private employabilityPlanService: EmployabilityPlanService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.employabilityPlanId = params.id;
    });
  }
  editSection() {
    this.edit.emit('goals');
  }
  singleEntry(a) {
    this.employabilityPlanService.EditGoalSection.next({ readOnly: true, inEditView: true, goal: a, isHistory: false });
    this.router.navigateByUrl(`pin/${this.pin}/employability-plan/goals/${this.employabilityPlanId}`);
  }
}
