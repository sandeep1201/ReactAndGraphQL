import { ChildAndYouthSupportsSection } from './child-youth-supports-section';
import { EducationHistorySection } from './education-history-section';
import { FamilyBarriersSection } from './family-barriers-section';
import { HousingSection } from './housing-section';
import { LanguagesSection } from './languages-section';
import { LegalIssuesSection } from './legal-issues-section';
import { MilitarySection } from './military-section';
import { PostSecondaryEducationSection } from './post-secondary-education-section';
import { WorkHistorySection } from './work-history-section';
import { WorkProgramsSection } from './work-programs-section';
import { TransportationSection } from './transportation-section';
import { NonCustodialParentsSection } from './non-custodial-parents-section';
import { NonCustodialParentsReferralSection } from './non-custodial-parents-referral-section';
import { ParticipantBarriersSection } from './participant-barriers-section';
import { Utilities } from '../utilities';

import * as moment from 'moment';

export class InformalAssessment {
  // Section names constants
  public static LanguagesSectionName = 'languages';
  public static WorkHistorySectionName = 'work-history';
  public static WorkProgramsSectionName = 'work-programs';
  public static EducationHistorySectionName = 'education';
  public static PostSecondaryEducationSectionName = 'post-secondary';
  public static MilitaryTrainingSectionName = 'military-training';
  public static HousingSectionName = 'housing';
  public static TransportationSectionName = 'transportation';
  public static LegalIssuesSectionName = 'legal-issues';
  public static ParticipantBarriersSectionName = 'participant-barriers';
  public static ChildYouthSupportsSectionName = 'child-youth-supports';
  public static FamilyBarriersSectionName = 'family-barriers';
  public static NonCustodialParentsSectionName = 'non-custodial-parents';
  public static NonCustodialParentsReferralSectionName = 'non-custodial-parents-referral';

  id: number;
  type: string;
  submitDate: moment.Moment;

  languagesSection: LanguagesSection;
  educationSection: EducationHistorySection;
  postSecondarySection: PostSecondaryEducationSection;
  workProgramSection: WorkProgramsSection;
  militarySection: MilitarySection;
  childYouthSupportsSection: ChildAndYouthSupportsSection;
  participantBarriersSection: ParticipantBarriersSection;
  legalIssuesSection: LegalIssuesSection;
  familyBarriersSection: FamilyBarriersSection;
  housingSection: HousingSection;
  workHistorySection: WorkHistorySection;
  transportationSection: TransportationSection;
  nonCustodialParentsSection: NonCustodialParentsSection;
  nonCustodialParentsReferralSection: NonCustodialParentsReferralSection;

  private static clone(input: any, instance: InformalAssessment) {

    instance.id = input.id;
    instance.type = input.type;
    instance.submitDate = input.submitDate;
    instance.languagesSection = Utilities.deserilizeChild(input.languagesSection, LanguagesSection);
    instance.educationSection = Utilities.deserilizeChild(input.educationSection, EducationHistorySection);
    instance.postSecondarySection = Utilities.deserilizeChild(input.postSecondarySection, PostSecondaryEducationSection);
    instance.workProgramSection = Utilities.deserilizeChild(input.workProgramSection, WorkProgramsSection);
    instance.militarySection = Utilities.deserilizeChild(input.militarySection, MilitarySection);
    instance.childYouthSupportsSection = Utilities.deserilizeChild(input.childYouthSupportsSection, ChildAndYouthSupportsSection);
    instance.familyBarriersSection = Utilities.deserilizeChild(input.familyBarriersSection, FamilyBarriersSection);
    instance.housingSection = Utilities.deserilizeChild(input.housingSection, HousingSection);
    instance.workHistorySection = Utilities.deserilizeChild(input.workHistorySection, WorkHistorySection);
    instance.transportationSection = Utilities.deserilizeChild(input.transportationSection, TransportationSection);
    instance.nonCustodialParentsSection = Utilities.deserilizeChild(input.nonCustodialParentsSection, NonCustodialParentsSection);
    instance.nonCustodialParentsReferralSection = Utilities.deserilizeChild(input.nonCustodialParentsReferralSection, NonCustodialParentsReferralSection);
    instance.participantBarriersSection = Utilities.deserilizeChild(input.participantBarriersSection, ParticipantBarriersSection);
    instance.legalIssuesSection = Utilities.deserilizeChild(input.legalIssuesSection, LegalIssuesSection);
  }

  public deserialize(input: any) {
    InformalAssessment.clone(input, this);
    return this;
  }

}
