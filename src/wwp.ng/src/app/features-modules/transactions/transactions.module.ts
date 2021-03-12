import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { TransactionsComponent } from './transactions.component';
import { TransactionsRoutingModule } from './transactions.routing';
import { TransactionsService } from './services/transactions.service';

@NgModule({
  declarations: [TransactionsComponent],
  imports: [CommonModule, TransactionsRoutingModule, SharedModule],
  providers: [TransactionsService]
})
export class TransactionsModule {}
