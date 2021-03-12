import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkHistoryWageComponent } from './current-wage/current-wage.component';
import { WorkHistoryEditComponent } from './edit/edit.component';
import { WorkHistoryEmbedComponent } from './embed/embed.component';
import { WorkHistoryLeaveHistoryComponent } from './leave-history/leave-history.component';
import { WorkHistoryListComponent } from './list/list.component';
import { WorkHistoryListPageComponent } from './list-page/list-page.component';
import { WorkHistoryPastWageComponent } from './past-wage/past-wage.component';
import { PrintComponent } from './print/print.component';
import { WorkHistorySingleComponent } from './single/single.component';
import { WorkHistoryWageHistoryComponent } from './wage-history/wage-history.component';
import { WorkHistoryAppService } from '../../shared/services/work-history-app.service';
import { ContactsModule } from '../contacts/contacts.module';
import { HourlyEntryListComponent } from './weekly-hours-worked/list/list.component';
import { HourlyEntryEditComponent } from './weekly-hours-worked/edit/edit.component';

@NgModule({
  imports: [CommonModule, SharedModule, ContactsModule],
  declarations: [
    WorkHistoryWageComponent,
    WorkHistoryEditComponent,
    WorkHistoryEmbedComponent,
    WorkHistoryLeaveHistoryComponent,
    WorkHistoryListComponent,
    WorkHistoryListPageComponent,
    WorkHistoryPastWageComponent,
    PrintComponent,
    WorkHistorySingleComponent,
    WorkHistoryWageHistoryComponent,
    HourlyEntryListComponent,
    HourlyEntryEditComponent
  ],
  exports: [
    WorkHistoryWageComponent,
    WorkHistoryEditComponent,
    WorkHistoryEmbedComponent,
    WorkHistoryLeaveHistoryComponent,
    WorkHistoryListComponent,
    WorkHistoryListPageComponent,
    WorkHistoryPastWageComponent,
    PrintComponent,
    WorkHistorySingleComponent,
    WorkHistoryWageHistoryComponent,
    HourlyEntryListComponent
  ]
})
export class WorkHistoryModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: WorkHistoryModule,
      providers: [WorkHistoryAppService]
    };
  }
}
