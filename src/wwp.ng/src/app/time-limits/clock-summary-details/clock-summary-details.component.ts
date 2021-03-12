import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ChangeDetectionStrategy
} from '@angular/core';
import { ClockTypes, ClockType, TimelineMonth } from '../../shared/models/time-limits';



@Component({
  selector: 'app-clock-details',
  templateUrl: './clock-summary-details.component.html',
  styleUrls: ['./clock-summary-details.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ClockSummaryDetailsComponent implements OnInit {

  ClockTypes = ClockTypes;
  ClockType = ClockType;
  public monthClockTypes: ClockTypes[] = [];
  public showPayment = false;
  public timelimitsMap = new Map<ClockTypes, number>();
  @Input() clockType: ClockTypes;
  @Output() public close = new EventEmitter();
  @Output() monthSelected = new EventEmitter<TimelineMonth>();
  @Input() public get months(): TimelineMonth[] {
    return this._months;
  }

  public set months(val: TimelineMonth[]) {
    this._months = val;
    this._months.sort((a,b)=>{return b.date.diff(a.date)});
    this.initSummaryMap();
  }

  private _months: TimelineMonth[] = [];

  constructor() {

  }

  initSummaryMap() {
    this.timelimitsMap.clear();
    this.months.map(x => {
      if(x.tick.clockTypes.state.PlacementLimit && x.tick.clockTypes.state.NOPlacementLimit){
        this.timelimitsMap.set(ClockTypes.NoPlacementLimit, (this.timelimitsMap.get(ClockTypes.NoPlacementLimit) || 0) + 1);
      }else{
        this.timelimitsMap.set(x.tick.tickType, (this.timelimitsMap.get(x.tick.tickType) || 0) + 1);
      }
    });
    this.monthClockTypes = Array.from(this.timelimitsMap.keys());
  }

  ngOnInit() {
  }

  exit() {
    this.clockType = null;
    this.close.emit();
  }

  public clockTotal(clockType: ClockTypes) {
    return this.timelimitsMap.get(clockType);
  }
  selectMonthAndExit(index: number) {
    if (index > this.months.length - 1) {
      index = this.months.length - 1;
    }

    if (index < 0) {
      index = 0;
    }

    this.exit();
    this.monthSelected.emit(this.months[index]);
  }
}
