import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
// import { Response } from '@angular/http';
import { HelpNavComponent } from './help-nav/help-nav.component';
import { Location } from '@angular/common';

// import { Participant } from '../shared/models/participant';
// import { ParticipantService } from '../shared/services/participant.service';
// import { Utilities } from '../shared/utilities';

declare const $: any;

@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.css']
})
export class PageHelpComponent implements OnInit {

  isSidebarCollapsed: boolean = false;
  currDoc: string = '';
  get shouldLoadHelpComponent(): boolean {
    return this.shouldLoadOverviewHelpComponent || this.shouldLoadExtensionsHelpComponent;
  }
  shouldLoadOverviewHelpComponent: boolean = false;
  shouldLoadExtensionsHelpComponent: boolean = false;

  constructor(private _changeDetectorRef: ChangeDetectorRef, private _location: Location) {

    $(window).on('scroll', function () {
      const o = $('aside');
      if (1.2 * $('aside').outerHeight() < $(window).height()) {
        const e = $(window).scrollTop(),
          s = e - $('app-header').outerHeight() - $('app-sub-header').outerHeight() + 40;
        s > 0 ? o.css('top', s + 'px') : o.css('top', '0');
      } else { o.css('top', '0') };
    });

  }

  public navigateToItem(doc: string) {
    if (doc === 'tl-overview') {
      this.shouldLoadOverviewHelpComponent = true;
      this.shouldLoadExtensionsHelpComponent = false;
    } else if (doc === 'tl-ext-decisions') {
      this.shouldLoadOverviewHelpComponent = false;
      this.shouldLoadExtensionsHelpComponent = true;
    } else {
      this.shouldLoadOverviewHelpComponent = false;
      this.shouldLoadExtensionsHelpComponent = false;
    }
  }

  ngOnInit() {

    const page = this;
    window.onpopstate = (event: PopStateEvent) => {
      if (event.state) {
        // document.getElementById('main').innerHTML = '<p>' + event.state + '</p>';
        page.currDoc = event.state;
        if (page.currDoc.toString() === '/help/help-pages/tl-overview.html') {
          page.shouldLoadOverviewHelpComponent = true;
          page.shouldLoadExtensionsHelpComponent = false;
        } else if (page.currDoc.toString() === '/help/help-pages/tl-ext-decisions.html') {
          page.shouldLoadOverviewHelpComponent = false;
          page.shouldLoadExtensionsHelpComponent = true;
        } else {
          page.shouldLoadOverviewHelpComponent = false;
          page.shouldLoadExtensionsHelpComponent = false;
        }
      }

      page._changeDetectorRef.detectChanges();

    }
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

}
