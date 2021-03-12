import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ContactsModule } from '../contacts/contacts.module';
import { RfaRoutingModule } from './rfa-routing.module';
import { RfaEditComponent } from './edit/edit.component';
import { RfaListComponent } from './list/list.component';
import { RfaPageComponent } from './page/page.component';
import { RfaChildrenRepeaterComponent } from './rfa-children-repeater/rfa-children-repeater.component';
import { RfaSingleComponent } from './single/single.component';
import { RfaService } from '../../shared/services/rfa.service';

@NgModule({
  imports: [CommonModule, SharedModule, ContactsModule, RfaRoutingModule],
  declarations: [RfaEditComponent, RfaListComponent, RfaPageComponent, RfaChildrenRepeaterComponent, RfaSingleComponent],
  providers: [RfaService]
})
export class RfaModule {}
