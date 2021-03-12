import { Pipe, PipeTransform } from '@angular/core';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';

@Pipe({
  name: 'actionNeededFilter'
})
export class ActionNeededListPipe implements PipeTransform {
  transform(value: ActionNeededTask[], filterOnGoing: boolean, filterRecentActions: boolean, filterCompleted: boolean, filterNotCompleted: boolean): ActionNeededTask[] {
    if (value == null) {
      return;
    }
    const filteredList: ActionNeededTask[] = [];

    for (const v of value) {
      if (filterOnGoing === true && v.getTaskStatus().toLowerCase() === ActionNeededTask.taskStatuses[0].toLowerCase()) {
        filteredList.push(v);
      } else if (filterRecentActions === true && v.isRecentTask() === true) {
        filteredList.push(v);
      } else if (filterCompleted === true && v.getTaskStatus().toLowerCase() === ActionNeededTask.taskStatuses[2].toLowerCase()) {
        filteredList.push(v);
      } else if (filterNotCompleted === true && v.getTaskStatus().toLowerCase() === ActionNeededTask.taskStatuses[3].toLowerCase()) {
        filteredList.push(v);
      } else if (filterOnGoing !== true && filterRecentActions !== true && filterCompleted !== true && filterNotCompleted !== true) {
        //filteredList.push(v);
      }
    }

    return filteredList;
  }
}
