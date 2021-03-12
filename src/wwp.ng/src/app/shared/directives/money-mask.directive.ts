// tslint:disable: directive-selector
// tslint:disable: no-output-rename
// tslint:disable: no-input-rename
// tslint:disable: no-use-before-declare
// tslint:disable: prefer-const
import { HostListener, Input, Output, Optional, Directive, OnChanges, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';
import { NgModel } from '@angular/forms';

// https://github.com/castrolol/ng2-mask-money
// TODO: Clean up after passing testing.

@Directive({
  selector: 'input[mask-money]'
})

export class MoneyMaskDirective implements AfterViewInit, OnChanges {
  @Output('moneyModel')
  moneyModelChange: EventEmitter<number> = new EventEmitter<number>(true);
  @Input('moneyModel')
  moneyModel: number;

  @Input('money-mask-options')
  inputOptions: any = {};

  inputEventHandler: MoneyInputEventHandler;
  elementRef: HTMLInputElement;
  options = {
    allowNegative: false,
    precision: 2,
    prefix: '',
    suffix: '',
    thousands: ',',
    decimal: '.',
    allowZero: true,
    affixesStay: true
  };

  constructor(@Optional() private ngModel: NgModel, public el: ElementRef) {
  }

  ngOnChanges(changes) {
    if ("moneyModel" in changes) {

      const value = (+changes.moneyModel.currentValue || 0).toString();

      setTimeout(() => this.inputEventHandler.setValue(value), 100);
    }
  }

  ngAfterViewInit() {

    this.elementRef = this.el.nativeElement as HTMLInputElement;
    this.elementRef.style.textAlign = 'right';

    const options = Object.assign({}, this.options, this.inputOptions);

    this.inputEventHandler = new MoneyInputEventHandler(this.elementRef, options, v => {
      if (this.ngModel) {
        let event = new CustomEvent("Event");
        event.initEvent('input', true, false);
        this.elementRef.dispatchEvent(event);
      }
      this.moneyModelChange.emit(this.inputEventHandler.inputService.value);
    });
  }


  @HostListener('keypress', ['$event'])
  handleKeypress(e) {
    this.inputEventHandler.handleKeypress(e);
  }

  @HostListener('keydown', ['$event'])
  handleKeydown(e) {
    this.inputEventHandler.handleKeydown(e);
  }

  @HostListener('blur', ['$event'])
  handleBlur(e) {
    this.inputEventHandler.handleBlur(e);
  }

  @HostListener('focus', ['$event'])
  handleFocus(e) {
    this.inputEventHandler.handleFocus(e);
  }

  @HostListener('click', ['$event'])
  handleClick(e) {
    this.inputEventHandler.handleClick(e);
  }

  @HostListener('paste', ['$event'])
  handlePaste() {
    this.inputEventHandler.handlePaste();
  }

}

class MoneyInputEventHandler {

  inputService: MoneyInputService;

  constructor(private input: HTMLInputElement, options: any, onchange) {
    this.inputService = new MoneyInputService(input, options, onchange);
  }

  setValue(value) {

    this.inputService.value = value;
  }

  handleKeypress(e) {
    const { inputService } = this;
    let key = e.which || e.charCode || e.keyCode;

    if (key === undefined) {
      return false;
    }

    if (key < 48 || key > 57) {
      // -(minus) key
      if (key === 45) {
        //   inputService.changeSign();
        e.preventDefault();
        return true;
        // +(plus) key
      } else if (key === 43) {
        //   inputService.removeSign();
        e.preventDefault();
        return true;
        // enter key or tab key
      } else if (key === 13 || key === 9) {
        return true;
      } else { // any other key with keycode less than 48 and greater than 57
        e.preventDefault();
        return true;
      }
    } else if (!inputService.canInputMoreNumbers) {
      return false;
    } else {
      e.preventDefault();
      inputService.addNumber(key);
      return false;
    }

  }

  handleKeydown(e) {

    const { inputService } = this;


    let key = e.which || e.charCode || e.keyCode;

    if (key === undefined) {
      return false;
    }
    /*
      let selection = inputService.inputSelection;
      let startPos = selection.start;
      let endPos = selection.end;
    */

    //espaço ou delete
    if (key === 8 || key === 46 || key === 63272) {
      e.preventDefault();

      inputService.processSpacebar(key);

      return false;
    } else if (key === 9) { // tab
      return true;
    } else { // outros
      return true;
    }
  }

  handleBlur(e) {
    this.inputService.reformatField();
  }

  handleClick(e) {
  }

  handleFocus(e) {
  }

  handleCutPastEvent(e) {
    this.inputService.waitAndFormat();
  }

  handlePaste() {
    this.inputService.reformatField();
  }

}

class MoneyInputService {

  lastValidValue: string = '';
  maskProvider: MoneyMaskProvider;
  inputManager: InputManager;

  triggerChange = (() => {
    return;
  });

  elementRef: any;
  options = {
    allowNegative: false,
    precision: 2,
    prefix: '',
    suffix: '',
    thousands: '.',
    decimal: ',',
    allowZero: true,
    affixesStay: true
  };

  onchange = (val) => {
    return val;
  }

  get rawValue() {
    return this.elementRef && this.elementRef.value;
  }

  set rawValue(value) {
    if (this.elementRef) {
      this.elementRef.value = value;
      if (this.onchange) {
        setTimeout(() => this.onchange(this.rawValue), 1);
      }
    }
  }

  get value() {
    return this.maskProvider.clear(this.rawValue);
  }

  set value(val) {

    let rawValue = this.maskProvider.fromNumber(val);
    this.rawValue = rawValue;
  }


  get canInputMoreNumbers() {
    return this.inputManager.canInputMoreNumbers;
  }

  get inputSelection() {
    return this.inputManager.inputSelection;
  }

  get emptyValue() {
    return this.maskProvider.setSymbol(this.maskProvider.defaultMask);
  }

  constructor(input, options, onchange) {
    this.elementRef = input;
    this.options = Object.assign({}, this.options, options);
    this.onchange = onchange;

    this.maskProvider = new MoneyMaskProvider(this.options);
    this.inputManager = new InputManager(input, this.options);
  }

  init() {
    this.elementRef.style.textAlign = 'right';
    this.updateFieldValue(0);
  }

  onChange(handler) {
    this.triggerChange = handler || (() => {
      return;
    });
  }

  updateFieldValue(startPos) {
    let value = this.rawValue || '';
    let length = value.length;
    value = this.maskProvider.applyMask(value);
    this.inputManager.updateValueAndCursor(value, length, startPos);
  }

  changeSign() {
    this.rawValue = this.maskProvider.changeSign(this.rawValue);
  }

  removeSign() {
    this.rawValue = this.rawValue.replace('-', '');
  }

  processSpacebar(key) {
    let selection = this.inputSelection;
    let startPos = selection.start;
    let endPos = selection.end;
    let value = this.rawValue;

    // sem seleção
    if (startPos === endPos) {
      // espaço
      if (key === 8) {
        let lastNumber = value.split('').reverse().join('').search(/\d/);
        startPos = value.length - lastNumber - 1;
        endPos = startPos + 1;
      } else {
        endPos += 1;
      }
    }

    this.rawValue = value.substring(0, startPos) + value.substring(endPos, value.length);
    this.updateFieldValue(startPos);
  }

  reformatField() {
    let value = this.rawValue;
    let empty = this.emptyValue;

    if (value === '' || value === empty) {
      if (!this.options.allowZero) {
        this.rawValue = '';
      } else if (!this.options.affixesStay) {
        this.rawValue = this.maskProvider.defaultMask;
      } else {
        this.rawValue = empty;
      }
    } else {
      if (!this.options.affixesStay) {
        this.rawValue = this.rawValue.replace(this.options.prefix, '').replace(this.options.suffix, '');
      }
    }

    if (this.rawValue !== this.lastValidValue) {
      this.triggerChange();
    }
  }

  resetSelection() {
    let { elementRef } = this;

    if (elementRef.setSelectionRange) {
      length = this.rawValue.length;
      elementRef.setSelectionRange(length, length);
    } else {
      let value = this.rawValue;
      setTimeout(() => {
        this.rawValue = value;
      }, 1);
    }
  }

  saveFocusValue() {
    this.lastValidValue = this.rawValue;

    this.rawValue = this.maskProvider.apply(this.rawValue);
    let input = this.elementRef;

    if (input.createTextRange) {
      let textRange = input.createTextRange();
      textRange.collapse(false); // set the cursor at the end of the input
      textRange.select();
    }
  }

  waitAndFormat() {
    setTimeout(() => {
      this.maskProvider.apply(this.rawValue);
    }, 1);
  }

  addNumber(key) {
    let keyPressedChar = String.fromCharCode(key);
    let selection = this.inputSelection;
    let startPos = selection.start;
    let endPos = selection.end;
    let value = this.rawValue;
    this.rawValue = value.substring(0, startPos) + keyPressedChar + value.substring(endPos, value.length);
    this.updateFieldValue(startPos + 1);
  }

}

class InputManager {

  constructor(private input: any, private options: any) {
  }

  get rawValue() {
    return this.input && this.input.value;
  }

  set rawValue(value) {
    if (this.input) {
      this.input.value = value;
    }
  }

  get canInputMoreNumbers(): boolean {

    let input = this.input;
    let maxlength = input.maxLength;

    let haventReachedMaxLength = !(this.rawValue.length >= maxlength && maxlength >= 0);
    let selection = this.inputSelection;
    let start = selection.start;
    let end = selection.end;
    let haveNumberSelected = (selection.start !== selection.end && input.value.substring(start, end).match(/\d/)) ? true : false;
    let startWithZero = (input.value.substring(0, 1) === '0');

    return haventReachedMaxLength || haveNumberSelected || startWithZero;
  }

  get inputSelection() {
    let el = this.input;
    let start = 0;
    let end = 0;


    if (typeof el.selectionStart === 'number' && typeof el.selectionEnd === 'number') {
      start = el.selectionStart;
      end = el.selectionEnd;
    } else {
      let range = (<any>document).selection.createRange(); //

      if (range && range.parentElement() === el) {
        let len = el.value.length;
        let normalizedValue = el.value.replace(/\r\n/g, '\n');

        // Create a working TextRange that lives only in the input
        let textInputRange = el.createTextRange();
        textInputRange.moveToBookmark(range.getBookmark());

        // Check if the start and end of the selection are at the very end
        // of the input, since moveStart/moveEnd doesn't return what we want
        // in those cases
        let endRange = el.createTextRange();
        endRange.collapse(false);

        if (textInputRange.compareEndPoints('StartToEnd', endRange) > -1) {
          start = end = len;
        } else {
          start = -textInputRange.moveStart('character', -len);
          start += normalizedValue.slice(0, start).split('\n').length - 1;

          if (textInputRange.compareEndPoints('EndToEnd', endRange) > -1) {
            end = len;
          } else {
            end = -textInputRange.moveEnd('character', -len);
            end += normalizedValue.slice(0, end).split('\n').length - 1;
          }
        }
      }
    }

    return {
      start: start,
      end: end
    };
  }

  updateValueAndCursor(value, oldLen, startPos) {
    let length = oldLen;
    this.rawValue = value;
    let newLength = value.length;
    startPos = startPos - (length - newLength);
    this.setCursorAt(startPos);
  }

  setCursorAt(pos) {
    let elem = this.input;
    if (elem.setSelectionRange) {
      elem.focus();
      elem.setSelectionRange(pos, pos);
    } else if (elem.createTextRange) {
      let range = elem.createTextRange();
      range.collapse(true);
      range.moveEnd('character', pos);
      range.moveStart('character', pos);
      range.select();
    }
  }
}

class MoneyMaskProvider {

  options = {
    allowNegative: false,
    precision: 2,
    thousands: ',',
    decimal: '.',
    prefix: '',
    suffix: ''
  };

  constructor(options) {
    this.options = Object.assign({}, this.options, options);
  }

  get defaultMask() {
    return '';
  }

  fromNumber(value) {
    const { allowNegative, precision, thousands, decimal, prefix, suffix } = this.options;

    value = (value || 0)
    if (!allowNegative) value = Math.abs(value);

    let text = (+value || 0).toFixed(precision);

    let [integer, dec] = text.split(".");


    let integerPart = integer.replace(/\B(?=(\d{3})+(?!\d))/g, thousands);
    let decimalPart = precision <= 0 ? "" : `${decimal}${dec}`;

    return `${prefix}${integerPart}${decimalPart}${suffix}`;

  }

  clear(textValue) {
    let value = (textValue);
    let isNegative = value.indexOf('-') !== -1;
    let decimalPart = '';


    value
      .split(/\D/)
      .reverse()
      .forEach(function (element) {
        if (element) {
          decimalPart = element;
          return false;
        }
      });

    value = value.replace(/\D/g, '');
    value = value.replace(new RegExp(decimalPart + '$'), '.' + decimalPart);
    if (isNegative) {
      value = '-' + value;
    }
    return parseFloat(value);

  }

  applyMask(value) {
    let { allowNegative, precision, thousands, decimal } = this.options;

    let negative = (value.indexOf('-') > -1 && allowNegative) ? '-' : '',
      onlyNumbers = value.replace(/[^0-9]/g, ''),
      integerPart = onlyNumbers.slice(0, onlyNumbers.length - precision),
      newValue,
      decimalPart,
      leadingZeros;

    if (integerPart !== '') {
      integerPart = integerPart.replace(/^0*/g, '');
      if (integerPart === '') {
        integerPart = '0';
      }
    }

    integerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, thousands);
    newValue = negative + integerPart;

    if (precision > 0) {
      decimalPart = onlyNumbers.slice(onlyNumbers.length - precision);
      if (decimalPart.length === 1 && value !== '.0') {
        leadingZeros = new Array((precision + 1) - decimalPart.length).join('0');
      } else {
        leadingZeros = new Array((precision + 1) - decimalPart.length).join(' ');
      }
      newValue += decimal + leadingZeros + decimalPart;
    }
    return this.setSymbol(newValue);
  }

  apply(value) {

    if (this.options.precision > 0 && value.indexOf(this.options.decimal) < 0) {
      value += this.options.decimal + new Array(this.options.precision + 1).join('0');
    }
    return this.applyMask(value);
  }

  setSymbol(value) {

    let { prefix, suffix } = this.options;

    let operator = '';
    if (value.indexOf('-') > -1) {
      value = value.replace('-', '');
      operator = '-';
    }
    return operator + prefix + value + suffix;
  }

  changeSign(value) {
    let inputValue = value;
    if (this.options.allowNegative) {
      if (inputValue !== '' && inputValue.charAt(0) === '-') {
        return inputValue.replace('-', '');
      } else {
        return '-' + inputValue;
      }
    } else {
      return inputValue;
    }
  }


}
