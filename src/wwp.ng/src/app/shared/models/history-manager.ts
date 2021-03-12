import { InformalAssessmentService } from '../../shared/services/informal-assessment.service';

import * as moment from 'moment';

import * as _ from 'lodash';
import { DropDownField } from './dropdown-field';

/**
 *  PIN passed class that gives us history for a informal assessment section.
 *
 * @export
 * @class HistoryManager
 */
export class HistoryManager {
  public history: any[];
  public historyDrop: DropDownField[];
  private section: any;

  constructor(private iaService: InformalAssessmentService, private pin: string) {
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
