import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { Router } from '@angular/router';
import { W2PlansService } from '../../services/w-2-plans.service';
import { take } from 'rxjs/operators';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Utilities } from 'src/app/shared/utilities';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';

@Component({
  selector: 'app-health',
  templateUrl: './health.component.html',
  styleUrls: ['./health.component.scss']
})
export class HealthComponent implements OnInit {
  sectionName = W2PlanSections.Health;
  sectionNameForId = 'Health';
  constructor() {}

  ngOnInit() {}
}
