import { Component, OnInit, Input } from '@angular/core';
import { HelpService } from '../../services/help.service';

@Component({
  selector: 'app-help-button',
  templateUrl: './help-button.component.html',
  styleUrls: ['./help-button.component.css'],
  providers: [HelpService]
})
export class HelpButtonComponent implements OnInit {
  @Input() featureName = '';

  constructor(public helpService: HelpService) {}

  ngOnInit() {}

  public helpClick() {
    this.newWindowAtFeatureUrl();
  }

  private newWindowAtFeatureUrl() {
    this.helpService.getHelpUrl(this.featureName).subscribe(x => {
      this.redirectToUrlInNewWindow(x[0]);
    });
  }

  private redirectToUrlInNewWindow(url: string) {
    window.open(url, '_blank');
  }
}
