import { Location } from '@angular/common';
import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Params, Router, RouterModule, Routes } from '@angular/router';

declare var $: any;

@Component({
  selector: 'app-help-nav',
  templateUrl: './help-nav.component.html',
  styleUrls: ['./help-nav.component.css']
})

export class HelpNavComponent implements OnInit {

  constructor(private _location: Location, private route: ActivatedRoute, private router: Router) { }

  @Output() menuClicked = new EventEmitter<string>();
  
  public menuItemClicked(doc: string, event: MouseEvent) {
    console.log("Event Fired!");
    console.dir(event);
    if (doc) {
      $('li.menu-item').removeClass('select');
      $(this).addClass('select');

      let snapshot = this.route.snapshot;
      let queryParams = snapshot.queryParams;
      let newUrl = '/help?pg=' + doc;
      this._location.go(newUrl);
      this.menuClicked.emit(doc);

      // const u = window.location.pathname,
      //   p = $.param({ 'pg': doc }),
      //   np = u + '?' + p,
      //   dp = '/help/help-pages/' + doc + '.html';
      // window.history.pushState({ urlPath: u }, '', np);
      // const popStateEvent = new PopStateEvent('popstate', { state: dp });
      // dispatchEvent(popStateEvent);
    }
  }

  ngOnInit() {

    function gup(name, url) {
      if (!url) { url = location.href }
      name = name.replace(/[\[]/, '\\\[').replace(/[\]]/, '\\\]');
      const regexS = '[\\?&]' + name + '=([^&#]*)',
        regex = new RegExp(regexS),
        results = regex.exec(url);
      return results == null ? null : results[1];
    }

    const path = window.location,
      page = gup('pg', path);

    $('li.menu-item').on('click', function () {
      const tar = $(this).data('target')
      if (tar) {
        $('#topmenu').removeClass('open').fadeOut(function () {
          $('.tomain').removeClass('hidden').fadeIn();
          $('#' + tar).fadeIn().addClass('open');
        });
      }

    });

    $('.bc').on('click', function () {
      if ($('.tomain').is(':visible')) {
        $('ul.open').removeClass('open').fadeOut(function () {
          $('.tomain').fadeOut(function () { $(this).addClass('hidden') });
          $('#topmenu').fadeIn().addClass('open');
        });
      }
    });

    if (page) {
      const menu = $('li[data-doc=' + page + ']').closest('ul.submenu');
      if (menu) {
        $('span.tomain').removeClass('hidden');
        const menusibs = menu.siblings('ul');
        menusibs.removeClass('open').hide();
        menu.addClass('open').fadeIn();
      }
      //$('li[data-doc=' + page + ']').trigger('click');
      this.menuClicked.emit(page);
    }

  }

}
