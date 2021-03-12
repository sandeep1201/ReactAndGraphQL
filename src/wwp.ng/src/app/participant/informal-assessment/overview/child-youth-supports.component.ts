import { Component, OnInit, Input, OnDestroy, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AppService } from 'src/app/core/services/app.service';
import { BaseOverviewSecton } from '../overview/base-overview';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ChildAndYouthSupportsSection, Child, Teen } from '../../../shared/models/child-youth-supports-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';

@Component({
  selector: 'app-child-youth-supports-overview',
  templateUrl: 'child-youth-supports.component.html',
  styleUrls: ['./overview.css']
})
export class ChildYouthSupportsOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy, OnChanges {
  @Input()
  section: ChildAndYouthSupportsSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;

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
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
    });
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new ChildAndYouthSupportsSection();
      ChildAndYouthSupportsSection.clone(this.section, this.cachedSection);
      if (this.section.hasChildren) {
        if (this.section.children.length === 0) {
          const x = new Child();
          x.id = 0;
          this.section.children.push(x);
        }
      }
      if (this.section.hasTeensWithDisabilityInNeedOfChildCare) {
        if (this.section.teens.length === 0) {
          const x = new Child();
          x.id = 0;
          this.section.teens.push(x);
        }
      }
    }
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'child-youth-supports', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isChildCareHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/child-youth-supports`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }
}
