import { TransactionsService as TransactionService } from './services/transactions.service';
import { Participant } from './../../shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { TransactionModel } from './transactions.model';
import { Utilities } from 'src/app/shared/utilities';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-transactions',
  templateUrl: './transactions.component.html',
  styleUrls: ['./transactions.component.scss']
})
export class TransactionsComponent implements OnInit {
  public isLoaded = false;
  public isInEditMode = false;
  private pin: string;
  public participant: Participant;
  public goBackUrl = '';
  public transactions: TransactionModel[] = [];
  private originalTransactions: TransactionModel[] = [];
  searchQuery = '';

  public agencyDrop: DropDownField[] = [];
  private originalAgencyDrop: DropDownField[] = [];
  public agencyTypes = [];
  public workerDrop: DropDownField[] = [];
  private originalWorkerDrop: DropDownField[] = [];
  public workerTypes = [];
  public transactionTypeDrop: DropDownField[] = [];
  private originalTransactionTypeDrop: DropDownField[] = [];
  public transactionTypes = [];
  public fromDate: string;
  public toDate: string;
  public goDisableFlag = true;
  public isSortByDateToggled = false;
  private dateFiltered = false;

  constructor(private route: ActivatedRoute, private transactionService: TransactionService, private partService: ParticipantService) {}

  ngOnInit(): void {
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.goBackUrl = '/pin/' + this.pin;
    this.partService
      .getCachedParticipant(this.pin)
      .pipe(
        concatMap(participant => {
          this.participant = participant;
          return this.transactionService.getTransactions(this.participant.id);
        })
      )
      .subscribe(transactions => {
        this.transactions = transactions;
        this.originalTransactions = transactions;
        this.initAgencyDrop();
        this.originalAgencyDrop = this.agencyDrop;
        this.initWorkerDrop();
        this.originalWorkerDrop = this.workerDrop;
        this.initTransactionTypeDrop();
        this.originalTransactionTypeDrop = this.transactionTypeDrop;
        this.isLoaded = true;
      });
  }

  initAgencyDrop() {
    const agencyTypeNames: DropDownField[] = [];
    this.originalTransactions.forEach(i => {
      if (i.agencyId) {
        const agency = new DropDownField();
        agency.id = i.agencyId;
        agency.name = i.agencyName;
        agencyTypeNames.push(agency);
      }
    });
    this.agencyDrop = _.orderBy<DropDownField>(_.uniqBy(agencyTypeNames, 'id'), [o => o.name], ['asc']);
    this.agencyTypes = this.agencyDrop && this.agencyDrop.length > 0 ? this.agencyTypes : [];
  }

  initWorkerDrop() {
    const workerNames: DropDownField[] = [];
    this.originalTransactions.forEach(i => {
      if (i.workerId) {
        const worker = new DropDownField();
        worker.id = i.workerId;
        worker.name = i.workerName;
        workerNames.push(worker);
      }
    });
    this.workerDrop = _.orderBy<DropDownField>(_.uniqBy(workerNames, 'id'), [o => o.name], ['asc']);
    this.workerTypes = this.workerDrop && this.workerDrop.length > 0 ? this.workerTypes : [];
  }

  initTransactionTypeDrop() {
    const transactionTypeNames: DropDownField[] = [];
    this.originalTransactions.forEach(i => {
      if (i.transactionTypeId) {
        const transactionType = new DropDownField();
        transactionType.id = i.transactionTypeId;
        transactionType.name = i.transactionTypeName;
        transactionTypeNames.push(transactionType);
      }
    });
    this.transactionTypeDrop = _.orderBy<DropDownField>(_.uniqBy(transactionTypeNames, 'name'), [o => o.name], ['asc']);
    this.transactionTypes = this.transactionTypeDrop && this.transactionTypeDrop.length > 0 ? this.transactionTypes : [];
  }

  filterSearchList(searchQuery: string) {
    if (!Utilities.isStringEmptyOrNull(searchQuery) && this.originalTransactions != null) {
      const query = searchQuery.trim().toLowerCase();
      this.transactions = this.originalTransactions.filter(x => x.description.toLowerCase().indexOf(query) > -1);
    } else {
      this.transactions = this.originalTransactions;
    }
  }

  sortByDate() {
    this.transactions = Utilities.sortArrayByDate(this.transactions, 'createdDate', this.isSortByDateToggled);
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }

  resetFilter() {
    this.transactions = this.originalTransactions;
    this.agencyDrop = this.originalAgencyDrop;
    this.workerDrop = this.originalWorkerDrop;
    this.transactionTypeDrop = this.originalTransactionTypeDrop;
    this.agencyTypes = [];
    this.workerTypes = [];
    this.transactionTypes = [];
    this.fromDate = null;
    this.toDate = null;
    this.searchQuery = '';
    this.goDisableFlag = true;
    this.isSortByDateToggled = false;
    this.dateFiltered = false;
  }

  filter(isDate = false) {
    let agencyTypes: any[];
    let workerTypes: any[];
    let transactionTypes: number[];
    if (this.agencyTypes) agencyTypes = this.agencyTypes.length > 0 ? this.agencyTypes : this.originalAgencyDrop.map(i => i.id);
    if (this.workerTypes) workerTypes = this.workerTypes.length > 0 ? this.workerTypes : this.originalWorkerDrop.map(i => i.id);
    if (this.transactionTypes) transactionTypes = this.transactionTypes.length > 0 ? this.transactionTypes : this.originalTransactionTypeDrop.map(i => i.id);
    if (this.originalTransactions && agencyTypes && workerTypes && transactionTypes) {
      const transactionTypeNames = Utilities.fieldDataNamesByIds(transactionTypes, this.originalTransactionTypeDrop);
      this.filterSearchList(this.searchQuery);
      this.transactions = this.transactions.filter(
        i => agencyTypes.indexOf(i.agencyId) > -1 && transactionTypeNames.indexOf(i.transactionTypeName) > -1 && workerTypes.indexOf(i.workerId) > -1
      );
    }
    if (isDate || this.dateFiltered) this.dateFilter();
  }

  dateFilter() {
    let fromDate: any;
    let toDate: any;
    const sorted = _.orderBy<TransactionModel>(this.transactions, [o => new Date(o.modifiedDate)], ['asc']);
    if (sorted) {
      fromDate = this.fromDate && this.fromDate.trim() !== '' ? moment(new Date(this.fromDate).toDateString()) : moment(new Date(sorted[0].modifiedDate).toDateString());
      toDate = this.toDate && this.toDate.trim() !== '' ? moment(new Date(this.toDate).toDateString()) : moment(new Date(sorted[sorted.length - 1].modifiedDate).toDateString());
    }
    this.transactions = this.transactions.filter(
      i => i.createdDate && moment(new Date(i.createdDate).toDateString()).isSameOrAfter(fromDate) && moment(new Date(i.createdDate).toDateString()).isSameOrBefore(toDate)
    );
    this.goDisableFlag = true;
    this.dateFiltered = true;
  }

  setGoFlag() {
    const fromDate = moment(this.fromDate, 'MM/DD/YYYY');
    const toDate = moment(this.toDate, 'MM/DD/YYYY');
    const currentDate = moment(moment().format('MM/DD/YYYY'), 'MM/DD/YYYY');
    const toBeforeFrom = toDate.isBefore(fromDate);
    let isFromValid = false;
    let isToValid = false;
    if (this.fromDate && this.fromDate.trim() !== '' && this.fromDate.length === 10 && fromDate.isValid() && fromDate.isSameOrBefore(currentDate)) isFromValid = true;
    if (this.toDate && this.toDate.trim() !== '' && this.toDate.length === 10 && toDate.isValid() && toDate.isSameOrBefore(currentDate)) isToValid = true;
    this.goDisableFlag = !isFromValid || !isToValid || toBeforeFrom;
  }
}
