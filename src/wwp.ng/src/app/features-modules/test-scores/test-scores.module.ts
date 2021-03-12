import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { TestScoreCardComponent } from './card/card.component';
import { TestScoresEditComponent } from './edit/edit.component';
import { TestScoresEmbedComponent } from './embed/embed.component';
import { TestScoresListComponent } from './list/list.component';
import { TestScoresListPageComponent } from './list-page/list-page.component';
import { TestScoresService } from '../../shared/services/test-scores.service';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [
    TestScoreCardComponent,
    TestScoresEditComponent,
    TestScoresEmbedComponent,
    TestScoresListComponent,
    TestScoresListPageComponent
  ],
  exports: [
    TestScoreCardComponent,
    TestScoresEditComponent,
    TestScoresEmbedComponent,
    TestScoresListComponent,
    TestScoresListPageComponent
  ],
  entryComponents: [TestScoresListPageComponent]
})
export class TestScoresModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: TestScoresModule,
      providers: [TestScoresService]
    };
  }
}
