import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ChangeDetectionStrategy } from '@angular/core';
import { TimelineMonth, ClockType, ClockTypes } from '../../../shared/models/time-limits';


@Component({
  selector: 'app-month-box',
  templateUrl: './month-box.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  styleUrls: ['./month-box.component.css']
})
export class MonthBoxComponent implements OnInit {

  @Input() index: number;
  @Input() isSelected: boolean;
  @Input() model: TimelineMonth;
  @Input() prev: TimelineMonth;
  @Input() next: TimelineMonth;
  @Output() selected = new EventEmitter<number>();

  get federalBarClasses() {
    return this.getBarClasses('Federal');
  }
  get stateBarClasses() {
    return this.getBarClasses('State');
  }
  get placementBarClasses() {
    return this.getBarClasses('Placement');
  }

  get containerClasses() {
    return {
      'year-ind': this.model.isJanuary,
      'today': this.model.isCurrentMonth,
      'used': this.model.tick && !this.model.tick.clockTypes.eql(ClockTypes.None),
      'paid': this.model.isPaid,
      'future': this.model.isFuture,
      'edited': this.model.isEdited,
      'extension': this.model.hasExtension()
    }
  }

  constructor(public elementRef: ElementRef) { }

  ngOnInit() {
  }

  onMonthClick() {
    this.selected.emit(this.index);
  }

  private getBarClasses(barType: 'Federal' | 'State' | 'Placement') {

    const prev = this.prev || new TimelineMonth(this.model.date.clone().subtract(1, 'month'));
    const next = this.next || new TimelineMonth(this.model.date.clone().add(1, 'month'));

    const barClasses = new Bar(); // { ticked: false, start: false, end: false, extension: false };

    const isTicked = this.model.tick.clockTypes.valueOf() !== ClockTypes.None; //null or undefined

    // We have to mark unused months as "ticked" when they are elapsed and in an extensions
    const tickIsFederal = isTicked && this.model.tick.clockTypes.state.Federal;
    const tickIsState = (isTicked && this.model.tick.clockTypes.state.State) || (this.model.hasExtension(ClockTypes.State));
    const tickIsPlacement = (isTicked && this.model.tick.clockTypes.state.PlacementLimit && !this.model.tick.clockTypes.state.NOPlacementLimit ) || (this.model.hasExtension(ClockTypes.PlacementLimit));

    const startFederalTick = tickIsFederal && (!prev.tick.clockTypes.state.Federal);
    const endFederalTick = tickIsFederal && (!next.tick.clockTypes.state.Federal);

    const hasStateExt = this.model.hasExtension(ClockTypes.State);
    const startStateTick = tickIsState && !prev.tick.clockTypes.state.State && !prev.hasExtension(ClockTypes.State);
    const endStateTick = tickIsState && !next.tick.clockTypes.state.State && !next.hasExtension(ClockTypes.State);

    const hasPlacementExt = this.model.hasExtension(ClockTypes.PlacementLimit);
    const startPlacementTick = tickIsPlacement && (!prev.tick.clockTypes.state.PlacementLimit || prev.tick.clockTypes.state.NOPlacementLimit) && !prev.hasExtension(ClockTypes.PlacementLimit);
    const endPlacementTick = tickIsPlacement && (!next.tick.clockTypes.state.PlacementLimit || next.tick.clockTypes.state.NOPlacementLimit) && !next.hasExtension(ClockTypes.PlacementLimit);


    if (barType === 'Federal') {
      barClasses.ticked = tickIsFederal;
      barClasses.start = startFederalTick;
      barClasses.end = endFederalTick || (!this.model.isFuture && next.isFuture);
    }

    if (barType === 'State') {
      barClasses.ticked = tickIsState;
      barClasses.start = startStateTick;
      barClasses.end = endStateTick || (!this.model.isFuture && next.isFuture);
      barClasses.extension = hasStateExt;
    }

    if (barType === 'Placement') {
      barClasses.ticked = tickIsPlacement;
      barClasses.start = startPlacementTick;
      barClasses.end = endPlacementTick || (!this.model.isFuture && next.isFuture);
      barClasses.extension = hasPlacementExt;
    }

    return barClasses;
  }

}


class Bar {

  ticked = false;
  start = false;
  end = false;
  extension = false;


}
