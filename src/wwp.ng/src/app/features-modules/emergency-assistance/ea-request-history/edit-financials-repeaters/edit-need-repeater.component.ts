import { EAFinancialNeeds } from './../../models/ea-request-agency-summary.model';
import { Utilities } from './../../../../shared/utilities';
import { Component, forwardRef, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-financial-need-repeater',
  templateUrl: './edit-need-repeater.component.html',
  styleUrls: ['./edit-financials-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => FinancialNeedRepeaterComponent),
      multi: true
    }
  ]
})
export class FinancialNeedRepeaterComponent extends BaseRepeaterComponent<EAFinancialNeeds> implements ControlValueAccessor, OnChanges {
  @Input() financialNeedDrop: DropDownField[];
  @Input() initTotal: EAFinancialNeeds[];
  @Input() isReadOnly = false;
  @Input() maxPaymentAmount: string;
  @Input() isRequired = true;
  totalValue: string;
  maxAmount: string;
  lesserAmount: string;

  constructor() {
    super(EAFinancialNeeds.create);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes && changes.initTotal && changes.initTotal !== undefined) {
      this.calculateTotal(this.initTotal);
    }
  }

  calculateTotal(needs: EAFinancialNeeds[]) {
    let total = 0;
    if (needs) {
      needs.forEach(x => {
        if (x.amount && x.amount !== '') {
          total += Utilities.currencyToNumber(x.amount);
        }
      });
    }
    this.totalValue = total.toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.maxAmount = (+this.maxPaymentAmount).toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.lesserAmount = total < +this.maxPaymentAmount ? this.totalValue : this.maxAmount;
  }
}
