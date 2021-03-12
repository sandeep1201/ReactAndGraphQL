import { Component, Output, OnInit, OnChanges, Input, ComponentRef, EventEmitter, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { TestScore, ExamResult } from '../../../shared/models/test-scores';
import { TestScoresEditComponent } from '../../test-scores/edit/edit.component';
import { TestScoresService } from '../../../shared/services/test-scores.service';
import { ModalService } from 'src/app/core/modal/modal.service';

@Component({
  selector: 'app-test-score-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TestScoreCardComponent implements OnInit, OnChanges {
  @Input() testScores: TestScore[] = [];
  @Input() pin: string;
  @Input() isReadOnly = true;
  @Input() cardType: string;
  @Input() failStatusId: number;
  @Input() passStatusId: number;
  @Output() showEditModalWithId = new EventEmitter<[number, number]>();
  @Output() deletedTestScore = new EventEmitter<TestScore>();

  private modalServiceBase: ModalService;

  private currentTestScoreIndex: number;

  public displayedExamResults: ExamResult[] = [];
  public displayedTestScore: TestScore;
  public isInEdit = false;

  public isLoaded = false;
  readonly abeAse = 'ABE/ASE';
  readonly gedHsed = 'GED/HSED';
  readonly eslEll = 'ESL/ELL';

  // Card Columns.
  public isNrsColDisplayed = false;
  public isSplColDisplayed = false;
  public isScaleScoreColDisplayed = false;
  public isScoreColDisplayed = false;
  public isTotalScoreColDisplayed = false;
  public isVersionColDisplayed = false;
  public isLevelColDisplayed = false;
  public isDatePassedColDisplayed = false;
  public isPassFailColDisplayed = false;
  public isGradeEquivalencyColDisplayed = false;
  public isGedPassed = false;
  public inConfirmDeleteView = false;

  constructor(private modalService: ModalService, private testScoresService: TestScoresService, private cd: ChangeDetectorRef) {
    this.modalServiceBase = modalService;
  }

  ngOnInit() {
    this.displayLatestTest();
    this.isLoaded = true;
  }

  openAdd(id: number): void {
    this.showEditModalWithId.emit([id, this.displayedTestScore.typeId]);
  }

  clickTestDate(testScore: TestScore) {
    if (testScore == null || testScore.examResults == null) {
      return null;
    }
    this.currentTestScoreIndex = this.testScores.indexOf(testScore);
    this.displayedTestScore = testScore;
    this.displayedExamResults = Array.from(testScore.examResults);
    this.cd.detectChanges();
  }

  initCardType(cardType: string) {
    if (cardType === this.gedHsed) {
      this.configGedHsedCard();
    } else if (cardType === 'BEST') {
      this.configBestCard();
    } else if (cardType === 'TABE CLAS E') {
      this.configTabeClasECard();
    } else if (cardType === 'TABE 9 & 10' || cardType === 'TABE 11 & 12') {
      this.configAbeAseCard();
    } else if (cardType === 'CASAS') {
      this.configCasasCard();
    }
  }

  ngOnChanges() {
    if (this.isLoaded === true && this.testScores != null && this.testScores[this.currentTestScoreIndex] != null) {
      this.displayedTestScore = this.testScores[this.currentTestScoreIndex];
      this.displayedExamResults = this.testScores[this.currentTestScoreIndex].examResults;
    }

    this.initCardType(this.cardType);
    this.displayLatestTest();
  }

  displayLatestTest() {
    if (this.testScores != null) {
      this.clickTestDate(this.testScores[this.testScores.length - 1]);
    }
  }
  delete(ts: TestScore) {
    this.testScoresService.deleteTestScoreExam(ts.id).subscribe(data => {
      this.inConfirmDeleteView = false;
      this.deletedTestScore.emit(ts);
      this.displayLatestTest();
    });
  }

  // Score, Total Score, Date Passed, Pass/Fail Cols Displayed

  public configGedHsedCard() {
    this.isScoreColDisplayed = true;
    this.isTotalScoreColDisplayed = true;
    this.isDatePassedColDisplayed = true;
    this.isPassFailColDisplayed = true;

    // Using to determine if Ged is passing; removed per change requests.
    // if (this.testScores != null) {
    //   for (const ts of this.testScores) {
    //     for (const sb of ts.examResults) {
    //       if (sb.hasPassed === 1) {
    //         this.isGedPassed = true;
    //       } else {
    //         this.isGedPassed = false;
    //         break;
    //       }
    //     }
    //   }
    // }
  }

  // Tabe
  private configAbeAseCard() {
    this.isNrsColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isLevelColDisplayed = true;
    this.isVersionColDisplayed = true;
    this.isGradeEquivalencyColDisplayed = true;
  }

  // BEST and TABE CLAS-E
  private configTabeClasECard() {
    this.isLevelColDisplayed = true;
    this.isNrsColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isSplColDisplayed = true;
  }

  private configBestCard() {
    this.isVersionColDisplayed = true;
    this.isSplColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isNrsColDisplayed = true;
  }

  private configCasasCard() {
    this.isLevelColDisplayed = true;
    this.isNrsColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isGradeEquivalencyColDisplayed = true;
  }

  onConfirmDelete() {
    this.delete(this.displayedTestScore);
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  tryDelete() {
    this.inConfirmDeleteView = true;
  }
}
