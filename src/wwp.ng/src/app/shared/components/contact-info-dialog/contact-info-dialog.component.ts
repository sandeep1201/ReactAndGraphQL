import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';
import { ContactInfoService } from '../../services/contactInfo.service';
import { ContactInfo } from '../../models/contact-info.model';
import { ValidationManager } from '../../models/validation';
import { ModelErrors } from '../../interfaces/model-errors';

@Component({
  selector: 'app-contact-info-dialog',
  templateUrl: './contact-info-dialog.component.html',
  styleUrls: ['./contact-info-dialog.component.css']
})
@Modal()
export class ContactInfoDialogComponent implements OnInit, DestroyableComponent {
  errorMessage: string;
  public originalModel = new ContactInfo();
  public model: ContactInfo;
  public modelErrors: ModelErrors = {};
  public isSaving = false;
  public isSectionModified = false;
  public workerId = '';
  public contactInfo = new ContactInfo();
  public hasTriedSave = false;
  public isSectionValid = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);

  @Output() isInEditMode = new EventEmitter<any>();

  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor(private appService: AppService, private contactInfoService: ContactInfoService) {}

  get isDialogPresent() {
    return this.appService.isDialogPresent;
  }
  ngOnInit() {
    this.contactInfoService.getWorkerContactInfo().subscribe(data => {
      this.workerId = data.workerId;
      if (!(data.id == null || data.id === 0)) {
        this.model = data;
      } else {
        this.model = ContactInfo.create();
      }

      ContactInfo.clone(this.model, this.originalModel);
    });
  }

  exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.isInEditMode.emit(false);
      this.closeDialog();
      this.destroy();
    }
  }

  submit() {
    this.errorMessage = null;
    this.hasTriedSave = true;
    this.validate();

    if (this.isSectionValid) {
      this.isSaving = true;
      this.model.workerId = this.workerId;
      this.model.email = this.model.email.trim();

      if (JSON.stringify(this.model) !== JSON.stringify(this.originalModel)) {
        this.contactInfoService.saveWorkerContactInfo(this.model).subscribe(
          res => {
            this.isSaving = false;
            this.isInEditMode.emit(false);
            this.closeDialog();
            this.destroy();
          },
          error => {
            this.isInEditMode.emit(true);
            this.isSaving = false;
          }
        );
      } else {
        this.isSaving = false;
        this.isInEditMode.emit(false);
        this.closeDialog();
        this.destroy();
      }
    } else {
      this.isSaving = false;
    }
  }

  validate() {
    this.isSectionModified = true;
    this.validationManager.resetErrors();
    const result = this.model.validate(this.validationManager);
    this.isSectionValid = result.isValid;
    if (this.isSectionValid) this.hasTriedSave = false;
    this.modelErrors = result.errors;
  }

  /**
   * Invoked when selects Ignore changes from the save confirmation
   */
  exitFinalistLocationEditIgnoreChanges(e) {
    this.isInEditMode.emit(false);
    this.closeDialog();
    this.destroy();
  }
}
