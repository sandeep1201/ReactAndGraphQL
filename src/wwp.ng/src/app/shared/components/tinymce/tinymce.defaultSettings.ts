import { InjectionToken } from '@angular/core';
import * as TinyMce from 'tinymce';

export const TINYMCE_SETTINGS_TOKEN = new InjectionToken('angular-tinymce-settings');
export const tinymceDefaultSettings: TinyMce.Settings | any =  {
    tinymceScriptURL: '/assets/tinymce/tinymce.min.js',
    theme_url: '/assets/tinymce/themes/modern/theme.min.js',
    skin_url: '/assets/tinymce/skins/lightgray',
    inline: true
}