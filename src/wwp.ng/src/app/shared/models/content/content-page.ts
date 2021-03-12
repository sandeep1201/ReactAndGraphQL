import { PageContract } from '../../services/contracts/content/content-service.contract';
import { ContentModule } from './content-module';
import { TextContentModuleComponent } from '../../components/content/text-content-module/text-content-module.component';
import { Type } from '@angular/core';
import { BaseModelContract, CommonDelCreatedModel } from '../model-base-contract';

export class ContentPage extends CommonDelCreatedModel {
    title: string;
    description: string;

    sortOrder: number = 0;
    slug: string;

    contentModules: ContentModule<any>[] = [];
    constructor() {
        super();
    }

    public static deserialize(contract: PageContract) {
        const contentPage = new ContentPage();
        CommonDelCreatedModel.setCommonProperties(contentPage, contract);
        contentPage.title = contract.title;
        contentPage.sortOrder = contract.sortOrder;
        contentPage.description = contract.description;
        return contentPage;
    }

    public serialize(): PageContract {
        const contract = new PageContract();
        BaseModelContract.setCommonProperties(contract, this);
        contract.title = this.title;
        contract.sortOrder = this.sortOrder;
        contract.description = this.description;

        for (let moduleDef of this.contentModules) {
            let moduleContract = moduleDef.serialize();

            for(let moduleMeta of Array.from(moduleDef.metas.values())){
                let metaContract = moduleMeta.serialize();

                moduleContract.moduleMetas.push(metaContract);
            }

            contract.modules.push(moduleContract);
        }

        return contract;
    }
}


