import { Component, OnInit, Input, Output, OnDestroy, forwardRef, HostListener, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-delete-reason-button',
  templateUrl: './delete-reason-button.component.html',
  styleUrls: ['./delete-reason-button.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DeleteReasonButtonComponent),
    multi: true
  }]
})

export class DeleteReasonButtonComponent extends BaseComponent implements ControlValueAccessor, OnInit, OnDestroy {

  @Input() type: string;

  /**
   * Sets which object is deleted.
   * 
   * @type {*}
   * @memberof DeleteReasonButtonComponent
   */
  @Input() objectWatched: any;
  @Output() deletedObject = new EventEmitter<any>();


  public dropDown: DropDownField[] = [];
  public isToggled = false;

  private dropSub: Subscription;

  @HostListener('document: click', ['$event.target'])
  onClick(target: HTMLElement) {
    if (!target.classList.contains('allowClick')) {
      this.closeDropDown();
    }
  }

  constructor(private fdService: FieldDataService) { super(); }

  ngOnInit() {
    this.getDeleteReasons(this.type);
  }

  get value(): any {
    return this.innerValue;
  };

  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  showOptions() {
    if (this.objectWatched != null && this.objectWatched.id != null && this.objectWatched.id !== 0) {
      return true;
    } else {
      return false;
    }
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  private closeDropDown() {
    this.isToggled = false;
  }

  public click() {
    this.isToggled = !this.isToggled;
  }

  public select(id: number) {
    this.registerOnChange(id);
    this.objectWatched.deleteReasonId = id;
    this.deletedObject.emit(this.objectWatched);
    this.isToggled = false;
  }

  private initDropDown(data) {
    this.dropDown = data;
  }

  private getDeleteReasons(type: string) {
    this.dropSub = this.fdService.getDeleteReasons(type).subscribe(data => this.initDropDown(data));
  }

  ngOnDestroy() {
    if (this.dropSub != null) {
      this.dropSub.unsubscribe();
    }
  }

}
