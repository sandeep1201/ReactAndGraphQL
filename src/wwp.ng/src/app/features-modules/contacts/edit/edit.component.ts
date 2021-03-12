import { Component, EventEmitter, Input, OnInit, OnDestroy, Output } from '@angular/core';
import { Subscription } from 'rxjs';
import { Contact } from '../models/contact';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { ContactsService } from '../services/contacts.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-edit-contact',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class ContactsEditComponent implements OnInit, OnDestroy {
  @Input() pin: string;
  @Input() contactId: number;
  @Output() save = new EventEmitter<number>();
  @Output() cancel = new EventEmitter();

  public isLoaded = false;
  public isModified = false;
  public model: Contact;
  public cachedModel: Contact;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.

  private cloneModelString: string;
  public contactTypesDrop: DropDownField[];
  private ctSub: Subscription;
  public hadSaveError = false;
  public isModelValid = true;
  public isSaving = false;
  public mode: string;
  private modelSub: Subscription;
  public otherTitleTypeId: number;
  private validationManager: ValidationManager = new ValidationManager(this.appService);

  constructor(private appService: AppService, private fdService: FieldDataService, private contactsService: ContactsService, private participantService: ParticipantService) {}

  ngOnInit() {
    this.ctSub = this.fdService.getContactTypes().subscribe(data => this.initContactTypes(data));

    // If the contact ID is null, that means we should be in Add mode.
    if (this.contactId != null && this.contactId > 0) {
      this.mode = 'Edit';
      this.modelSub = this.contactsService.getContactById(this.contactId, this.pin).subscribe(data => {
        if (data) {
          this.participantService.getCachedParticipant(this.pin).subscribe(p => {
            if (p) {
              const filteredData = this.appService.filterOneTA(data, p);
              if (filteredData) {
                this.initModel(filteredData);
                this.validate();
                this.isLoaded = true;
              } else {
                this.mode = 'Add';
                const contact = new Contact();
                // Force the ID to 0 so the API recognizes the contact is new.
                contact.id = 0;

                this.initModel(contact);

                // We start out in an invalid state, so indicate that.
                this.isModelValid = false;
                this.isLoaded = true;
              }
            }
          });
        } else {
          this.mode = 'Add';
          const contact = new Contact();
          // Force the ID to 0 so the API recognizes the contact is new.
          contact.id = 0;

          this.initModel(contact);

          // We start out in an invalid state, so indicate that.
          this.isModelValid = false;
          this.isLoaded = true;
        }
      });

      // Under normal circumstances, this should not be necessary, but we'll play it safe
      // and validate the object we got from the server API.
    } else {
      this.mode = 'Add';
      const contact = new Contact();
      // Force the ID to 0 so the API recognizes the contact is new.
      contact.id = 0;

      this.initModel(contact);

      // We start out in an invalid state, so indicate that.
      this.isModelValid = false;
      this.isLoaded = true;
    }
  }

  ngOnDestroy() {
    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
    }
  }

  private initContactTypes(data: DropDownField[]) {
    this.participantService.getCachedParticipant(this.pin).subscribe(p => {
      if (p) {
        const filteredDrop = this.appService.filterContactTypeDrop(data, p);
        this.contactTypesDrop = filteredDrop;
        this.otherTitleTypeId = Utilities.idByFieldDataName('Other', filteredDrop);
      }
    });
  }

  private initModel(model: Contact) {
    this.model = model;
    this.cachedModel = new Contact();
    Contact.clone(this.model, this.cachedModel);

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);
    this.validate();
  }

  validate() {
    if (this.model) {
      const result = this.model.validate(this.validationManager, this.otherTitleTypeId);
      this.isModelValid = result.isValid;
      this.modelErrors = result.errors;
    }
  }

  saveAndExit() {
    this.isSaving = true;
    this.hadSaveError = false;

    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
      this.modelSub = null;
    }

    // Call the service to save the data.
    this.modelSub = this.contactsService.saveContact(this.pin, this.model).subscribe(
      data => {
        // this.initModel(data);
        this.save.emit(data.id);
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
        throw error;
      }
    );
  }

  exit() {
    this.cancel.emit();
  }
}
