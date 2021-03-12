import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { AppService } from 'src/app/core/services/app.service';
import { Router } from '@angular/router';
import { W2PlansService } from '../../services/w-2-plans.service';
import { take } from 'rxjs/operators';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-transportation',
  templateUrl: './transportation.component.html',
  styleUrls: ['./transportation.component.scss']
})
export class TransportationComponent implements OnInit {
  sectionName = W2PlanSections.Transportation;
  sectionNameForId = 'Transportation';
  constructor() {}

  ngOnInit() {}
}
