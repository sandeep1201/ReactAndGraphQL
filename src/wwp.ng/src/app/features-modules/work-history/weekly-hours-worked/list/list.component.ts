import { Utilities } from './../../../../shared/utilities';
import { WeeklyHoursWorked } from './../../models/weekly-hours-worked.model';
import { Component, OnInit } from '@angular/core';
import { WeeklyHoursWorkedService } from '../../services/weekly-hours-worked.service';
import { Employment } from 'src/app/shared/models/work-history-app';
import { Participant } from 'src/app/shared/models/participant';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { PaginationInstance } from 'ng2-pagination';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class HourlyEntryListComponent implements OnInit {
  public isInEditMode = false;
  public isLoaded = false;
  public weeklyHoursWorkedEntries: WeeklyHoursWorked[];
  public weeklyHoursWorkedEntry: WeeklyHoursWorked = new WeeklyHoursWorked();
  public pin: string;
  public employment: Employment;
  public participant: Participant;
  public goBackUrl: string;

  public inConfirmDeleteView = false;
  public modelForDelete: WeeklyHoursWorked;
  public totalSubsidizedHours;
  public totalLifeTimeTMJTJSubsidizedHours;

  public maximumLifeTimeSubsidizedHoursAllowed = Employment.maximumLifeTimeSubsidizedHoursAllowed;
  public isReadOnly = false;
  public isSortByDateToggled = true;
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };
  public isStateStaff = false;
  constructor(private weeklyHoursWorkedService: WeeklyHoursWorkedService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    if (history.state.pin) {
      this.pin = history.state.pin;
      this.employment = history.state.employment;
      this.participant = history.state.participant;
      this.goBackUrl = `/pin/${this.pin}/work-history`;
      this.totalLifeTimeTMJTJSubsidizedHours = history.state.totalLifeTimeTMJTJSubsidizedHours;
      this.isStateStaff = history.state.isStateStaff;
      this.weeklyHoursWorkedService.getWeeklyHoursWorkedEntries(this.pin, this.employment.id).subscribe(res => {
        this.weeklyHoursWorkedEntries = res;
        this.totalSubsidizedHours = this.calculateTotalSubsidyHours();
        this.sortByDate();
        this.isLoaded = true;
      });
    } else {
      //TODO: see if this can be done in any other way..
      this.route.params.subscribe(params => {
        this.pin = params.pin;
        this.router.navigateByUrl(`/pin/${this.pin}/work-history`);
      });
    }
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  calculateTotalSubsidyHours() {
    let totalHours = 0;
    this.weeklyHoursWorkedEntries.forEach(entry => (totalHours += +entry.hours));
    return totalHours;
  }
  onAdd() {
    this.weeklyHoursWorkedEntry.id = 0;
    this.isInEditMode = true;
  }
  edit(entry: WeeklyHoursWorked) {
    this.weeklyHoursWorkedEntry = entry;
    this.isInEditMode = true;
  }
  onDelete(entry: WeeklyHoursWorked) {
    this.inConfirmDeleteView = true;
    this.modelForDelete = entry;
  }
  delete() {
    this.weeklyHoursWorkedService.deleteWeeklyHoursWorkedEntry(this.pin, this.modelForDelete).subscribe(res => {
      this.weeklyHoursWorkedEntries = res;
      this.reCalculateTotalLifeTimeSubsidyHours();
      this.inConfirmDeleteView = false;
    });
  }

  onConfirmDelete() {
    this.delete();
  }

  public canEdit(entry) {
    const tempStartDate = entry.startDate;
    return !(this.calculateTotalSubsidyHours() < this.maximumLifeTimeSubsidizedHoursAllowed && Math.abs(moment(tempStartDate).diff(Utilities.currentDate, 'days')) <= 30);
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }
  reCalculateTotalLifeTimeSubsidyHours() {
    this.totalLifeTimeTMJTJSubsidizedHours -= this.totalSubsidizedHours;
    this.totalSubsidizedHours = this.calculateTotalSubsidyHours();
    this.totalLifeTimeTMJTJSubsidizedHours += this.totalSubsidizedHours;
  }
  onExitEditView(event) {
    if (event.afterPostCallRes) {
      this.weeklyHoursWorkedEntries = event.afterPostCallRes;
      this.reCalculateTotalLifeTimeSubsidyHours();
      this.isSortByDateToggled = true;
      this.sortByDate();
    }
    this.isInEditMode = false;
  }

  sortByDate() {
    this.weeklyHoursWorkedEntries = Utilities.sortArrayByDate(this.weeklyHoursWorkedEntries, 'startDate', this.isSortByDateToggled);
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }
}
