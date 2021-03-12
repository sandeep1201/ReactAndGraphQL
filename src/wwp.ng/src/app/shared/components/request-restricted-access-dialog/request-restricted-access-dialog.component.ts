import { Component, OnInit } from '@angular/core';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';
import { FieldDataService } from '../../services/field-data.service';
import { DropDownField } from '../../models/dropdown-field';
import { ElevatedAccessRequestModel } from '../../interfaces/elevatedAccessReason';
import { AppService } from 'src/app/core/services/app.service';
import { ModelErrors } from '../../interfaces/model-errors';
import { ParticipantService } from '../../services/participant.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-request-restricted-access-dialog',
  templateUrl: './request-restricted-access-dialog.component.html',
  styleUrls: ['./request-restricted-access-dialog.component.css'],
  providers: [FieldDataService]
})
@Modal()
export class RequestRestrictedAccessDialogComponent implements OnInit {
  public elevatedAccessReasonDrop: DropDownField[];
  public elevatedAccessReasonId: number;
  public elevatedAccessReasonDetails: string;
  public isValid = false;
  public model = new ElevatedAccessRequestModel();
  public modelErrors: ModelErrors = {};
  public actionSubmitted = new Subject<{ isRequestingAccess: boolean; model?: ElevatedAccessRequestModel }>();
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor(private appService: AppService, private fdService: FieldDataService, private participantService: ParticipantService) {}

  ngOnInit() {
    this.fdService.getElevatedAccessReason().subscribe(data => {
      this.initElevatedAccessReasonType(data);
    });
  }

  public validate() {
    if (this.elevatedAccessReasonId) {
      this.isValid = true;
      this.modelErrors.elevatedAccessReasonId = false;
    } else {
      this.isValid = false;
      this.modelErrors.elevatedAccessReasonId = true;
    }
  }
  public setModel() {
    if (this.isValid) {
      this.model.elevatedAccessReasonId = this.elevatedAccessReasonId;
      this.model.details = this.elevatedAccessReasonDetails || '';
      this.participantService.setAction({ isRequestingAccess: true, model: this.model });
      this.closeDialog();
      this.destroy();
    } else {
      this.modelErrors.elevatedAccessReasonId = true;
    }
  }
  public onCancel() {
    this.participantService.setAction({ isRequestingAccess: false });
    this.closeDialog();
    this.destroy();
  }

  private initElevatedAccessReasonType(data: DropDownField[]) {
    this.elevatedAccessReasonDrop = data;
  }
}
