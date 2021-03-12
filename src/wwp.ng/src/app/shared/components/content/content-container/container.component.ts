import { ContentService } from '../../../services/content.service';
import { HtmlContentModuleMeta, HtmlContentModule } from '../../../models/content/html/html-content-module';
import { ContentModule } from '../../../models/content/content-module';
import { ContentPage } from '../../../models/content/content-page';
import { Component, EventEmitter, OnInit, Output, Input } from '@angular/core';
import { Location } from '@angular/common';
import { TextContentModuleComponent } from '../text-content-module/text-content-module.component';
import { take, catchError, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-content-container',
  templateUrl: './container.component.html',
  styleUrls: ['./container.component.css']
})
export class ContentContainerModuleComponent implements OnInit {
  @Input() model: ContentPage;
  hadSaveError: boolean;
  isSaving: boolean;
  @Input() slug: string;
  @Output() saved = new EventEmitter<boolean>();
  public editMode = false;
  private _modules: ContentModule<any>[] = [];

  public get modules(): ContentModule<any>[] {
    const moduleArray = this._modules.filter(x => !x.isDeleted).sort((a, b) => a.sortOrder - b.sortOrder);
    return moduleArray;
  }
  moduleDefintion = { module: ContentModule, inputs: {}, output: {} };
  constructor(private contentService: ContentService, private location: Location) {}
  ngOnInit() {
    if (!this.slug) {
      this.slug = this.location.path(false);
    }

    if (this.model) {
      this.loadPageContentModules(this.model);
    } else {
      this.contentService
        .getPageAndContent(this.slug)
        .pipe(take(1))
        .subscribe(x => {
          this.model = x;
          this.loadPageContentModules(x);
        });
    }

    const modules: ContentModule<any>[] = [];
    modules.push(new HtmlContentModule('TextContentModuleComponent'));
    modules.push(new HtmlContentModule('TextContentModuleComponent'));
    modules.push(new HtmlContentModule('TextContentModuleComponent'));

    this._modules = modules.map((x, index) => {
      // TODO: Remove, this is test data
      x.metas.set('html', new HtmlContentModuleMeta('html', 'Test Content #' + index));

      //TODO: Do this dynamically
      x.componentType = TextContentModuleComponent;
      x.setInputsAndOutputs(index);
      x.inputs.editMode = false;
      return x;
    });
  }

  public loadPageContentModules(pageContent: ContentPage) {
    const modules = pageContent.contentModules;
    modules.map((y, index) => {
      y.setInputsAndOutputs(index);
      y.inputs.editMode = false;
    });
  }

  public edit() {
    this.editMode = true;
    this._modules.map((x, index) => {
      x.inputs.editMode = true;
    });
  }

  public cancel() {
    this.editMode = false;
    this._modules.map((x, index) => {
      x.inputs.editMode = false;
    });
  }

  public save() {
    // TODO: Save module Data
    this.isSaving = true;
    this.hadSaveError = false;
    this.contentService
      .saveContentPage(this.model)
      .pipe(
        take(1),
        catchError((error, caught) => {
          this.hadSaveError = true;
          return caught;
        }),
        finalize(() => {
          this.isSaving = false;
        })
      )
      .subscribe(x => {
        this.model = x;
        this.saved.emit(true);
        this.loadPageContentModules(x);
      });
  }
}
