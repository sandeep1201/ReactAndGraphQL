
import { InformalAssessmentService } from '../../shared/services/informal-assessment.service';
import { HistoryManager } from '../../shared/models/history-manager';
import { InputHistory } from '../../shared/models/input-history';

import { } from 'jasmine';

const jsonText = require('./bobRossLangHistory.json');

import * as TypeMoq from 'typemoq';


describe('History Manager: getHistory', () => {

    const json = jsonText;
    const informalAssessmentServiceMock: TypeMoq.IMock<InformalAssessmentService> = TypeMoq.Mock.ofType<InformalAssessmentService>();
    let historyManager: HistoryManager = new HistoryManager(informalAssessmentServiceMock.object, '123');

    beforeEach(() => {
        historyManager = new HistoryManager(informalAssessmentServiceMock.object, '123');
        historyManager.history = json;
    });
});
