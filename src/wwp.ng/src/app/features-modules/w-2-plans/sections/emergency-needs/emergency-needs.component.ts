import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { Utilities } from '../../../../shared/utilities';
import { ValidationManager } from '../../../../shared/models/validation-manager';
import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Component, Input, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { Router } from '@angular/router';
import { W2PlansService } from '../../services/w-2-plans.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-emergency-needs',
  templateUrl: './emergency-needs.component.html',
  styleUrls: ['./emergency-needs.component.scss']
})
export class EmergencyNeedsComponent implements OnInit {
  //public isLoaded = false;
  sectionName = W2PlanSections.EmergencyNeeds;
  sectionNameForId = 'EmergencyNeeds';
  constructor() {}

  ngOnInit() {}
}
