import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-ep-card-header',
  templateUrl: './ep-card-header.component.html',
  styleUrls: ['./ep-card-header.component.scss']
})
export class EpCardHeaderComponent implements OnInit {
  @Input() title: string;
  @Input() showEdit = true;
  @Output() editSec = new EventEmitter();
  @Output() elapsedSec = new EventEmitter();

  @Input() elapsedActivityFlag: boolean;

  constructor() {}

  ngOnInit() {}
  edit() {
    this.editSec.emit();
  }
  endElapsedActivities() {
    this.elapsedSec.emit();
  }
}
