import { coerceBooleanProperty } from '../../../decorators/boolean-property';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-content-edit-bar',
  templateUrl: './content-edit-bar.component.html',
  styleUrls: ['./content-edit-bar.component.css']
})
export class ContentEditBarComponent implements OnInit {
  private _editMode = false;
  @Input() get editMode() {
    return this._editMode;
  }
  set editMode(val) {
    this._editMode = coerceBooleanProperty(val);
  }

  private _isDirty = false;
  @Input() get isDirty() {
    return this._isDirty;
  }
  set isDirty(val) {
    this._isDirty = coerceBooleanProperty(val);
  }

  @Output() edit = new EventEmitter();

  @Output() cancel = new EventEmitter();

  @Output() save = new EventEmitter();

  private _isSaving = false;
  @Input() get isSaving() {
    return this._isSaving;
  }
  set isSaving(val) {
    this._isSaving = coerceBooleanProperty(val);
  }

  constructor() {}

  ngOnInit() {}

  editPage() {
    this.editMode = true;
    this.edit.emit();
  }

  cancelEdit() {
    this.cancel.emit();
    this.exit();
  }

  onSave() {
    this.save.emit();
    this.exit();
  }

  private exit() {
    this.editMode = false;
    this.isSaving = false;
    this.isDirty = false;
  }

  pageSettings() {
    // TODO:
  }
}
