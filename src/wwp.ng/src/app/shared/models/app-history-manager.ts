import * as moment from 'moment';

import * as _ from 'lodash';
import { DropDownField } from './dropdown-field';
import { ParticipantService } from '../../shared/services/participant.service';
import { ParticipantBarrierAppService } from '../services/participant-barrier-app.service';

/**
 *  PIN passed class that gives us history for an App.
 *
 * @export
 * @class AppHistoryManager
 */
export class AppHistoryManager {
  public history: any[];
  public historyDrop: DropDownField[];
  private section: any;

  constructor(private partService: ParticipantService, private pin: string) {
    this.history = [];
  }

  initHistoryDrop() {
    if (this.history !== null) {
      this.historyDrop = new Array<DropDownField>();
      for (let i = 0; i < this.history.length; i++) {
        const field = new DropDownField();
        field.id = i;
        field.name = moment(this.history[i].modifiedDate).format('MM/DD/YYYY h:mm:ss A') + ' - ' + this.history[i].modifiedBy;
        field.isSelected = i === 0;
        this.historyDrop.push(field);
      }
    }
  }

  getHistoryAtIndex(index) {
    return this.history[index];
  }
}
