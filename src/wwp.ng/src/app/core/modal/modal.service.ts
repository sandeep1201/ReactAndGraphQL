import { componentFactoryName } from '@angular/compiler';
import {
    Compiler,
    Component,
    ComponentFactoryResolver,
    ComponentRef,
    Injectable,
    Injector,
    NgModule,
    OnInit,
    ReflectiveInjector,
    Type,
    ViewChild,
    ViewContainerRef
} from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';


@Injectable()
export class ModalService {
    private vcRef: ViewContainerRef;
    private injector: Injector;
    public activeInstances: number = 0;
    //public activeLoginInstances: number = 0;

    public allowAction: boolean;
    constructor(private compiler: Compiler, private componentFactoryResolver: ComponentFactoryResolver) {
    }

    registerViewContainerRef(vcRef: ViewContainerRef): void {
        this.vcRef = vcRef;
    }

    registerInjector(injector: Injector): void {
        this.injector = injector;
    }

    // TODO: component: typeof DestroyableComponent was changed to any because passing two parameters inside of a modal component like test scores edit kept breaking.
    create<T>(component: Type<T>, parameters?: Object): Observable<ComponentRef<T>> {

        const componentRef$ = new ReplaySubject();
        let componentFactory = this.componentFactoryResolver.resolveComponentFactory<T>(component);

        const componentRef = this.vcRef.createComponent(componentFactory);
        Object.assign(componentRef.instance, parameters); // pass the @Input parameters to the instance
        this.activeInstances++;
        componentRef.instance['componentIndex'] = this.activeInstances;
        componentRef.instance['destroy'] = () => {
            this.activeInstances--;
            componentRef.destroy();
        };
        componentRef$.next(componentRef);
        componentRef$.complete();
        return <Observable<ComponentRef<T>>>componentRef$.asObservable();

    }
}