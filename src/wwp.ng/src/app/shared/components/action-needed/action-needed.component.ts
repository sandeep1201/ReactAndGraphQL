import { Component, Input, forwardRef, Output, EventEmitter, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, NgControl } from '@angular/forms';

import { ActionNeeded } from '../../../features-modules/actions-needed/models/action-needed';
import { ActionNeededInfo } from '../../../features-modules/actions-needed/models/action-needed-info';
import { BaseComponent } from '../base-component';

@Component({
    selector: 'app-action-needed',
    templateUrl: './action-needed.component.html',
    styleUrls: ['./action-needed.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => ActionNeededComponent),
        multi: true
    }]
})

export class ActionNeededComponent extends BaseComponent implements ControlValueAccessor {

    private canCheckState: boolean = false;
    private isDetailsRequired: boolean = false;
    // public isDirty: boolean = false;

    public isMultiSelect: boolean = false;
    private listOfSelectedIds: number[] = [];

    @Input('type')
    set type(type: string) {
        if (type === 'multi') {
            this.isMultiSelect = true;
        }
    }

    @Input() title: string;
    @Input() actionNeededs: ActionNeeded[];
    @Input() isActionErrored: boolean;
    @Input() isDetailsErrored: boolean;
    @Input() isHalf: boolean = true;
    @Input() isRequired: boolean = false;
    @Output() onCheckState = new EventEmitter<boolean>();

    constructor() {
        super();
        this.innerValue = new ActionNeededInfo();
        this.innerValue.actionNeededTypes = [];
    }

    get value(): any {
        return this.innerValue;
    };

    set value(v: any) {
        this.innerValue = v;
        this.onChangeCallback(v);
    }

    onBlur() {
        this.onTouchedCallback();
    }

    writeValue(value: ActionNeededInfo) {
        if (value !== this.innerValue) {
            this.innerValue = value;
        }
        if (value != null && value.actionNeededTypes != null) {
            for (let typeId of value.actionNeededTypes) {
                if (typeId != null) {
                    this.manageSelected(typeId);
                    // this.isActionNeededNameLong();
                }
            }
            // Setting CheckState to true; so that further modifications by the user will trigger the checkstate.
            this.canCheckState = true;

        } else if (value == null) {
            this.innerValue = new ActionNeededInfo();
            this.innerValue.actionNeededTypes = [];
        }
    }

    registerOnChange(fn: any) {
        this.onChangeCallback = fn;
    }

    registerOnTouched(fn: any) {
        this.onTouchedCallback = fn;
    }

    manageSelected(id: number) {
        // Checkbox has not been set; lets add it on this click.
        if (this.listOfSelectedIds.indexOf(id) === -1) {
            this.listOfSelectedIds.push(id);
            for (let x of this.actionNeededs) {
                if (x.id === id) {
                    x.isSelected = true;
                }
                if (x.requiresDetails === true && x.id === id) {
                    this.isDetailsRequired = true;
                }
            }
            for (let x of this.actionNeededs) {
                if (x.disablesOthers === true && x.id === id) {
                    this.listOfSelectedIds = [];
                    this.listOfSelectedIds.push(id);
                    for (let an of this.actionNeededs) {
                        if (an.disablesOthers === false) {
                            an.isSelected = false;
                            an.isDisabled = true;
                            this.isDetailsRequired = false;
                        }
                    }
                }
            }
        } else {
            // Checkbox has been set; lets remove it on this click.
            let indexToRemove = this.listOfSelectedIds.indexOf(id);
            this.listOfSelectedIds.splice(indexToRemove, 1);
            for (let x of this.actionNeededs) {
                if (x.id === id) {
                    x.isSelected = false;
                }
                if (x.requiresDetails === true && x.id === id) {
                    this.isDetailsRequired = false;
                }
            }
            for (let x of this.actionNeededs) {
                if (x.disablesOthers === true && x.id === id) {
                    for (let an of this.actionNeededs) {
                        an.isDisabled = false;
                    }
                }
            }
        }

        this.value.actionNeededTypes = this.listOfSelectedIds;

        // Lets check if details is still required.
        for (let x of this.actionNeededs) {
            if (x.requiresDetails) {
                if (this.listOfSelectedIds.indexOf(x.id) > -1) {
                    this.isDetailsRequired = true;
                }
            }
        }

        this.checkState();
    }

    // isActionNeededNameLong() {
    //     if (this.actionNeededs != null) {
    //         for (let an of this.actionNeededs) {
    //             if (an.isNameLong()) {
    //                 this.isHalf = false;
    //             }
    //         }
    //     }
    // }

    checkState() {
        if (this.canCheckState) {
            this.onCheckState.emit(true);
            this.isRequired = false;
            // this.isDirty = true;
        }
    }
}
