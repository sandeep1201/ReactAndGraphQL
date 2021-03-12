import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { EmergencyAssistanceRoutingModule } from './emergency-assistance.routing';
import { EARequestHistoryComponent } from './ea-request-history/ea-request-history.component';
import { EARequestEditComponent } from './ea-request-history/edit/edit.component';
import { EmergencyAssistanceService } from './services/emergancy-assistance.service';
import { EARequestDetailsComponent } from './ea-request-history/single/single.component';
import { EAGroupEditComponent } from './ea-request-history/edit-group-menbers/edit-group-members.component';
import { EARequestEditService } from './services/ea-request-edit.service';
import { EARequestDemographicsEditComponent } from './ea-request-history/edit/demographics.component';
import { EARequestEmergencyTypeEditComponent } from './ea-request-history/edit/emergency-type.component';
import { EARequestHouseholdMembersEditComponent } from './ea-request-history/edit/household-members.component';
import { EARequestHouseholdFinancialsEditComponent } from './ea-request-history/edit/household-financials.component';
import { EARequestAgencySummaryEditComponent } from './ea-request-history/edit/agency-summary.component';
import { IncomeRepeaterComponent } from './ea-request-history/edit-financials-repeaters/edit-income-repeater.component';
import { AssetsRepeaterComponent } from './ea-request-history/edit-financials-repeaters/edit-asset-repeater.component';
import { EAIPVListComponent } from './ea-request-history/ipv/list/list.component';
import { EAIPVEditComponent } from './ea-request-history/ipv/edit/edit.component';
import { EAPaymentEditComponent } from './ea-request-history/ea-payment/edit-ea-payment.component';
import { VehiclesRepeaterComponent } from './ea-request-history/edit-financials-repeaters/edit-vehicle-repeater.component';
import { FinancialNeedRepeaterComponent } from './ea-request-history/edit-financials-repeaters/edit-need-repeater.component';

@NgModule({
  declarations: [
    EARequestHistoryComponent,
    EARequestEditComponent,
    EARequestDetailsComponent,
    EAGroupEditComponent,
    EARequestDemographicsEditComponent,
    EARequestEmergencyTypeEditComponent,
    EARequestHouseholdMembersEditComponent,
    EARequestHouseholdFinancialsEditComponent,
    EARequestAgencySummaryEditComponent,
    IncomeRepeaterComponent,
    AssetsRepeaterComponent,
    VehiclesRepeaterComponent,
    FinancialNeedRepeaterComponent,
    EAIPVListComponent,
    EAIPVEditComponent,
    EAPaymentEditComponent
  ],
  imports: [CommonModule, EmergencyAssistanceRoutingModule, SharedModule],
  providers: [EmergencyAssistanceService, EARequestEditService]
})
export class EmergencyAssistanceModule {}
