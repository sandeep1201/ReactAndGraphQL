import { Component, OnInit, Input, OnDestroy, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AppService } from 'src/app/core/services/app.service';
import { BaseOverviewSecton } from '../overview/base-overview';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { LanguagesSection } from '../../../shared/models/languages-section';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ValidationManager } from '../../../shared/models/validation-manager';

import * as _ from 'lodash';

@Component({
  selector: 'app-languages-overview',
  templateUrl: 'languages.component.html',
  styleUrls: ['./overview.css']
})
export class LanguagesOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy, OnChanges {
  @Input()
  section: LanguagesSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;
  public englishId: number;
  private routeSub: Subscription;

  constructor(
    public appService: AppService,
    public iaService: InformalAssessmentService,
    private route: ActivatedRoute,
    public modalService: ModalService,
    private router: Router
  ) {
    super(appService, iaService, modalService);
  }

  ngOnInit() {
    this.englishId = this.iaService.englishId;
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new LanguagesSection();
      LanguagesSection.clone(this.section, this.cachedSection);
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/languages`;
    this.router.navigateByUrl(route);
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'languages', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isLanguageHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }
}
