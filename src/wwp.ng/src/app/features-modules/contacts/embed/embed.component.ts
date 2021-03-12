import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Component, forwardRef, Input, OnChanges, OnInit, OnDestroy, SimpleChanges, HostBinding, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ContactsService } from '../services/contacts.service';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { Contact } from '../models/contact';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-contacts-embed',
  templateUrl: './embed.component.html',
  styleUrls: ['./embed.component.css'],
  providers: [
    ContactsService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ContactsEmbedComponent),
      multi: true
    }
  ]
})
export class ContactsEmbedComponent extends BaseComponent implements OnChanges, OnInit, OnDestroy, ControlValueAccessor {
  @Output() onInEditView = new EventEmitter<boolean>();
  @Input() isReadOnly: boolean = false;

  @Input() isRequired = false;
  @Input() isInvalid = false;
  @Input() pin: string;
  // tslint:disable-next-line:no-input-rename
  //@Input('value') contactId: number;

  private contactSub: Subscription;
  private contactDeleteSub: Subscription;
  private onTouched: () => {};
  private onChange: (_: number) => {};
  private clonePin: string;
  private _inEditView: boolean = false;
  public pinContactsUrl: string;
  public inConfirmDeleteView: boolean = false;
  public inSelectView: boolean = false;

  get inEditView(): boolean {
    return this._inEditView;
  }

  set inEditView(value: boolean) {
    this._inEditView = value;
    this.onInEditView.emit(this._inEditView);
  }

  @HostBinding('class.dirty') isEmbedDirty: boolean = false;

  @HostBinding('class.required') isEmbedRequired = false;

  @HostBinding('class.error') isEmbedErrored = false;

  public model: Contact;

  constructor(private contactService: ContactsService, private participantService: ParticipantService, private appService: AppService, public route: ActivatedRoute) {
    super();
  }

  ngOnInit() {}

  private initContact(model: Contact) {
    // Ask the service if there are server errors.
    if (model != null) {
      this.model = model;
    }
  }

  get value(): number {
    this.isEmbedDirty = this.isModified;
    return this.innerValue;
  }

  set value(v: number) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChange(v);
      this.onTouched();
      this.isEmbedDirty = this.isModified;
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    // This method is called after any @Input or the value has been changed.
    // In order to update the display, we need both the value (contact ID) and
    // the PIN.  THey might come in on different change callbacks, so we need
    // to get our logic set to only init the contact app when both are set.
    // But, we may never have a contact ID (if it hasn't been set).  In this
    // case, we stil need to set our pin contacts URL.

    // See if we have a contact ID and the PIN
    if (this.innerValue != null && this.innerValue > 0 && this.pin !== this.clonePin) {
      this.clonePin = this.pin;
      if (this.pin != null) {
        this.initContactApp();
      }
    }

    // Anytime we have a pin, update the URL.
    if (this.pin != null) {
      this.pinContactsUrl = `/pin/${this.pin}/contacts`;
    }

    this.isEmbedRequired = this.isRequired;
    this.isEmbedErrored = this.isInvalid;
  }

  writeValue(value: number) {
    if (value !== this.innerValue) {
      this.innerValue = value;
      if (value != null && this.pin != null) {
        this.initContactApp();
      } else {
        this.model = null;
      }
    }
  }

  initContactApp() {
    this.participantService.getCachedParticipant(this.pin).subscribe(p => {
      if (p) {
        if (this.innerValue != null || this.innerValue !== 0) {
          this.contactSub = this.contactService.getContactById(this.innerValue, this.pin).subscribe(contact => {
            if (contact) {
              const filteredCon = this.appService.filterOneTA(contact, p);
              this.initContact(filteredCon);
            }
          });
        }
      }
    });
  }

  registerOnChange(fn: any) {
    this.onChange = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }

  ngOnDestroy() {
    this.destroyContactDeleteSubscription();
    this.destroyContactSubscription();
  }

  edit() {
    this.inEditView = true;
  }

  add() {
    this.inEditView = true;
  }

  onContactSave(contactId: number) {
    this.inEditView = false;

    this.value = contactId;
    this.initContactApp();
  }

  onEditContactCancel() {
    this.inEditView = false;
  }

  clear() {
    this.model = null;
    this.value = null;
  }

  delete() {
    this.inConfirmDeleteView = true;
  }

  select() {
    this.inSelectView = true;
  }

  private destroyContactDeleteSubscription() {
    if (this.contactDeleteSub != null) {
      this.contactDeleteSub.unsubscribe();
    }
  }

  private destroyContactSubscription() {
    if (this.contactSub != null) {
      this.contactSub.unsubscribe();
    }
  }

  onContactSelected(contactId: number) {
    this.value = contactId;
    this.inSelectView = false;
    this.initContactApp();
  }

  onSelectContactCanceled() {
    this.inSelectView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  onConfirmDelete() {
    this.inConfirmDeleteView = false;

    this.destroyContactDeleteSubscription();

    this.contactDeleteSub = this.contactService.deleteContactById(this.innerValue, this.pin).subscribe(resp => {
      // We'll just clear the data.
      this.model = null;
      this.value = null;
    });
  }

  openContact(id: number) {
    const newWindow = window.open(this.pinContactsUrl + '/' + id);
  }
}
