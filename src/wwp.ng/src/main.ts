import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

declare let feature: any;
declare const $: any;

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic()
  .bootstrapModule(AppModule, { preserveWhitespaces: true })
  .catch(err => console.error(err))
  .then(x => {
    const isPhone = /Android|webOS|iPhone|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    // Add feature check here.
    if (isPhone) {
      $('body')
        .addClass('fail')
        .removeClass('loading');
    } else {
      $('body').removeClass('loading');
    }
  });
