import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { HtmlContentModule, HtmlContentModuleMeta } from '../../../models/content/html/html-content-module';
@Component({
  selector: 'app-text-content-module',
  templateUrl: './text-content-module.component.html',
  styleUrls: ['./text-content-module.component.css']
})
export class TextContentModuleComponent implements OnInit {
  date = '';
  @Input() editMode = true;
  @Input() model: HtmlContentModule;
  @Output() change = new EventEmitter<HtmlContentModuleMeta[]>();
  _metaName = 'html';
  htmlMeta: HtmlContentModuleMeta;
  get data() {
    return this.htmlMeta.data;
  }
  set data(val) {
    this.htmlMeta.data = val;
  }
  constructor() {}
  onChange(event) {
    this.change.emit(Array.from(this.model.metas.values()));
  }
  ngOnInit() {
    this.htmlMeta = this.model.metas.get(this._metaName) || new HtmlContentModuleMeta(this._metaName, '');
    this.model.setMeta(this._metaName, this.htmlMeta);
  }
}
