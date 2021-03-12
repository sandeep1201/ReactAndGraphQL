// tslint:disable: component-selector
// tslint:disable: use-life-cycle-interface
// Imports
import { coerceNumberProperty } from '../../decorators/number-property';
import 'CKEDITOR';
import { Component, Input, Output, ViewChild, EventEmitter, NgZone, forwardRef, QueryList, AfterViewInit, ContentChildren, SimpleChanges, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

import { CKButtonDirective } from './ck-button.directive';
import { CKGroupDirective } from './ck-group.directive';

// import 'CKEDITOR';

/**
 * CKEditor component
 * Usage :
 *  <ckeditor [(ngModel)]="data" [config]="{...}" debounce="500"></ckeditor>
 */
@Component({
  selector: 'ckeditor',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CKEditorComponent),
      multi: true
    }
  ],
  template: `
    <textarea #host></textarea>
  `
})
export class CKEditorComponent implements OnChanges, AfterViewInit {
  @Input() config: CKEDITOR.config;
  @Input() readonly: boolean;
  private _debounce = 500;
  @Input() get debounce(): number {
    return this._debounce;
  }
  set debounce(value) {
    this.debounce = coerceNumberProperty(value);
  }
  @Output() change = new EventEmitter();
  @Output() ready = new EventEmitter();
  @Output() blur = new EventEmitter();
  @Output() focus = new EventEmitter();

  @ViewChild('host', { static: true }) host: any;

  @ContentChildren(CKButtonDirective) toolbarButtons: QueryList<CKButtonDirective>;
  @ContentChildren(CKGroupDirective) toolbarGroups: QueryList<CKGroupDirective>;

  _value = '';
  instance: CKEDITOR.editor;
  debounceTimeout: number;
  isFocused = true;

  /**
   * Constructor
   */
  constructor(private zone: NgZone) {}

  get value(): any {
    return this._value;
  }
  @Input() set value(v) {
    if (v !== this._value) {
      this._value = v;
      this.onChange(v);
    }
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.readonly && this.instance) {
      this.instance.setReadOnly(changes.readonly.currentValue);
    }
  }

  /**
   * On component destroy
   */
  ngOnDestroy() {
    if (this.instance) {
      setTimeout(() => {
        this.instance.removeAllListeners();
        CKEDITOR.instances[this.instance.name].destroy();
        this.instance.destroy();
        this.instance = null;
      });
    }
  }

  /**
   * On component view init
   */
  ngAfterViewInit() {
    this.ckeditorInit(this.config || {});
  }

  /**
   * On component view checked
   */
  ngAfterViewChecked() {
    this.ckeditorInit(this.config || {});
  }

  /**
   * Value update process
   */
  updateValue(value: string) {
    this.zone.run(() => {
      this.value = value;

      this.onChange(value);

      this.onTouched();
      this.change.emit(value);
    });
  }

  /**
   * CKEditor init
   */
  ckeditorInit(config: CKEDITOR.config) {
    if (typeof CKEDITOR === 'undefined') {
      throw new Error('CKEditor 4.x is missing');
    } else {
      // Check textarea exists
      if (this.instance || !document.contains(this.host.nativeElement)) {
        return;
      }

      config.allowedContent = true;

      if (this.readonly) {
        config.readOnly = this.readonly;
      }

      (<any>config).fontawesomePath = '/assets/fonts/fontawesome/font-awesome.min.css';

      // CKEditor replace textarea
      this.instance = CKEDITOR.replace(this.host.nativeElement, config);

      // Set initial value
      this.instance.setData(this.value);

      // listen for instanceReady event
      this.instance.on('instanceReady', (evt: any) => {
        // send the evt to the EventEmitter
        this.ready.emit(evt);
      });

      // CKEditor change event
      this.instance.on('change', () => {
        this.onTouched();
        const value = this.instance.getData();

        // Debounce update
        if (this.debounce) {
          if (this.debounceTimeout) window.clearTimeout(this.debounceTimeout);
          this.debounceTimeout = window.setTimeout(() => {
            this.updateValue(value);
            this.debounceTimeout = null;
          }, this.debounce);

          // Live update
        } else {
          this.updateValue(value);
        }
      });

      // CKEditor blur event
      this.instance.on('blur', (evt: any) => {
        this.zone.run(() => {
          this.isFocused = false;
          this.blur.emit(evt);
        });
      });

      // CKEditor focus event
      this.instance.on('focus', (evt: any) => {
        this.zone.run(() => {
          this.isFocused = true;
          this.focus.emit(evt);
        });
      });

      // Add Toolbar Groups to Editor. This will also add Buttons within groups.
      this.toolbarGroups.forEach(group => {
        group.initialize(this);
      });
      // Add Toolbar Buttons to Editor.
      this.toolbarButtons.forEach(button => {
        button.initialize(this);
      });
    }
  }

  /**
   * Implements ControlValueAccessor
   */
  writeValue(value: any) {
    this._value = value;
    if (this.instance) this.instance.setData(value);
  }
  onChange(_: any) {}
  onTouched() {}
  registerOnChange(fn: any) {
    this.onChange = fn;
  }
  registerOnTouched(fn: any) {
    this.onTouched = fn;
  }
}
