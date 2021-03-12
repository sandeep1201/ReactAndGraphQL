import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EAIPV } from '../../../models/ea-ipv.model';

@Component({
  selector: 'app-ipv-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class EAIPVListComponent {
  @Input() ipvList: EAIPV[];
  @Output() goToSingleEntry = new EventEmitter();

  constructor() {}

  singleEntry(ipv: EAIPV, isReadOnly: boolean) {
    this.goToSingleEntry.emit({ ipvModel: ipv, isReadOnly: isReadOnly });
  }
}
