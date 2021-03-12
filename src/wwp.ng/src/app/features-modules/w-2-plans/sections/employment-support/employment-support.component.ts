import { W2PlansService } from '../../services/w-2-plans.service';
import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { Utilities } from 'src/app/shared/utilities';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-employment-support',
  templateUrl: './employment-support.component.html',
  styleUrls: ['./employment-support.component.scss']
})
export class EmploymentSupportComponent implements OnInit {
  sectionName = W2PlanSections.EmploymentSupport;
  sectionNameForId = 'EmploymentSupport';
  constructor() {}

  ngOnInit() {}
}
