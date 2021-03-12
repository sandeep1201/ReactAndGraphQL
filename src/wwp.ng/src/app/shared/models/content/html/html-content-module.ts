import { TextContentModuleComponent } from '../../../components/content/text-content-module/text-content-module.component';
import { Type } from '@angular/core';
import { ContentModuleMeta } from '../content-module-meta';
import { ContentModule } from '../content-module';
export class HtmlContentModule extends ContentModule<string>{
    
    constructor(componentType: string) {
        super(componentType);
    }

    public setInputsAndOutputs(mapId: number) {
        this.inputs.model = this; // "Test content " + mapId; //TODO: Get From meta
        this.outputs.change = (metas: HtmlContentModuleMeta[]) => {
            this.updateModuleModelMetas(metas);
        }
    }

    public updateModuleModelMetas(metas: HtmlContentModuleMeta[]) {
        metas.map(meta => {
            if (this.metas.has(meta.name)) {
                let metaOriginal = this.metas.get(meta.name);
                this.setMetaBaseProps(metaOriginal, meta);
            }
            this.setMeta(meta.name, meta);
        });
    }

}

export class HtmlContentModuleMeta extends ContentModuleMeta<string> {
    constructor(name: string = null, data: string = null) {
        super(name, data);
    }
}