import { TimeLimitsService } from '../../../shared/services/timelimits.service';
import { Extension, ExtensionSequence } from '../../../shared/models/time-limits';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subscription, Observable, empty } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';
export type deleteWarningType = 'end' | 'delete';

@Component({
  selector: 'app-delete-warning',
  templateUrl: './delete-warning.component.html',
  styleUrls: ['./delete-warning.component.css']
})
export class DeleteWarningComponent implements OnInit {
  @Input() model: Extension;
  @Input() pin: number;
  @Input() warningType = 'end';

  @Output() close = new EventEmitter();
  @Output() save = new EventEmitter<ExtensionSequence>();

  public isSaving = false;
  public hadSaveError = false;
  public errorMsg: string;
  private modelSub: Subscription;
  constructor(private timeLimitsService: TimeLimitsService) {}

  ngOnInit() {}

  endExtension() {
    let ext = this.model.clone();
    ext.dateRange.end = Utilities.currentDate.endOf('month');
    this.saveAndExit(ext);
  }

  deleteExtension() {
    let ext = this.model.clone();
    ext.isDeleted = true;
    this.saveAndExit(ext);
  }
  saveAndExit(extension: Extension) {
    this.isSaving = true;
    this.hadSaveError = false;
    this.errorMsg = null;

    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
      this.modelSub = null;
    }

    // Call the service to save the data.
    this.modelSub = this.timeLimitsService
      .saveExtension(`${this.pin}`, extension)
      .pipe(take(1))
      .pipe(catchError(this.handleSaveError))
      .subscribe(data => {
        // this.initModel(data);
        this.hadSaveError = false;
        this.isSaving = false;
        this.save.emit(data);
        // TODO:make this work - this.save.emit(data);
        this.exit();
      });
  }

  exit() {
    this.isSaving = false;
    this.hadSaveError = false;
    this.model = null;
    this.errorMsg = null;
    this.close.emit();
  }

  private handleSaveError(err, obs): Observable<ExtensionSequence> {
    this.isSaving = false;
    this.hadSaveError = true;
    this.errorMsg = err && err.message ? err.message : 'Uknown error encoutered.';
    return empty();
  }
}
