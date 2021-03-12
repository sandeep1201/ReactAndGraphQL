import { Component, OnInit, Input } from '@angular/core';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Utilities } from '../../../shared/utilities';
import { Participant } from 'src/app/shared/models/participant';

@Component({
  selector: 'app-pep-card',
  templateUrl: './pep-card.component.html',
  styleUrls: ['./pep-card.component.css']
})
export class PepCardComponent implements OnInit {
  @Input() showFcdpFeature = false;
  @Input() enrolledProgram: EnrolledProgram;
  @Input() wpGeoArea: string;

  @Input() participant: Participant;
  public completionReasonDrop: DropDownField[];
  public completionReason: string;
  public otherCompletionReason: boolean;

  constructor(private fieldDataService: FieldDataService) {}

  ngOnInit() {
    if (this.enrolledProgram.isTj || this.enrolledProgram.isTmj || this.enrolledProgram.isCF || this.enrolledProgram.isFCDP) {
      this.loadCompletionReasons();
    }
  }
  private loadCompletionReasons() {
    this.fieldDataService.getCompletionReasons(this.enrolledProgram.programCd).subscribe(data => {
      this.completionReasonDrop = data;
      this.findCompletionReason();
    });
  }
  private findCompletionReason() {
    this.completionReason = Utilities.fieldDataNameById(this.enrolledProgram.completionReasonId, this.completionReasonDrop);
  }
}
