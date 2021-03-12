// tslint:disable: directive-selector
// tslint:disable: deprecation
// tslint:disable: triple-equals
// tslint:disable: use-life-cycle-interface
import { coerceNumberProperty } from '../../decorators/number-property';
import { AfterViewInit, Directive, ElementRef, EventEmitter, forwardRef, Input, NgZone, Output, Renderer } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
// import 'CKEDITOR';

@Directive({
  selector: '[ckeditable]',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CkEditableDirective),
      multi: true
    }
  ]
})
export class CkEditableDirective implements ControlValueAccessor, AfterViewInit {
  private element;
  private instance: CKEDITOR.editor;
  private _value: string;
  @Input() config: CKEDITOR.config;
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

  get value() {
    return this._value;
  }
  @Input() set value(value) {
    if (value !== this._value) {
      this._value = value;
      this.onChange(value);
      this.onTouched();
    }
  }

  debounceTimeout: number;

  constructor(elementRef: ElementRef, private renderer: Renderer, private zone: NgZone) {
    this.element = elementRef.nativeElement;
    this.renderer.setElementAttribute(this.element, 'contenteditable', 'true');
  }

  ngAfterViewInit() {
    if (typeof CKEDITOR == 'undefined') {
      console.warn('CKEditor not available');
    } else {
      this.config = this.config || {};
      // stop auto-replacing inline components
      CKEDITOR.disableAutoInline = true;
      this.config.allowedContent = true;

      (<any>this.config).fontawesomePath = '/assets/fonts/fontawesome/font-awesome.min.css';

      // CKEditor inline
      this.instance = CKEDITOR.inline(this.element, this.config);

      // Set initial value
      this.instance.setData(this.value);

      // listen for instanceReady event
      this.instance.on('instanceReady', (evt: any) => {
        // send the evt to the EventEmitter
        this.ready.emit(evt);
      });

      this.instance.on('blur', (evt: any) => {
        this.blur.emit(evt);
      });

      // CKEditor focus event
      this.instance.on('focus', (evt: any) => {
        this.focus.emit(evt);
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
        const els = Array.from(document.querySelectorAll('.cke'));
        els.map(x => x.parentNode.removeChild(x));
      });
    }
  }

  updateValue(value: string) {
    this.zone.run(() => {
      this.value = value;

      this.onChange(value);

      this.onTouched();
      this.change.emit(value);
    });
  }

  /**
   * Implements ControlValueAccessor
   */
  writeValue(value) {
    this._value = value;
    if (this.instance) this.instance.setData(value);
  }
  onChange(_) {}
  onTouched() {}
  registerOnChange(fn) {
    this.onChange = fn;
  }
  registerOnTouched(fn) {
    this.onTouched = fn;
  }
}
