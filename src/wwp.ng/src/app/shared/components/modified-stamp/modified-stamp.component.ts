import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

import * as moment from 'moment';

@Component({
  selector: 'app-modified-stamp',
  templateUrl: './modified-stamp.component.html',
  styleUrls: ['./modified-stamp.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ModifiedStampComponent {
  @Input() author: string;
  @Input() lastModified: moment.Moment;
  @Input() readOnly: boolean = false;
  @Input() label: string = 'Last Edited by';
  @Input() showPencil: boolean = true;
  @Input() deleteReason: string;
  @Input() showTrash: boolean = false;
}
