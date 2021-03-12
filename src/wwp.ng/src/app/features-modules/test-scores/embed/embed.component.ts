import { Component, ComponentRef, Input, OnInit, OnDestroy, OnChanges } from '@angular/core';

import { FieldDataService } from '../../../shared/services/field-data.service';
import { PaginationInstance } from 'ng2-pagination';
import { Utilities } from '../../../shared/utilities';
import { TestScore, ExamResult } from '../../../shared/models/test-scores';
import { TestScoresService } from '../../../shared/services/test-scores.service';
import { Subscription } from 'rxjs';
import { ModalService } from 'src/app/core/modal/modal.service';
import { TestScoresEditComponent } from '../../test-scores/edit/edit.component'

declare var $: any;

@Component({
  selector: 'app-test-scores-embed',
  templateUrl: './embed.component.html',
  styleUrls: ['./embed.component.css'],
  providers: [TestScoresService]
})
export class TestScoresEmbedComponent implements OnInit, OnDestroy, OnChanges {

  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 1,
    currentPage: 1
  };

  @Input() isReadOnly: boolean = false;
  @Input() pin: string;
  private tabSub: Subscription;
  private testTypesSub: Subscription;
  private tempModalRef: ComponentRef<TestScoresEditComponent>;
  private modalServiceBase: ModalService;
  private gedHsedId: number;
  public model: TestScore[];
  public pinTestScoresUrl: string;
  public displayedExamResult: ExamResult[];
  public editExamId: number;
  public defaultExamTypeId: number;
  public isInEdit = false;
  public inConfirmDeleteView = false;
  readonly gedHsed = 'GED/HSED';
  constructor(private testScoresService: TestScoresService, private fdService: FieldDataService, modalService: ModalService) {
    this.modalServiceBase = modalService;
  }
  ngOnInit() {
    this.testScoresService.setPin(this.pin);
    this.testTypesSub = this.fdService.getEducationTestTypes().subscribe(data => this.initEducationTestTypesDrop(data));
    this.getTestScores();
  }

  initTestScores(data) {
    this.model = data;
    if (this.model != null) {
      this.config.currentPage = this.model.length;
    }
  }
  private getTestScores(): void {
    this.tabSub = this.testScoresService.getTestScoreByExam('ged').subscribe(data => {
      if (data != null) {
        // Map Data
        this.initTestScores(data);
      }
    })
  }

  initEducationTestTypesDrop(data) {
    this.gedHsedId = Utilities.idByFieldDataName('GED/HSED', data);
    this.defaultExamTypeId = this.gedHsedId;
  }

  openAdd(isNew: boolean): void {
    if (isNew === true) {
      this.editExamId = 0;
    } else {
      this.editExamId = this.model[this.config.currentPage - 1].id;
    }
    this.isInEdit = true;

  }

  handleModalSaved(hasSaved: boolean) {
      this.getTestScores();
      this.isInEdit = false;
  }

  handleModalClosed() {
    this.isInEdit = false;
  }


  delete() {
    if (this.model != null) {
      const id = this.model[this.config.currentPage - 1].id;
      this.testScoresService.deleteTestScoreExam(id).subscribe(data => this.getTestScores());

      // Remember to remove dialog when deleting.
      this.inConfirmDeleteView = false;
    }
  }

  onConfirmDelete() {
    this.delete();
  }


  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  tryDelete() {
    this.inConfirmDeleteView = true;
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  ngOnChanges() {
    if (this.pin != null) {
      this.pinTestScoresUrl = `/pin/${this.pin}/test-scores`;
    }
  }

  ngOnDestroy() {
    if (this.tabSub != null) {
      this.tabSub.unsubscribe();
    }
    if (this.testTypesSub != null) {
      this.testTypesSub.unsubscribe();
    }
  }
}
