import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-employments-overview',
  templateUrl: './employments-overview.component.html',
  styleUrls: ['./employments-overview.component.scss']
})
export class EpEmploymentsOverviewComponent implements OnInit {
  @Input() employments: any;
  @Input() showEdit = true;
  @Output() edit = new EventEmitter<string>();

  constructor() {}

  ngOnInit() {}
  editSection() {
    this.edit.emit('employments');
  }
}
