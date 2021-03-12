import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrganizationInformationRoutingModule } from './organization-information-routing.module';
import { OrganizationInformationEditComponent } from './edit/edit.component';
import { OrganizationInormationListComponent } from './list/list.component';
import { OrganizationInformationComponent } from './organization-information.component';
import { OrganizationInformationService } from './services/organization-information.service';

@NgModule({
  imports: [CommonModule, SharedModule, OrganizationInformationRoutingModule],
  declarations: [OrganizationInformationEditComponent, OrganizationInormationListComponent, OrganizationInformationComponent],
  providers: [OrganizationInformationService]
})
export class OrganizationInformationModule {}
