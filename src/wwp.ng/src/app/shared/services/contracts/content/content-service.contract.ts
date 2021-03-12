import {ModuleStatus} from '../../../models/content/content-module';
import {BaseModelContract} from '../../../models/model-base-contract';
export class PageContract extends BaseModelContract {
    title: string;
    description: string;
    sortOrder: number = 0;
    slug: string;
    modules: ModuleContract[] = [];
}

export class ModuleContract extends BaseModelContract {
    name: string;
    title: string;
    showTitle = false;
    description: string;
    showDescription = false;
    status: ModuleStatus;
    sortOrder: number = 0;
    contentPageId?: number;
    moduleMetas: MetaContract[] = [];
}

export class MetaContract extends BaseModelContract {
    name: string;
    data: any;
    contentModuleId?: number;
}