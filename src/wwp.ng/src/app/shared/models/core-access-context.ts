import { EnrolledProgram } from './enrolled-program.model';
import { AccessType } from '../enums/access-type.enum';
export class CoreAccessContext {
  programs: EnrolledProgram[] = [];
  canAccess: boolean;
  isStateStaff: boolean;
  isMostRecentProgramInOrg: boolean;
  canEdit: boolean;
  isMostRecentProgramInSisterOrg: boolean;
  isUserInAssociatedAgency = false;
  isParticipantSummary = false;
  canAccessPOPClaims_View = false;

  public toString(): string {
    return (
      'canAccess: ' +
      this.canAccess +
      ' isStateStaff: ' +
      this.isStateStaff +
      ' isMostRecentProgramInOrg: ' +
      this.isMostRecentProgramInOrg +
      ' canEdit: ' +
      this.canEdit +
      ' isMostRecentPrograminSisterOrg: ' +
      this.isMostRecentProgramInSisterOrg +
      ' AccessType: ' +
      AccessType[this.evaluate()]
    );
  }

  public evaluate(): AccessType {
    if (this.canAccess) {
      if (this.isStateStaff) {
        if (!this.canEdit) {
          // 111
          return AccessType.view;
        } else {
          // 110
          return AccessType.edit;
        }
      } else {
        if (this.isMostRecentProgramInOrg) {
          if (!this.canEdit) {
            // 1011
            return AccessType.view;
          } else {
            // 1010
            return AccessType.edit;
          }
        } else if (this.isMostRecentProgramInSisterOrg) {
          // 1001
          return AccessType.view;
        } else if (this.isUserInAssociatedAgency) {
          return AccessType.view;
        } else if (this.isParticipantSummary) {
          return AccessType.view;
        } else if (this.canAccessPOPClaims_View) {
          return AccessType.view;
        } else {
          return AccessType.none;
        }
      }
    } else {
      // 0
      return AccessType.none;
    }
  }
}
