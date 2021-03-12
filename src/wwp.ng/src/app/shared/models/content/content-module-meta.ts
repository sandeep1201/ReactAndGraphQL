import { Type } from '@angular/core';
import { MetaContract } from '../../services/contracts/content/content-service.contract';
import { BaseModelContract, CommonDelCreatedModel } from '../model-base-contract';
// import { HtmlContentModuleMeta } from './html/html-content-module';
export abstract class ContentModuleMeta<T> extends CommonDelCreatedModel {
    contentModuleId: number;
    constructor(public name: string = null, public data: T = null) {
        super();
    }

    public static deserialize(contract: MetaContract, moduleMetaType: Type<ContentModuleMeta<any>>) {
        let contentModuleMeta: ContentModuleMeta<any> = new moduleMetaType();

        CommonDelCreatedModel.setCommonProperties(contentModuleMeta, contract);
        contentModuleMeta.name = contract.name;
        contentModuleMeta.data = contract.data; //TODO FIgure out if this is a string... shouldn't be, server should have formated it into a JSON object and Response.json() should have made it valid json
        contentModuleMeta.contentModuleId = contract.contentModuleId;
        return contentModuleMeta;
    }

    public serialize() {
        let contract = new MetaContract();
        BaseModelContract.setCommonProperties(contract, this);
        contract.name = this.name;
        contract.data = this.data;
        contract.contentModuleId = this.contentModuleId;

        return contract;
    }
}
