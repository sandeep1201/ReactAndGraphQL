import { InformalAssessmentService } from '../../shared/services/informal-assessment.service';
import { InputHistory } from '../../shared/models/input-history';
import { DropDownField } from './../../shared/models/dropdown-field';

import {} from 'jasmine';

const jsonText = require('./bobRossActionTasks.json');

import * as TypeMoq from 'typemoq';
import { ActionNeededListViewComponent } from 'src/app/features-modules/actions-needed/list-view/list-view.component';

describe('Action Needed: Sort US1266', () => {
  const json = jsonText;
  const informalAssessmentServiceMock: TypeMoq.IMock<InformalAssessmentService> = TypeMoq.Mock.ofType<InformalAssessmentService>();
  let actionNeededListViewComponent: ActionNeededListViewComponent = new ActionNeededListViewComponent();

  // Components inits dropdown that matches this index.
  // const sortOptions = ['Page', 'Priority', 'Completion Date', 'Due Date'];

  beforeEach(() => {
    actionNeededListViewComponent = new ActionNeededListViewComponent();
    actionNeededListViewComponent.listName = 'Worker';
    actionNeededListViewComponent.allTasks = jsonText;
  });

  it('Sort - Completion Date', () => {
    actionNeededListViewComponent.selectedSort = 2;
    actionNeededListViewComponent.sortTasks();
    expect(actionNeededListViewComponent.tasks.length).toEqual(8);
    expect(actionNeededListViewComponent.tasks[0].completionDate).toEqual('2016-01-02T00:00:00');
    expect(actionNeededListViewComponent.tasks[1].completionDate).toEqual('2016-01-01T00:00:00');
    expect(actionNeededListViewComponent.tasks[3].completionDate).toEqual(null);
    expect(actionNeededListViewComponent.tasks[5].isNoCompletionDate).toEqual(false);
    expect(actionNeededListViewComponent.tasks[7].isNoCompletionDate).toEqual(true);
  });

  it('Sort - Due Date', () => {
    actionNeededListViewComponent.selectedSort = 3;
    actionNeededListViewComponent.sortTasks();
    expect(actionNeededListViewComponent.tasks.length).toEqual(8);
    expect(actionNeededListViewComponent.tasks[0].dueDate).toEqual('2014-03-11T00:00:00');
    expect(actionNeededListViewComponent.tasks[1].dueDate).toEqual('2015-01-01T00:00:00');
    expect(actionNeededListViewComponent.tasks[1].priorityName).toEqual('High');
    expect(actionNeededListViewComponent.tasks[2].dueDate).toEqual('2015-01-01T00:00:00');
    expect(actionNeededListViewComponent.tasks[2].priorityName).toEqual('Medium');
  });

  it('Sort - Priority', () => {
    actionNeededListViewComponent.selectedSort = 1;
    actionNeededListViewComponent.sortTasks();
    expect(actionNeededListViewComponent.tasks.length).toEqual(8);
    expect(actionNeededListViewComponent.tasks[0].priorityName).toEqual('High');
    expect(actionNeededListViewComponent.tasks[5].priorityName).toEqual('Medium');
    expect(actionNeededListViewComponent.tasks[7].priorityName).toEqual('Low');
  });
});
