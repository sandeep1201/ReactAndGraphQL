import { AppService } from 'src/app/core/services/app.service';
import { TableauService } from './../../shared/services/tableau.service';
import { Component, OnInit, ViewChild, ElementRef, Input } from '@angular/core';
import { RfaService } from '../../shared/services/rfa.service';
import { RFAProgram } from '../../shared/models/rfa.model';
import { Utilities } from '../../shared/utilities';
declare var tableau: any;

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css'],
  providers: [TableauService, RfaService]
})
export class ReportsComponent implements OnInit {
  public baseUrl: string;
  public basetableauUrl: string;
  public extraUrl: string;
  public tableauTicket: any;
  public tableauRes: any;
  public constructedTableauUrl: string;
  public placeHolder: ElementRef;
  viz: any;
  public mainFrameId: string;
  public kidsPin: number;
  public options: any;
  public programs: RFAProgram[];
  public isLoaded = false;
  @Input() page: string;
  @Input() pin: string;
  @ViewChild('vizContainerRef',{ static: true }) vizContainerRef: ElementRef;
  constructor(private tableauService: TableauService, private appService: AppService, private rfaService: RfaService) {
    this.basetableauUrl = this.appService.tableauUrl;
  }

  ngOnInit() {
    this.mainFrameId = this.appService.user.mainFrameId;
    if (this.page === 'kids' && this.pin) {
      this.rfaService.getSortedRfasByPin(this.pin).subscribe(res => {
        this.programs = res;
        const recentProg = this.programs.filter(i => i.isFCDPProgram);
        this.kidsPin = recentProg[0].kidsPin;
        this.initViz();
      });
    } else this.initViz();
  }

  initViz() {
    this.placeHolder = this.vizContainerRef.nativeElement;
    this.tableauService.getTicket().subscribe(res => {
      this.tableauTicket = res;
      this.baseUrl = `${this.basetableauUrl}trusted/${this.tableauTicket}/t/${this.appService.tableauSiteId}/views/`;
      this.report(this.page);
      this.constructedTableauUrl = `${this.baseUrl}${this.extraUrl}`;
      this.viz = new tableau.Viz(this.placeHolder, this.constructedTableauUrl, this.options);
      this.isLoaded = true;
    });
  }

  report(page: string) {
    switch (page) {
      case 'kids': {
        this.extraUrl = 'FCDPEmploymentHistory/FCDPEmploymentHistory?iframeSizedToWindow=true&:embed=y&:showAppBanner=false&:display_count=no&:showVizHome=no&:toolbar=no';
        this.options = {
          KIDSPIN: Utilities.pad(this.kidsPin, 10),
          hideTabs: true,
          hideToolbar: true
        };
        break;
      }
      default: {
        this.extraUrl = 'W2WEBISwitchboard/W2ReportList?iframeSizedToWindow=true&:embed=y&:showAppBanner=false&:display_count=no&:showVizHome=no&:toolbar=no';
        this.options = {
          FEP_ID: this.mainFrameId || 'XKE522'
        };
        break;
      }
    }
  }
}
