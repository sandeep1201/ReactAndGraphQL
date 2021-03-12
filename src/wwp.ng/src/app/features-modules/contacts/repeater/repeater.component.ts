import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { GenericItem } from 'src/app/shared/models/primitives';

@Component({
  selector: 'app-contacts-repeater',
  templateUrl: './repeater.component.html',
  styleUrls: ['./repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ContactsRepeaterComponent),
      multi: true
    }
  ]
})
export class ContactsRepeaterComponent extends BaseParticipantComponent implements OnInit, ControlValueAccessor {
  @Input() isReadOnly = false;
  @Input() maxContacts = 999999;
  @Input() pin;
  @Input() inHistory = false;

  public cacheContactIds: number[] = [];

  constructor(route: ActivatedRoute, router: Router, partService: ParticipantService) {
    super(route, router, partService);
    this.innerValue = [];
  }

  ngOnInit() {}

  initContact() {
    const g = new GenericItem();
    g.value = null;
    this.model.push(g);
  }

  // Get accessor
  get model(): GenericItem[] {
    return this.innerValue;
  }

  // Set accessor including call the onchange callback
  set model(v: GenericItem[]) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChange();
    }
  }

  // Used in html.
  public readOnly() {
    return this.isReadOnly;
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: any) {
    if (!this.inHistory) {
      if (value != null && JSON.stringify(value) !== JSON.stringify(this.cacheContactIds)) {
        for (const x of value) {
          const g = new GenericItem();
          g.value = x;
          this.model.push(g);
        }
        this.cacheContactIds = value;
      }

      if (value != null && value.length === 0) {
        this.model = null;
        this.model = [];
        this.initContact();
      }
    }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  onChange() {
    const ga = [];
    if (this.model != null) {
      for (const x of this.model) {
        if (x.value != null) {
          const gen = x.value;
          ga.push(gen);
        }
      }
    }
    this.onChangeCallback(ga);
  }

  add() {
    if (this.allowAddition()) {
      const c = new GenericItem();
      c.value = null;
      this.model.push(c);
      this.onChange();
    }
  }

  /**
   * Adds disable css to add button and wont allow additon of new contacts.
   *
   * @private
   * @returns {boolean}
   *
   * @memberOf ContactsRepeaterComponent
   */
  private allowAddition(): boolean {
    if (this.model != null && this.model.length < this.maxContacts) {
      return true;
    } else {
      return false;
    }
  }
}
