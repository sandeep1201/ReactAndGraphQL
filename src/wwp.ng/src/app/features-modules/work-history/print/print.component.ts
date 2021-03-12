import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ReportService } from '../../../shared/services/report.service';
import { Employment } from '../../../shared/models/work-history-app';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  styleUrls: ['./print.component.scss']
})
export class PrintComponent implements OnInit {
  @Input() employments: Employment[];
  public tempEmps;
  public isLoaded = false;
  public pin: any;
  public pdfLoaded = false;
  public selectedEmpsForPrint: Employment[] = [];

  constructor(private router: Router, private route: ActivatedRoute, private reportService: ReportService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
    });
    if (this.employments) {
      this.tempEmps = this.employments.splice(0);
      this.isLoaded = true;
    }
  }
  cancel() {
    this.router.navigate([`pin/${this.pin}`], { skipLocationChange: false }).then(() => this.router.navigateByUrl(`pin/${this.pin}/work-history`));
  }

  getEmployments(emp: Employment, checked: boolean) {
    const index = this.selectedEmpsForPrint.indexOf(emp);
    if (checked === true && index <= -1) {
      this.selectedEmpsForPrint.push(emp);
    }

    if (checked === false && index > -1) {
      this.selectedEmpsForPrint.splice(index, 1);
    }
  }

  generatePdf() {
    this.pdfLoaded = true;
    this.reportService.getEmploymentReport(this.selectedEmpsForPrint, this.pin).subscribe(
      data => {
        const blob: any = new Blob([data], { type: 'application/pdf' });
        if (blob) {
          //IE doesn't allow using a blob object directly as link href instead it is necessary to use msSaveOrOpenBlob
          if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, `WorkHistory_ ${this.pin}.pdf`);
            this.pdfLoaded = false;
            return;
          }

          // For other browsers:
          // Create a link pointing to the ObjectURL containing the blob.
          const blobUrl = URL.createObjectURL(blob);
          window.open(blobUrl, `WorkHistory_ ${this.pin}.pdf`);
          setTimeout(() => {
            //Revoke the blob url after the tab is opened
            window.URL.revokeObjectURL(blobUrl);
          }, 1000);
          this.pdfLoaded = false;
        }
      },
      () => {
        this.pdfLoaded = false;
      }
    );
  }
}
