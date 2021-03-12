import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';

import * as TinyMce from 'tinymce';

import { TinyMceComponent } from './tinymce.component';
import { TinyMCEinlineDirective } from './tinymce.inline.directive';
import { TINYMCE_SETTINGS_TOKEN } from './tinymce.defaultSettings';
/**
 * Module for TincyMCE
 */
@NgModule({
  imports: [CommonModule],
  declarations: [TinyMceComponent, TinyMCEinlineDirective],
  exports: [TinyMceComponent, TinyMCEinlineDirective]
})
export class TinyMceModule {
static forRoot(settings: TinyMce.Settings): ModuleWithProviders {
  return {
    ngModule: TinyMceModule,
    providers: [
      { provide: TINYMCE_SETTINGS_TOKEN, useValue: settings }
    ]
  };
  }
}