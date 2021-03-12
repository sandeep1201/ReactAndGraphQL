import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { PaymentDetailsRoutingModule } from './payment-details.routing';
import { PaymentDetailsComponent } from './payment-details.component';
import { PaymentDetailsService } from './services/payment-details.service';

@NgModule({
  declarations: [PaymentDetailsComponent],
  imports: [CommonModule, PaymentDetailsRoutingModule, SharedModule],
  providers: [PaymentDetailsService]
})
export class PaymentDetailsModule { }
