import { EAHouseholdIncomes } from './../../models/ea-request-financials.model';
import { Component, forwardRef, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-income-repeater',
  templateUrl: './edit-income-repeater.component.html',
  styleUrls: ['./edit-financials-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => IncomeRepeaterComponent),
      multi: true
    }
  ]
})
export class IncomeRepeaterComponent extends BaseRepeaterComponent<EAHouseholdIncomes> implements ControlValueAccessor, OnChanges {
  @Input() verificationDrop: DropDownField[];
  @Input() groupMembersDrop: DropDownField[];
  @Input() initIncome: EAHouseholdIncomes[];
  @Input() isReadOnly = false;
  totalIncome: string;

  constructor() {
    super(EAHouseholdIncomes.create);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes && changes.initIncome && changes.initIncome !== undefined) {
      this.calculateTotal(this.initIncome);
    }
  }

  calculateTotal(incomes: EAHouseholdIncomes[]) {
    let total = 0;
    if (incomes) {
      incomes.forEach(x => {
        if (x.monthlyIncome && x.monthlyIncome !== '') {
          total += Utilities.currencyToNumber(x.monthlyIncome);
        }
      });
    }
    this.totalIncome = total.toLocaleString('en-US', { minimumFractionDigits: 2 });
  }
}
