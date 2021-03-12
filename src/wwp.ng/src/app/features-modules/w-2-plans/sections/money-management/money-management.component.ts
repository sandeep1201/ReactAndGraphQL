import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { W2PlansService } from '../../services/w-2-plans.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-money-management',
  templateUrl: './money-management.component.html',
  styleUrls: ['./money-management.component.scss']
})
export class MoneyManagementComponent implements OnInit {
  sectionName = W2PlanSections.LegalAssistance;

  sectionNameForId = 'LegalAssistance';

  constructor() {}

  ngOnInit() {}
}
