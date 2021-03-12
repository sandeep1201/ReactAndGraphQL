import { Timeline } from './timeline';
import { Participant } from '../participant';
import { AssistanceGroupMemberContract } from '../../services/contracts/timelimits/service.contract';
export class AssistanceGroupMember {
  pin: number;
  isSelectable: boolean;
  relationship: string;
  isPlaced: boolean;
  timeline: Timeline;
  participant?: Participant;

  public clone() {
    let newMember = new AssistanceGroupMember();
    newMember.pin = this.pin;
    newMember.isSelectable = this.isSelectable;
    newMember.relationship = this.relationship;
    newMember.isPlaced = this.isPlaced;

    // TODO : clone timeline and particpant
    newMember.timeline = this.timeline ? this.timeline.clone() : null;
    newMember.participant = this.participant ? this.participant.clone() : null;
    return newMember;
  }

  public deserialize(contract: AssistanceGroupMemberContract) {
    this.pin = contract.pin;
    this.isSelectable = contract.isSelectable;
    this.relationship = contract.relationship;
    this.isPlaced = contract.isPlaced;

    this.timeline = new Timeline(null);
    this.participant = new Participant();
  }
}
