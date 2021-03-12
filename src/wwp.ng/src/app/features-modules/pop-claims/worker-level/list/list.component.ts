import { AppService } from './../../../../core/services/app.service';
import { PopClaimsService } from './../../services/pop-claims.service';
import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { Participant } from 'src/app/shared/models/participant';
import { POPClaim } from '../../models/pop-claim.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-worker-level-pop-claims-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class WorkerLevelPopClaimsListComponent implements OnInit {
  public isLoaded = false;
  @Input() pin;
  @Input() popClaims: POPClaim[];
  public goBackUrl: string;
  public inEditMode = false;
  @Output() addPOP = new EventEmitter();
  @Input() participant: Participant;
  @Input() POPClaimTypes: DropDownField[];
  public inSingleEntryView = false;
  public clickedPOPClaim: POPClaim;
  public isWorker = true;

  constructor(private popClaimsService: PopClaimsService, private appService: AppService) {}

  ngOnInit() {
    this.goBackUrl = '/pin/' + this.pin;
    this.isLoaded = true;
    this.isWorker = this.appService.isPOPClaimWorker;
  }
  onAddPOP() {
    this.addPOP.emit();
  }

  refreshListView() {
    this.isLoaded = false;
    this.popClaimsService.getPopClaims(this.participant ? this.participant.id : null).subscribe(res => {
      this.popClaims = res;
      this.inSingleEntryView = false;
      this.isLoaded = true;
    });
  }

  onSingleItemClick(item: POPClaim) {
    this.inSingleEntryView = true;
    this.clickedPOPClaim = item;
  }
  exitSingleEntryView() {
    this.refreshListView();
  }
}
