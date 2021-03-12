import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrugScreeningComponent } from './drug-screening.component';
import { DrugScreeningRoutingModule } from './drug-screening-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { DrugScreeningService } from './services/drug-screening.service';

@NgModule({
  declarations: [DrugScreeningComponent],
  imports: [CommonModule, DrugScreeningRoutingModule, SharedModule],
  providers: [DrugScreeningService]
})
export class DrugScreeningModule {}
