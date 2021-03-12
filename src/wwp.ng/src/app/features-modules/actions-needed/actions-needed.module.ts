import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ActionNeededEditComponent } from './edit/edit.component';
import { ActionNeededEmbedComponent } from './embed/embed.component';
import { ActionNeededEmbedTaskComponent } from './embed/embed-task/embed-task.component';
import { ActionNeededListViewComponent } from './list-view/list-view.component';
import { ActionNeededListPipe } from './pipes/action-needed-list.pipe';
import { ActionNeededTaskComponent } from './task/task.component';
import { ActionsNeededComponent } from './actions-needed.component';
import { ActionNeededService } from './services/action-needed.service';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [
    ActionsNeededComponent,
    ActionNeededEditComponent,
    ActionNeededEmbedComponent,
    ActionNeededEmbedTaskComponent,
    ActionNeededListViewComponent,
    ActionNeededTaskComponent,
    ActionNeededListPipe
  ],
  exports: [
    ActionsNeededComponent,
    ActionNeededEditComponent,
    ActionNeededEmbedComponent,
    ActionNeededEmbedTaskComponent,
    ActionNeededListViewComponent,
    ActionNeededTaskComponent,
    ActionNeededListPipe
  ],
  entryComponents: [ActionNeededEditComponent]
})
export class ActionsNeededModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ActionsNeededModule,
      providers: [ActionNeededService]
    };
  }
}
