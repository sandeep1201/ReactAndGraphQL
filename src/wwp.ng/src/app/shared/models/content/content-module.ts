// import { HtmlContentModule } from './html/html-content-module';
import { ModuleContract } from '../../services/contracts/content/content-service.contract';
// import { TextContentModuleComponent } from '../../components/content/text-content-module/text-content-module.component';
import { ContentModuleMeta } from './content-module-meta';
import { Type } from '@angular/core';
import { BaseModelContract, CommonDelCreatedModel } from '../model-base-contract';
export abstract class ContentModule<T> extends CommonDelCreatedModel {
    public name: string;

    public title: string;

    public showTitle: boolean;

    public description: string;

    public showDescription: boolean;

    public status: ModuleStatus;

    public sortOrder: number;
    public componentType: Type<any>;

    public metas: Map<string, ContentModuleMeta<any>> = new Map<string, ContentModuleMeta<any>>();
    public inputs: any = {};
    public outputs: any = {};
    public mapId: number = null;
    public contentPageId: number;
    constructor(componentType: string) {
        super();
        // this.componentType = ContentModule.componentTypeMap.get(componentType);
    }
    public setMetaBaseProps(source: ContentModuleMeta<any>, dest: ContentModuleMeta<any>) {

        dest.createdDate = source.createdDate;
        dest.data = source.data;
        dest.id = source.id;
        dest.isDeleted = source.isDeleted;
        dest.modifiedBy = source.modifiedBy;
        dest.modifiedDate = source.modifiedDate;
        dest.rowVersion = source.rowVersion;
    }
    public setMeta(name: string, moduleMeta: ContentModuleMeta<any>) {
        this.metas.set(moduleMeta.name, moduleMeta);
    }
    public abstract setInputsAndOutputs(mapId: number);

    public static deserialize(contract: ModuleContract, moduleType: Type<ContentModule<any>>): ContentModule<any> {
        let contentModule: ContentModule<any> = new moduleType();

        // switch (contract.name) {
        //     case 'html': {
        //         contentModule = new HtmlContentModule('TextContentModuleComponent');
        //         break;
        //     }
        //     default: {
        //         throw new Error(`Unable to dezerilize contract:${contract.name} with id:${contract.name}`)
        //     }
        // }

        CommonDelCreatedModel.setCommonProperties(contentModule, contract);
        contentModule.name = contract.name;
        contentModule.title = contract.title;
        contentModule.showTitle = contract.showTitle;
        contentModule.description = contract.description;
        contentModule.showDescription = contract.showDescription;
        contentModule.status = contract.status;
        contentModule.sortOrder = contract.sortOrder;
        contentModule.contentPageId = contract.contentPageId;
        return contentModule;
    }

    public serialize() {
        let contract = new ModuleContract();

        BaseModelContract.setCommonProperties(contract, this);
        contract.name = this.name;
        contract.title = this.title;
        contract.showTitle = this.showTitle;
        contract.description = this.description;
        contract.showDescription = this.showDescription;
        contract.status = this.status;
        contract.sortOrder = this.sortOrder;
        contract.contentPageId = this.contentPageId;

        return contract;
    }
}

export enum ModuleStatus {
    Draft,
    Published
}