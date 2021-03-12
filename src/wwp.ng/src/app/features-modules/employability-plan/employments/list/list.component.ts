import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { EpEmployment } from '../../models/ep-employment.model';

@Component({
  selector: 'app-epemployments-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class EpEmploymentsListComponent implements OnInit {
  @Input() employments: EpEmployment[];
  @Input() showControls = true;
  @Input() isReadOnly = true;
  @Output() selectedEmployment = new EventEmitter();
  public checked: boolean;
  public tempEmps;

  constructor(private appService: AppService) {}

  ngOnInit() {
    if (this.employments) {
      this.tempEmps = this.employments.splice(0);
    }
  }

  selected() {
    this.selectedEmployment.emit(this.tempEmps);
    this.appService.componentDataModified.next({ dataModified: true });
    this.appService.isEPUrlChangeBlocked = true;
  }
}
