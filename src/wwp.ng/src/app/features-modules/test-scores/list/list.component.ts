import { Component, OnInit, Input, ComponentRef, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { Subscription } from 'rxjs';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { ParticipantService } from '../../../shared/services/participant.service';
import { TestScoresService } from '../../../shared/services/test-scores.service';
import { TestScore } from '../../../shared/models/test-scores';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Utilities } from '../../../shared/utilities';

import * as _ from 'lodash';

@Component({
  selector: 'app-test-scores-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css'],
  providers: [FieldDataService, TestScoresService]
})
export class TestScoresListComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public isInEdit = false;
  public goBackUrl: string;

  private modalServiceBase: ModalService;
  private tabSub: Subscription;
  private educationTestTypesDrop: DropDownField[];
  private gedHsedId: number;
  private tabe912Id: number;
  private tabe1112Id: number;
  private tableClasEId: number;
  private bestId: number;
  private casasId: number;

  public testScores: TestScore[];
  public gedExams = [];
  public tabe910Exams = [];
  public tabe1112Exams = [];
  public bestExams = [];
  public tabeClasEExams = [];
  public casasExams = [];
  public testType = '';

  public isGedCardDisplayed = false;
  public isTabe910CardDisplayed = false;
  public isTabe1112CardDisplayed = false;
  public isBestCardDisplayed = false;
  public isTabeClasECardDisplayed = false;
  public isCasasCardDisplayed = false;

  readonly abeAse = 'Prior TABE';
  readonly gedHsed = 'GED/HSED';
  readonly eslEll = 'ESL/ELL';

  readonly tabe910Name = 'TABE 9 & 10';
  readonly tabe1112Name = 'TABE 11 & 12';
  readonly bestName = 'BEST';
  readonly tabeClassEName = 'TABE CLAS-E';
  readonly casasName = 'CASAS';

  public failStatusId: number;
  public passStatusId: number;
  public examId: number;
  public currentCardName = '';
  public defaultExamTypeId: number;

  @Input() pin: string;
  @Input() isReadOnly = true;

  public update(event) {
    this.getTestScores(true);
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    this.isLoaded = true;
  }

  ngOnInit() {
    this.testScoresService.setPin(this.pin);
    this.fdService.getEducationTestTypes().subscribe(data => this.initEducationTestTypesDrop(data));
    this.fdService.getEducationTestStatuses().subscribe(data => this.initEducationTestStatusesDrop(data));
    this.getTestScores(true);
    // Defaults to ged tab for now.
    //this.clickTestType(this.gedHsed);
  }

  showLatestExam() {
    if (this.testScores == null) {
      return;
    }

    const sortedByDate = _.sortBy(this.testScores, [
      function(o) {
        return new Date(o.dateTaken);
      }
    ]);

    const length = sortedByDate.length;
    const ts = sortedByDate[length - 1];

    if (ts == null) {
      return;
    }

    if (ts.name === this.tabe910Name) {
      this.clickTestType(this.abeAse);
    } else if (ts.name === this.tabe1112Name) {
      this.clickTestType(this.tabe1112Name);
    } else if (ts.name === this.gedHsed) {
      this.clickTestType(this.gedHsed);
    } else if (ts.name === this.bestName) {
      this.clickTestType(this.eslEll);
    } else if (ts.name === this.tabeClassEName) {
      this.clickTestType(this.eslEll);
    } else if (ts.name === this.casasName) {
      this.clickTestType(this.casasName);
    }
  }

  private getTestScores(showLatestExam: boolean, examTypeId?: number): void {
    this.tabSub = this.testScoresService.getTestScoreList().subscribe(data => {
      if (data != null) {
        // Map Data.
        this.initTestScores(data);
        if (showLatestExam === true) {
          this.showLatestExam();
        } else {
          if (examTypeId > 0) {
            const tabName = this.tabNameByExamTypeId(examTypeId);
            if (tabName === this.tabe910Name && this.tabe910Exams != null && this.tabe910Exams.length > 0) {
              this.clickTestType(this.abeAse);
            } else if (tabName === this.tabe1112Name && this.tabe1112Exams != null && this.tabe1112Exams.length > 0) {
              this.clickTestType(this.tabe1112Name);
            } else if (tabName === this.gedHsed && this.gedExams != null && this.gedExams.length > 0) {
              this.clickTestType(this.gedHsed);
            } else if (tabName === this.eslEll && this.bestExams != null && this.bestExams.length > 0) {
              this.clickTestType(this.eslEll);
            } else if (tabName === this.eslEll && this.tabeClasEExams != null && this.tabeClasEExams.length > 0) {
              this.clickTestType(this.eslEll);
            } else if (tabName === this.casasName && this.casasExams != null && this.casasExams.length > 0) {
              this.clickTestType(this.casasName);
            } else {
              this.showLatestExam();
            }
          }
        }
      }
    });
  }

  constructor(
    route: ActivatedRoute,
    router: Router,
    private testScoresService: TestScoresService,
    partService: ParticipantService,
    private modalService: ModalService,
    private fdService: FieldDataService
  ) {
    super(route, router, partService);
    this.modalServiceBase = modalService;
  }

  initTestScores(data) {
    // Slice does a copy of data.
    this.testScores = data.slice(0);
    this.initCards();
  }

  initCards() {
    if (this.testScores == null) {
      return;
    }

    this.gedExams = [];
    this.tabe910Exams = [];
    this.tabe1112Exams = [];
    this.bestExams = [];
    this.tabeClasEExams = [];
    this.casasExams = [];

    const gedExams = [];
    const tabe910Exams = [];
    const bestExams = [];
    const tabeClasEExams = [];
    const tabe1112Exams = [];
    const casasExams = [];

    for (const ts of Array.from(this.testScores)) {
      if (ts.name === this.tabe910Name) {
        tabe910Exams.push(ts);
      } else if (ts.name === this.tabe1112Name) {
        tabe1112Exams.push(ts);
      } else if (ts.name === this.gedHsed) {
        gedExams.push(ts);
      } else if (ts.name === this.bestName) {
        bestExams.push(ts);
      } else if (ts.name === this.tabeClassEName) {
        tabeClasEExams.push(ts);
      } else if (ts.name === this.casasName) {
        casasExams.push(ts);
      }
    }

    this.gedExams = Array.from(gedExams);
    this.tabe910Exams = tabe910Exams;
    this.tabe1112Exams = tabe1112Exams;
    this.bestExams = bestExams;
    this.tabeClasEExams = tabeClasEExams;
    this.casasExams = casasExams;
  }

  initEducationTestStatusesDrop(data) {
    this.passStatusId = Utilities.idByFieldDataName('Pass', data);
    this.failStatusId = Utilities.idByFieldDataName('Fail', data);
  }

  initEducationTestTypesDrop(data) {
    this.gedHsedId = Utilities.idByFieldDataName(this.gedHsed, data);
    this.tabe912Id = Utilities.idByFieldDataName(this.tabe910Name, data);
    this.tabe1112Id = Utilities.idByFieldDataName(this.tabe1112Name, data);
    this.bestId = Utilities.idByFieldDataName(this.bestName, data);
    this.tableClasEId = Utilities.idByFieldDataName(this.tabeClassEName, data);
    this.casasId = Utilities.idByFieldDataName(this.casasName, data);
    this.educationTestTypesDrop = data;
  }

  resetDisplayedCards() {
    this.isGedCardDisplayed = false;
    this.isTabe910CardDisplayed = false;
    this.isTabe1112CardDisplayed = false;
    this.isBestCardDisplayed = false;
    this.isTabeClasECardDisplayed = false;
    this.isCasasCardDisplayed = false;
  }

  clickTestType(type: string) {
    this.resetDisplayedCards();
    if (type === this.gedHsed) {
      this.isGedCardDisplayed = true;
    } else if (type === this.tabe1112Name) {
      this.isTabe1112CardDisplayed = true;
    } else if (type === this.abeAse) {
      this.isTabe910CardDisplayed = true;
    } else if (type === this.eslEll) {
      this.isBestCardDisplayed = true;
      this.isTabeClasECardDisplayed = true;
    } else if (type === this.casasName) {
      this.isCasasCardDisplayed = true;
    }

    this.initCards();
    this.testType = type;
  }

  handleModelSaved(savedTest: TestScore) {
    // If we get here, saving was successful.
    this.getTestScores(false, savedTest.typeId);
    //this.clickTestType(this.tabNameByExamTypeId(savedTest.typeId));
    this.isInEdit = false;
  }

  tabNameByExamTypeId(typeId: number): string {
    typeId = +typeId;
    switch (typeId) {
      case this.gedHsedId:
        return this.gedHsed;
      case this.tabe912Id:
        return this.abeAse;
      case this.bestId:
        return this.eslEll;
      case this.tableClasEId:
        return this.eslEll;
      case this.casasId:
        return this.casasName;
      case this.tabe1112Id:
        return this.tabe1112Name;
    }
  }

  handleModelDeleted(deletedTest: TestScore) {
    // If we get here, saving was successful and item has been deleted.
    this.getTestScores(false, deletedTest.typeId);
    //this.clickTestType(this.tabNameByExamTypeId(deletedTest.typeId));
    this.isInEdit = false;
  }

  handleModalClosed() {
    this.isInEdit = false;
  }

  openEdit(idAndTypeId): void {
    this.examId = idAndTypeId[0];
    if (idAndTypeId[1] != null) {
      this.defaultExamTypeId = idAndTypeId[1];
    }
    this.isInEdit = true;
  }

  openAdd(): void {
    this.examId = 0;
    this.defaultExamTypeId = null;
    this.isInEdit = true;
  }

  ngOnDestroy() {
    if (this.tabSub != null) {
      this.tabSub.unsubscribe();
    }
  }
}
