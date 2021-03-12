import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ContactDetailComponent } from './detail/detail.component';
import { ContactsEditComponent } from './edit/edit.component';
import { ContactsEmbedComponent } from './embed/embed.component';
import { ContactsListComponent } from './list/list.component';
import { ContactsListPageComponent } from './list-page/list-page.component';
import { ContactsSelectComponent } from './select/select.component';
import { ContactsService } from './services/contacts.service';
import { ContactsRepeaterComponent } from './repeater/repeater.component';
// import { ContactsRoutingModule } from './contacts-roting.module';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [
    ContactDetailComponent,
    ContactsEditComponent,
    ContactsEmbedComponent,
    ContactsListComponent,
    ContactsListPageComponent,
    ContactsSelectComponent,
    ContactsRepeaterComponent
  ],
  exports: [
    ContactDetailComponent,
    ContactsEditComponent,
    ContactsEmbedComponent,
    ContactsListComponent,
    ContactsListPageComponent,
    ContactsSelectComponent,
    ContactsRepeaterComponent
  ]
})
export class ContactsModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ContactsModule,
      providers: [ContactsService]
    };
  }
}
