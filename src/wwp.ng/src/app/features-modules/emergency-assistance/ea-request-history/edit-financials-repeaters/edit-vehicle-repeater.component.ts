import { Utilities } from './../../../../shared/utilities';
import { EAVehicles } from './../../models/ea-request-financials.model';
import { Component, forwardRef, Input, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-vehicles-repeater',
  templateUrl: './edit-vehicle-repeater.component.html',
  styleUrls: ['./edit-financials-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => VehiclesRepeaterComponent),
      multi: true
    }
  ]
})
export class VehiclesRepeaterComponent extends BaseRepeaterComponent<EAVehicles> implements ControlValueAccessor, OnChanges {
  @Input() verificationDrop: DropDownField[];
  @Input() vehicleValueVerificationDrop: DropDownField[];
  @Input() vehicleOwnerDrop: DropDownField[];
  @Input() initTotal: EAVehicles[];
  @Input() isReadOnly = false;
  @Output() vehiclesValue = new EventEmitter<number>();
  totalVehicleEquity: string;
  totalValuetowardsAssets: string;
  private readonly disregardAmount = 10000;

  constructor() {
    super(EAVehicles.create);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes && changes.initTotal) {
      this.calculateTotal(this.initTotal);
    }
  }

  calculateTotal(vehicle: EAVehicles[]) {
    let total = 0;
    if (vehicle) {
      vehicle.forEach(x => {
        const vehicleValue = x.vehicleValue && x.vehicleValue !== '' ? Utilities.currencyToNumber(x.vehicleValue) : 0;
        const amountOwed = x.amountOwed && x.amountOwed !== '' ? Utilities.currencyToNumber(x.amountOwed) : 0;
        const vehicleEquity = vehicleValue - amountOwed;
        x.vehicleEquity = vehicleEquity >= 0 ? vehicleEquity.toLocaleString('en-US', { minimumFractionDigits: 2 }) : '0.00';
        total += Utilities.currencyToNumber(x.vehicleEquity);
      });
    }
    this.totalVehicleEquity = total.toLocaleString('en-US', { minimumFractionDigits: 2 });
    const tempTotalValue = total - this.disregardAmount >= 0 ? total - this.disregardAmount : 0;
    this.totalValuetowardsAssets = tempTotalValue.toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.vehiclesValue.emit(tempTotalValue);
  }
}
