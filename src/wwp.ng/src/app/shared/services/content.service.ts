import { PageContract } from './contracts/content/content-service.contract';
import { HtmlContentModule, HtmlContentModuleMeta } from '../models/content/html/html-content-module';
import { Observable } from 'rxjs';
import { ContentModule } from '../models/content/content-module';
import { ContentPage } from '../models/content/content-page';
import { ContentModuleMeta } from '../models/content/content-module-meta';
import { LogService } from './log.service';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { BaseService } from './../../core/services/base.service';
import { Injectable, Type } from '@angular/core';
import { Response } from '@angular/http';
import { Utilities } from '../utilities';
import { map, catchError } from 'rxjs/operators';
@Injectable()
export class ContentService extends BaseService {
  public contentUrl: string;
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.contentUrl = this.appService.apiServer + 'api/content/';
  }

  public getPageAndContent(slug: string) {
    return this.http.get(this.contentUrl + 'page/' + encodeURI(Utilities.trimStart('/', slug))).pipe(map(response => this.extractPageData(response)));
  }

  public saveContentPage(model: ContentPage): Observable<ContentPage> {
    const data: PageContract = model.serialize();
    return this.http.post(this.contentUrl + 'page', JSON.stringify(data)).pipe(
      map(response => this.extractPageData(response)),
      catchError(this.handleError)
    );
  }

  private extractPageData(res: any) {
    const body = res as any;
    let contentPage: ContentPage = null;
    if (body) {
      const contract = <PageContract>body;

      contentPage = ContentPage.deserialize(contract);

      for (const moduleContract of contract.modules) {
        const moduleType = this.getModuleType(moduleContract);
        const moduleModel = ContentModule.deserialize(moduleContract, moduleType);

        for (const metaContract of moduleContract.moduleMetas) {
          const metaType = this.getModuleMetaType(metaContract);
          const meta = ContentModuleMeta.deserialize(metaContract, metaType);
          moduleModel.setMeta(meta.name, meta);
        }

        contentPage.contentModules.push(moduleModel);
      }
    }
    return contentPage;
  }

  private getModuleType(contract): Type<ContentModule<any>> {
    switch (contract.name) {
      case 'html': {
        return HtmlContentModule;
      }
      default: {
        throw new Error(`Unable to dezerilize contract:${contract.name} with id:${contract.name}`);
      }
    }
  }

  private getModuleMetaType(contract): Type<ContentModuleMeta<any>> {
    switch (contract.name) {
      case 'html': {
        return HtmlContentModuleMeta;
      }
      default: {
        throw new Error(`Unable to dezerilize contract:${contract.name} with id:${contract.name}`);
      }
    }
  }
}
