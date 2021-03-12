// tslint:disable: no-use-before-declare
import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { KnownLanguage } from '../../../../shared/models/languages-section';

const noop = () => {};

export const LANG_REPEATER_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => LanguageRepeaterComponent),
  multi: true
};

@Component({
  selector: 'app-language-repeater',
  templateUrl: './language-repeater.component.html',
  styleUrls: ['./language-repeater.component.css'],
  providers: [LANG_REPEATER_CONTROL_VALUE_ACCESSOR]
})
export class LanguageRepeaterComponent extends BaseRepeaterComponent<KnownLanguage> implements ControlValueAccessor {
  languageDropList: DropDownField[];

  constructor() {
    super(KnownLanguage.create);
  }

  @Input()
  set dropDownList(dropDownList: DropDownField[]) {
    dropDownList = dropDownList.filter(function(obj) {
      return obj.name !== 'English';
    });

    this.languageDropList = dropDownList;
  }

  isRequired(kl: KnownLanguage) {
    return kl.isRequired();
  }

  /**
   *  We use this to remove the disable state of a language from
   *  the language dropdown when it is deleted.
   *
   * @param {DropDownField} lang
   * @returns
   * @memberof LanguageRepeaterComponent
   */
  removeDisableOnLanguages(lang) {
    if (lang == null || lang.languageId < 1 || this.languageDropList == null) {
      return;
    }

    for (const dd of this.languageDropList) {
      if (+dd.id === +lang.languageId) {
        dd.isSelected = false;
        // We break because there only be 1.
        break;
      }
    }
  }
}
