import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseOverviewSecton } from '../overview/base-overview';
import { AppService } from 'src/app/core/services/app.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { PostSecondaryEducationSection } from '../../../shared/models/post-secondary-education-section';

@Component({
  selector: 'app-post-secondary-education-overview',
  templateUrl: 'post-secondary-education.component.html',
  styleUrls: ['./overview.css']
})
export class PostSecondaryEducationOverviewComponent extends BaseOverviewSecton implements OnInit, OnChanges, OnDestroy {
  @Input() section: PostSecondaryEducationSection;
  @Input() validationManager: ValidationManager;
  @Input() hasFcdpRole = false;

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

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new PostSecondaryEducationSection();
      PostSecondaryEducationSection.clone(this.section, this.cachedSection);
    }
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/post-secondary`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'post-secondary', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isPostSecondaryHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
