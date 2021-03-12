import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SupportiveService } from '../../models/supportive-service.model';

@Component({
  selector: 'app-supportive-services-overview',
  templateUrl: './supportive-services-overview.component.html',
  styleUrls: ['./supportive-services-overview.component.scss']
})
export class SupportiveServicesOverviewComponent implements OnInit {
  @Input() supportiveServices: SupportiveService[];
  @Output() edit = new EventEmitter<string>();
  @Input() showEdit = true;
  constructor() {}

  ngOnInit() {}

  editSection() {
    this.edit.emit('supportive-service');
  }
}
