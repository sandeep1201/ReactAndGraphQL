import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

import * as moment from 'moment';

@Component({
  selector: 'app-app-modified-stamp',
  templateUrl: './app-modified-stamp.component.html',
  styleUrls: ['./app-modified-stamp.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class AppModifiedStampComponent  {

  @Input() author: string;
  @Input() lastModified: moment.Moment;
  @Input() readOnly: boolean = false;
  @Input() showPencil: boolean = true;
}
