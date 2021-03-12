import { AppService } from 'src/app/core/services/app.service';
import { Timeline } from '../../models/time-limits/timeline';
import { Component, OnInit, Input, Output, forwardRef, OnChanges, EventEmitter, ChangeDetectionStrategy, HostBinding, IterableDiffers, IterableDiffer } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { GenericBaseComponent } from '../base-component';
import * as moment from 'moment';
import { Dictionary } from '../../dictionary';
import { DropDownMultiField, GenericDropDownMultiField } from '../../models/dropdown-multi-field';
import { DateRange, range, rangeInterval } from '../../moment-range';
import { remove, unique } from '../../arrays';
import { Utilities } from '../../utilities';

@Component({
  selector: 'app-month-selector',
  templateUrl: './month-selector.component.html',
  styleUrls: ['./month-selector.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MonthSelectorComponent),
      multi: true
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MonthSelectorComponent extends GenericBaseComponent<moment.Moment[]> implements OnInit, ControlValueAccessor, OnChanges, OnInit {
  @Input() public startMonth: moment.MomentInput = Utilities.currentDate.clone();

  public selectedMonths: moment.Moment[] = [];
  @Input() public excludedMonths: moment.Moment[] = [];
  @Input() public minMonth: moment.MomentInput = [1996, 8, 1];
  @Input() public maxMonth: moment.MomentInput = moment(Utilities.currentDate)
    .add(12, 'months')
    .endOf('year'); // default to the end of next year

  @Input() public dynamicTitleMaxItems = 3;
  @Input() public defaultTitle = 'Select Month(s)';
  @HostBinding('class.modal') @Input() isModal: boolean = false;

  private isError: boolean = false;
  @Input() isDisabled: boolean = false;
  @Input() isInvalid: boolean = false;

  monthsDropDownList: GenericDropDownMultiField<moment.Moment>[];

  displayYear1: DateRange;
  displayYear2: DateRange;

  selectedIndex: number;
  isEdit: boolean;
  title: string;
  differ: IterableDiffer<any>;
  getMonthOptions(year: number): GenericDropDownMultiField<moment.Moment>[] {
    // let yearRange = yearIndex === 1 ? this.displayYear1 : this.displayYear2;
    // let yearRange = this.displayYear1.start.year() === year ? this.displayYear1 : this.displayYear2;
    let yearRange = rangeInterval([year, 0, 1], 'year');
    let start = this.monthsDropDownList.findIndex(x => x.id.isSame(yearRange.start, 'month'));
    let end = this.monthsDropDownList.findIndex(x => x.id.isSame(yearRange.end, 'month')) + 1;
    return this.monthsDropDownList.slice(start, end);
  }

  getSelectedMonths(year?: number) {
    let result = year == null ? this.selectedMonths : this.selectedMonths.filter(x => x.year() === year);
    return result.sort((x, y) => {
      return x.diff(y);
    });
  }

  get selectedYears() {
    let result = unique(this.selectedMonths, x => {
      return x.year();
    })
      .map(x => {
        return x.year();
      })
      .sort();
    return result;
  }

  private _today = moment(Utilities.currentDate);

  public isCurrentItem(date: moment.Moment): boolean {
    if (date) {
      return date.isSame(this.startMonth, 'month');
    }
    return false;
  }

  public isLastSelected(date: moment.Moment): boolean {
    if (this.lastSelectedItem && this.lastSelectedItem.id) {
      return this.lastSelectedItem.id.isSame(date);
    }
    return false;
  }

  constructor(private differs: IterableDiffers) {
    super();
    this.differ = differs.find([]).create(null);
  }

  ngOnInit() {
    // this.selectedMonths.push(moment(this.startMonth)); TODO: Just highlight this one somehow
    this.displayYear1 = rangeInterval(this.startMonth, 'year');
    this.displayYear2 = rangeInterval(moment(this.startMonth).add(1, 'year'), 'year');

    this.initMonthsDropDownList();
  }
  ngOnChanges() {
    if (this.isInvalid != null && this.isInvalid === true) {
      this.isError = true;
    } else {
      this.isError = false;
    }
    this.updateTitle();
  }
  ngDoCheck() {
    let changes = this.differ.diff(this.selectedMonths);
    if (changes) {
      this.updateTitle();
    }
  }

  initMonthsDropDownList() {
    this.monthsDropDownList = [];

    // create a range to iterate over
    let range = new DateRange(moment(this.minMonth).subtract(1, 'year'), this.maxMonth);

    for (let month of Array.from(range.by('month'))) {
      let monthOption = new GenericDropDownMultiField<moment.Moment>();
      monthOption.id = month;
      monthOption.name = month.format('MMM');
      monthOption.disablesOthers = false;
      monthOption.isSelected = !!this.selectedMonths.find(x => x.isSame(month, 'month'));
      monthOption.isDisabled =
        month.isSameOrAfter(Utilities.currentDate, 'month') ||
        month.isSameOrBefore(this.minMonth, 'month') ||
        month.isSame(this.startMonth, 'month') ||
        !!this.excludedMonths.find(x => x.isSame(month, 'month'));
      this.monthsDropDownList.push(monthOption);
    }
  }

  changeYears(offset: number) {
    if (offset === 1 && this.displayYear2.end.year() >= moment(this.maxMonth).year()) {
      return;
    } else if (offset === -1 && this.displayYear1.start.year() < moment(this.minMonth).year()) {
      return;
    }

    this.displayYear1 = rangeInterval(this.displayYear1.start.clone().add(offset, 'year'), 'year');
    this.displayYear2 = rangeInterval(this.displayYear2.start.clone().add(offset, 'year'), 'year');
  }

  private lastSelectedItem: GenericDropDownMultiField<moment.Moment>;
  onMonthClicked(item: GenericDropDownMultiField<moment.Moment>, event: MouseEvent) {
    if (item.isDisabled) {
      return;
    }

    if (event.shiftKey) {
      event.preventDefault();
      event.stopPropagation();
      if (!this.lastSelectedItem) {
        this.lastSelectedItem = item;
        item.isSelected = false;
        this.toggleMonth(item);
      } else {
        this.selectMonthRange(item);
        this.lastSelectedItem = null;
      }
    } else {
      this.toggleMonth(item);
      this.lastSelectedItem = item;
    }
  }

  toggleMonth(item: GenericDropDownMultiField<moment.Moment>) {
    item.isSelected = !item.isSelected;
    this.updateSelectedMonth(item);
    this.onChangeCallback(this.selectedMonths);
  }

  updateSelectedMonth(item: GenericDropDownMultiField<moment.Moment>) {
    if (item.isSelected) {
      this.selectedMonths.push(item.id);
    } else {
      remove<moment.Moment>(this.selectedMonths, item.id, (x, y) => {
        return x.isSame(y, 'month');
      });
    }
  }

  selectMonthRange(endItem: GenericDropDownMultiField<moment.Moment>) {
    if (endItem && this.lastSelectedItem != endItem) {
      for (let item of this.monthsDropDownList) {
        if (item.isDisabled || item.isSelected) {
          continue;
        }

        if (item.id.isBetween(this.lastSelectedItem.id, endItem.id, 'month', '[]')) {
          item.isSelected = true;
        }
        this.updateSelectedMonth(item);
      }
    }
    this.lastSelectedItem = null;
    this.onChangeCallback(this.selectedMonths);
  }

  toggleIsEdit($event) {
    this.isEdit = !this.isEdit;
  }

  writeValue(obj: any) {
    if (obj !== undefined) {
      this.selectedMonths = obj;
    }
  }
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }
  setDisabledState(isDisabled: boolean) {
    this.isDisabled = isDisabled;
  }

  updateTitle() {
    if (this.selectedMonths.length === 0) {
      this.title = this.defaultTitle;
    } else if (this.dynamicTitleMaxItems >= this.selectedMonths.length) {
      this.title = this.getSelectedMonths()
        .map(x => {
          return x.format('MMM-YYYY');
        })
        .join(', ');
    } else {
      this.title = `${this.selectedMonths.length} month(s) selected`;
    }
  }
}
