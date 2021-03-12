import { ParticipationTracking } from '../../../shared/models/participation-tracking.model';

export class NonParticipationGoodCause {
  public Id: number;
  public totalNonParticipationHours: string;
  public totalGoodCauseHours: string;
  public remainingNonSanctionableHours: string;
  public totalSanctionableHours: string;
  public participationTracking: ParticipationTracking[];
}
