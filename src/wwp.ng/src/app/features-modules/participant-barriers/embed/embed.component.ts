import { Component, OnInit, Input, Output, OnChanges, EventEmitter } from '@angular/core';
import { ParticipantBarrier } from '../../../shared/models/participant-barriers-app';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-participant-barriers-embed',
  templateUrl: './embed.component.html',
  styleUrls: ['./embed.component.css']
})
export class ParticipantBarriersEmbedComponent implements OnInit, OnChanges {
  @Input() barrierType = '';
  @Input() pin = '';
  @Input() hideDeleted = false;
  @Input() isReadOnly = false;
  @Input() allParticipantBarriers: ParticipantBarrier[];
  @Output() participantBarriersChange = new EventEmitter();

  public participantBarriers: ParticipantBarrier[] = [];
  public inEditView = false;
  private recordId = 0;
  private barriersUrl = '';
  private barrierUrl = '';
  public inConfirmDeleteView = false;
  private deleteSelectedId: number;

  constructor(private participantBarrierAppService: ParticipantBarrierAppService) {}

  ngOnInit() {}

  ngOnChanges() {
    if (this.allParticipantBarriers != null) {
      this.participantBarriers = [];
      for (const pb of this.allParticipantBarriers) {
        if (pb.barrierTypeName === this.barrierType && pb.isOpen === true) {
          this.participantBarriers.push(pb);
        }
      }
    }
    if (this.hideDeleted) {
      // removing the deleted barriers on the IA summary.
      this.participantBarriers = _.remove(this.participantBarriers, x => x.isDeleted === false);
    }

    // Anytime we have a PIN, update the URL.
    if (this.pin != null) {
      this.barriersUrl = `/pin/${this.pin}/participant-barriers`;
    }
  }

  delete(id: number) {
    this.deleteSelectedId = id;
    this.inConfirmDeleteView = true;
  }

  onConfirmDelete() {
    this.participantBarrierAppService.deleteParticipantBarrier(this.deleteSelectedId).subscribe(complete => this.participantBarriersChange.emit());
    this.inConfirmDeleteView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  edit(id: number): void {
    this.inEditView = true;
    this.recordId = id;
  }

  add(): void {
    this.recordId = 0;
    this.inEditView = true;
  }

  open(id: number) {
    const newWindow = window.open(this.barriersUrl + '/' + id);
  }

  exit(): void {
    this.inEditView = false;
    this.participantBarriersChange.emit();
  }
}
