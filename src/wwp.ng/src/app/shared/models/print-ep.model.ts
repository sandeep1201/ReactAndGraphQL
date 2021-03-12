import { SupportiveService } from 'src/app/features-modules/employability-plan/models/supportive-service.model';
import { Activity } from 'src/app/features-modules/employability-plan/models/activity.model';
import { EpEmployment } from 'src/app/features-modules/employability-plan/models/ep-employment.model';
import { Goal } from 'src/app/features-modules/employability-plan/models/goal.model';
import { EmployabilityPlan } from 'src/app/features-modules/employability-plan/models/employability-plan.model';
import { Participant, OtherDemographicInformation } from './participant';

export class PrintEP {
  employabilityPlan: EmployabilityPlan;
  goals: Goal[];
  employmentInfo: EpEmployment[];
  activities: Activity[];
  supportiveServices: SupportiveService[];
  participant: Participant;
}
