import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subscription, Observable, forkJoin } from 'rxjs';

import { AppService } from '../../../core/services/app.service';
import { ExamResult, TestScore } from '../../../shared/models/test-scores';

import { FieldDataService } from '../../../shared/services/field-data.service';
import { TestScoresService } from '../../../shared/services/test-scores.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Participant } from '../../../shared/models/participant';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-testscores-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [TestScoresService]
})
export class TestScoresEditComponent implements OnInit, OnDestroy {
  @Output() hasSaved = new EventEmitter<TestScore>();
  @Output() close = new EventEmitter<boolean>();
  @Input() pin: string;
  @Input() examId: number;
  @Input() isExamTypeDisabled = false;
  @Input() examTypeId: number;

  public examSubjects: DropDownField[] = [];
  // Card Columns.
  public isNrsColDisplayed = false;
  public isSplColDisplayed = false;
  public isScaleScoreColDisplayed = false;
  public isTotalScoreColDisplayed = false;
  public isVersionColDisplayed = false;
  private _isLevelColDisplayed = false;
  private _isScoreColDisplayed = false;
  public isDatePassedColDisplayed = false;
  public isPassFailColDisplayed = false;
  public isGradeEquivalencyColDisplayed = false;
  public isDetailsDisplayed = false;
  public model: TestScore;
  public cached: TestScore;
  public existingTestScores: TestScore[];
  public gedHsedId: number;
  public tabe912Id: number;
  public tabe1112Id: number;
  public tableClasEId: number;
  public tableCasasId: number;
  public bestId: number;
  public mode = 'Add';
  public tableCols = 0;
  public hadSaveError = false;
  public isSaving = false;
  public examResults: ExamResult[] = [];
  public educationTestStatusesDrop: DropDownField[];
  public educationTestTypesDrop: DropDownField[];
  public nrsDrop: DropDownField[];
  public splDropSL: DropDownField[];
  public splDropRW: DropDownField[];
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  private examSubjectSub: Subscription;
  private testStatSub: Subscription;
  private testTypesSub: Subscription;
  private nrsTypesSub: Subscription;
  private splTypesSub: Subscription;
  private partSub: Subscription;
  private participantDob: string;
  private examGedSubjectsDrop: DropDownField[];
  private examBestSubjectsDrop: DropDownField[];
  private examCasasSubjectsDrop: DropDownField[];
  private examTabe910SubjectsDrop: DropDownField[];
  private examTabeClaseESubjectsDrop: DropDownField[];
  private validationManager: ValidationManager = new ValidationManager(this.appService);
  private hasloaded: boolean;

  constructor(private appService: AppService, private fdService: FieldDataService, private partService: ParticipantService, private testScoresService: TestScoresService) {}

  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  ngOnInit() {
    if (this.pin == null) {
      console.warn('PIN is not set');
    }
    this.testScoresService.setPin(this.pin);

    forkJoin(
      this.fdService.getExamSubjectsByExamUrl('ged').pipe(take(1)),
      this.fdService.getExamSubjectsByExamUrl('best').pipe(take(1)),
      this.fdService.getExamSubjectsByExamUrl('tabe910').pipe(take(1)),
      this.fdService.getExamSubjectsByExamUrl('tabeclase').pipe(take(1)),
      this.fdService.getExamSubjectsByExamUrl('casas').pipe(take(1)),
      this.fdService.getEducationTestTypes().pipe(take(1)),
      this.fdService.getNrsTypes().pipe(take(1)),
      this.fdService.getSplTypes().pipe(take(1)),
      this.fdService.getEducationTestStatuses().pipe(take(1)),
      this.testScoresService
        .getTestScoreList()
        .pipe(take(1))
        .pipe(take(1))
    ).subscribe(results => {
      this.initGedExamSubjectsDrop(results[0]);
      this.initBestExamSubjectsDrop(results[1]);
      this.initTabe910ExamSubjectsDrop(results[2]);
      this.initTabeClasEExamSubjectsDrop(results[3]);
      this.initCasasExamSubjectsDrop(results[4]);
      this.initEducationTestTypesDrop(results[5]);
      this.initNrsTypesDrop(results[6]);
      this.initSplTypesDrop(results[7]);
      this.initEducationTestStatusesDrop(results[8]);
      this.initTestScores(results[9]);
      // Load after field data loads.
      this.loadCardModel();
    });

    this.partSub = this.partService.getParticipant(this.testScoresService.getPin()).subscribe(data => this.initParticipantModel(data));
  }

  // getTableColumns Currently controls width of the details box.
  public getTableColumns() {
    this.tableCols = 0;
    if (this.isScoreColDisplayed()) {
      this.tableCols++;
    }
    if (this.isScaleScoreColDisplayed) {
      this.tableCols++;
    }
    if (this.isPassFailColDisplayed) {
      this.tableCols++;
    }
    if (this.isDatePassedColDisplayed) {
      this.tableCols++;
    }
    if (this.isVersionColDisplayed) {
      this.tableCols++;
    }
    if (this.isGradeEquivalencyColDisplayed) {
      this.tableCols++;
    }
    if (this.isLevelColDisplayed()) {
      this.tableCols++;
    }
    if (this.isNrsColDisplayed) {
      this.tableCols++;
    }
    if (this.isSplColDisplayed) {
      this.tableCols++;
    }
    if (this.model && this.model.typeId === this.tableCasasId) {
      this.tableCols++;
    }
  }

  loadCardModel() {
    if (this.examId == null || this.examId < 1) {
      this.model = new TestScore();
      this.model.typeId = this.examTypeId;
      this.cached = new TestScore();
      TestScore.clone(this.model, this.cached);
      if (this.model.typeId != null) {
        this.initCardType(this.model.typeId, true);
      }
    } else {
      // Disable exam type when we are loading existing.
      this.isExamTypeDisabled = true;
      this.mode = 'Edit';
      // Make request for data.
      this.testScoresService.getTestScoreById(this.examId).subscribe(dataExam => this.initModel(dataExam));
    }
  }

  initEducationTestStatusesDrop(data) {
    this.educationTestStatusesDrop = data;
  }

  initModel(data) {
    this.model = data;
    this.cached = new TestScore();
    TestScore.clone(data, this.cached);
    this.initCardType(this.model.typeId, false);
  }

  initParticipantModel(data: Participant) {
    this.participantDob = data.dateOfBirth;
  }

  initTestScores(data: TestScore[]) {
    this.existingTestScores = data;
  }

  initEducationTestTypesDrop(data) {
    this.gedHsedId = Utilities.idByFieldDataName('GED/HSED', data);
    this.tabe912Id = Utilities.idByFieldDataName('TABE 9 & 10', data);
    this.tabe1112Id = Utilities.idByFieldDataName('TABE 11 & 12', data);
    this.bestId = Utilities.idByFieldDataName('Best', data);
    this.tableClasEId = Utilities.idByFieldDataName('TABE CLAS-E', data);
    this.tableCasasId = Utilities.idByFieldDataName('CASAS', data);
    this.educationTestTypesDrop = data;
  }

  initNrsTypesDrop(data) {
    this.nrsDrop = data;
    this.nrsDrop.forEach(element => {
      element.name = element.id;
    });
  }

  initSplTypesDrop(data) {
    this.splDropSL = data;
    this.splDropRW = Array.from(data).slice(0, 9);
  }

  initGedExamSubjectsDrop(data) {
    this.examGedSubjectsDrop = data;
  }

  initBestExamSubjectsDrop(data) {
    this.examBestSubjectsDrop = data;
  }

  initCasasExamSubjectsDrop(data) {
    this.examCasasSubjectsDrop = data;
  }

  initTabe910ExamSubjectsDrop(data) {
    this.examTabe910SubjectsDrop = data;
  }
  initTabeClasEExamSubjectsDrop(data) {
    this.examTabeClaseESubjectsDrop = data;
  }

  exit() {
    this.close.emit(true);
  }

  isSaveEnabled(): boolean {
    this.validationManager.resetErrors();

    const result = this.model.validate(
      this.validationManager,
      this.participantDob,
      this.existingTestScores,
      this.isGradeEquivalencyColDisplayed,
      this.model.typeId === this.tableCasasId
    );

    // Update our properties so the UI can bind to the resultss.
    this.modelErrors = result.errors;

    return result.isValid;
  }

  save() {
    if (this.isSaveEnabled() === false) {
      return;
    }
    this.isSaving = true;
    this.testScoresService.postTestScoreExam(this.model).subscribe(
      resp => {
        //TODO: if(resp.ok) ? maybe?
        this.hasSaved.emit(this.model);
        this.exit();
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }

  ngOnDestroy() {
    if (this.testStatSub != null) {
      this.testStatSub.unsubscribe();
    }
    if (this.testTypesSub != null) {
      this.testTypesSub.unsubscribe();
    }
    if (this.nrsTypesSub != null) {
      this.nrsTypesSub.unsubscribe();
    }
    if (this.splTypesSub != null) {
      this.splTypesSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
    if (this.examSubjectSub != null) {
      this.examSubjectSub.unsubscribe();
    }
  }

  public isLevelColDisplayed(examSubject?: string, typeId?: number): boolean {
    // examSubject === 'Speaking/Listening' &&
    if (examSubject === 'Reading/Writing' && Number(typeId) === this.bestId) {
      return false;
    } else {
      return this._isLevelColDisplayed;
    }
  }

  public isScoreColDisplayed(examSubject?: string, typeId?: number) {
    if ((examSubject === 'Civics' || examSubject === 'Health') && Number(typeId) === this.gedHsedId) {
      return false;
    } else {
      return this._isScoreColDisplayed;
    }
  }

  initCardType(cardTypeId: number, isNew: boolean) {
    if (+cardTypeId === this.gedHsedId) {
      this.configGedHsedCard(isNew);
      this.getTableColumns();
    } else if (+cardTypeId === this.tabe912Id) {
      this.configAbeAseCard(isNew);
      this.getTableColumns();
    } else if (+cardTypeId === this.tabe1112Id) {
      this.configAbeAseCard(isNew);
      this.getTableColumns();
    } else if (+cardTypeId === this.bestId) {
      this.configBestCard(isNew);
      this.getTableColumns();
    } else if (+cardTypeId === this.tableClasEId) {
      this.configTabeClasECard(isNew);
      this.getTableColumns();
    } else if (+cardTypeId === this.tableCasasId) {
      this.configCasasCard(isNew);
      this.getTableColumns();
    } else {
      this.resetConfigCard();
    }
  }

  private resetConfigCard() {
    //this.model.examResults = [];
    this.isNrsColDisplayed = false;
    this.isSplColDisplayed = false;
    this.isScaleScoreColDisplayed = false;
    this._isScoreColDisplayed = false;
    this.isTotalScoreColDisplayed = false;
    this.isVersionColDisplayed = false;
    this._isLevelColDisplayed = false;
    this.isDatePassedColDisplayed = false;
    this.isPassFailColDisplayed = false;
    this.isGradeEquivalencyColDisplayed = false;
    this.isDetailsDisplayed = false;
  }

  // GED
  private configGedHsedCard(isNew: boolean) {
    // this.examSubjects = ['Languages', 'Mathematics', 'Science', 'Social Studies', 'Civics', 'Health']
    this.resetConfigCard();
    //this.examSubjects = this.examGedSubjectsDrop;
    if (isNew === true) {
      this.model.examResults = this.examGedSubjectsDrop.map(function(subject) {
        const x = new ExamResult();
        x.name = subject.name;
        if (x.name !== 'Civics' && x.name !== 'Health') {
          x.totalScore = 200;
        }
        x.subjectId = subject.id;
        return x;
      });
    }

    this._isScoreColDisplayed = true;
    this.isTotalScoreColDisplayed = true;
    this.isPassFailColDisplayed = true;
    this.isDetailsDisplayed = true;
  }

  public configSplDrop(subjectName?: string) {
    if (this.splDropRW == null || this.splDropSL == null) {
      return;
    }
    if (subjectName === 'Reading/Writing') {
      return this.splDropRW;
    } else {
      return this.splDropSL;
    }
  }

  // Tabe
  private configAbeAseCard(isNew: boolean) {
    //   this.examSubjects = ['Reading', 'Mathematics', 'Languages']
    this.resetConfigCard();
    this.examSubjects = this.examTabe910SubjectsDrop;
    if (isNew === true) {
      this.model.examResults = this.examSubjects.map(function(subject) {
        const x = new ExamResult();
        x.name = subject.name;
        x.totalScore = 999;
        x.subjectId = subject.id;
        return x;
      });
    }
    this.isNrsColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    // this.isScoreColDisplayed = true;
    this._isLevelColDisplayed = true;
    this.isVersionColDisplayed = true;
    this.isGradeEquivalencyColDisplayed = true;
    this.isDetailsDisplayed = true;
  }

  // TABE CLAS-E
  private configTabeClasECard(isNew: boolean) {
    //    this.examSubjects = ['Reading', 'Mathematics', 'Languages']
    this.resetConfigCard();
    this.examSubjects = this.examTabeClaseESubjectsDrop;
    if (isNew === true) {
      this.model.examResults = this.examSubjects.map(function(subject) {
        const x = new ExamResult();
        x.name = subject.name;
        x.totalScore = 999;
        x.subjectId = subject.id;
        return x;
      });
    }
    this._isLevelColDisplayed = true;
    this.isNrsColDisplayed = true;
    this.isSplColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isDetailsDisplayed = true;
  }

  private configBestCard(isNew: boolean) {
    //  this.examSubjects = ['Reading and Writing', 'Speaking and Listening']
    this.resetConfigCard();
    this.examSubjects = this.examBestSubjectsDrop;
    if (isNew === true) {
      this.model.examResults = this.examSubjects.map(function(subject) {
        const x = new ExamResult();
        x.name = subject.name;
        if (x.name === 'Reading/Writing') {
          x.totalScore = 78;
        } else if (x.name === 'Speaking/Listening') {
          x.totalScore = 565;
        }
        x.subjectId = subject.id;
        return x;
      });
    }
    this._isLevelColDisplayed = true;
    this.isNrsColDisplayed = true;
    this.isSplColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isVersionColDisplayed = true;
    this.isDetailsDisplayed = true;
    // this.isGradeEquivalencyColDisplayed = true;
  }

  private configCasasCard(isNew: boolean) {
    //  this.examSubjects = ['Reading and Writing', 'Speaking and Listening']
    this.resetConfigCard();
    this.examSubjects = this.examCasasSubjectsDrop;
    if (isNew === true) {
      this.model.examResults = this.examSubjects.map(function(subject) {
        const x = new ExamResult();
        x.name = subject.name;
        x.subjectId = subject.id;
        return x;
      });
    }
    this._isLevelColDisplayed = true;
    this.isNrsColDisplayed = true;
    this.isScaleScoreColDisplayed = true;
    this.isDetailsDisplayed = true;
    this.isGradeEquivalencyColDisplayed = true;
  }
}
