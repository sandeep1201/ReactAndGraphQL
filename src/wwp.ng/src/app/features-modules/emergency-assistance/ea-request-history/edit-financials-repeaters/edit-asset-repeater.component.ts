import { Utilities } from './../../../../shared/utilities';
import { EAAssets } from './../../models/ea-request-financials.model';
import { Component, forwardRef, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-assets-repeater',
  templateUrl: './edit-asset-repeater.component.html',
  styleUrls: ['./edit-financials-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AssetsRepeaterComponent),
      multi: true
    }
  ]
})
export class AssetsRepeaterComponent extends BaseRepeaterComponent<EAAssets> implements ControlValueAccessor, OnChanges {
  @Input() verificationDrop: DropDownField[];
  @Input() assetOwnerDrop: DropDownField[];
  @Input() initTotal: EAAssets[];
  @Input() isReadOnly = false;
  @Output() financialAssets = new EventEmitter<number>();
  totalValue: string;

  constructor() {
    super(EAAssets.create);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes && changes.initTotal && changes.initTotal !== undefined) {
      this.calculateTotal(this.initTotal);
    }
  }

  calculateTotal(assets: EAAssets[]) {
    let total = 0;
    if (assets) {
      assets.forEach(x => {
        if (x.currentValue && x.currentValue !== '') {
          total += Utilities.currencyToNumber(x.currentValue);
        }
      });
    }
    this.totalValue = total.toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.financialAssets.emit(total);
  }
}
