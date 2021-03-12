import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-cancel-save-footer',
  templateUrl: './cancel-save-footer.component.html',
  styleUrls: ['./cancel-save-footer.component.scss']
})
export class CancelSaveFooterComponent implements OnInit {
  @Input() isReadOnly = false;
  @Input() hadSaveError = false;
  @Input() isSaving = false;
  @Input() saveLabel = 'Save';
  @Input() isDisabled = false;
  @Input() canSubmit = false;
  @Input() canWithdraw = false;
  @Input() canSaveOnReadOnly = false;
  @Input() canSaveWithWarning = false;
  @Input() pad = true;
  @Input() showSubmitInGreen = false;
  @Output() cancelEvent = new EventEmitter();
  @Output() submitEvent = new EventEmitter();
  @Output() withdrawEvent = new EventEmitter();
  @Output() saveEvent = new EventEmitter();
  @Output() saveWithWarningEvent = new EventEmitter();

  constructor() {}

  ngOnInit() {}

  clickCancel() {
    this.cancelEvent.emit();
  }

  clickSubmit() {
    this.submitEvent.emit();
  }

  clickWithdraw() {
    this.withdrawEvent.emit();
  }

  clickSave() {
    this.saveEvent.emit();
  }
  clickSaveWithWarning() {
    this.saveWithWarningEvent.emit();
  }
}
