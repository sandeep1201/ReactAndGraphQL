import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ModifiedStampComponent } from './modified-stamp.component';

describe('ModifiedStampComponent', () => {
    let comp: ModifiedStampComponent;
    let fixture: ComponentFixture<ModifiedStampComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ ModifiedStampComponent ],
            schemas: [ NO_ERRORS_SCHEMA ]
        });
        fixture = TestBed.createComponent(ModifiedStampComponent);
        comp = fixture.componentInstance;
    });

    it('can load instance', () => {
        expect(comp).toBeTruthy();
    });

    it('readOnly defaults to: false', () => {
        expect(comp.readOnly).toEqual(false);
    });

    it('label defaults to: Last Edited by', () => {
        expect(comp.label).toEqual('Last Edited by');
    });

    it('showPencil defaults to: true', () => {
        expect(comp.showPencil).toEqual(true);
    });

    it('showTrash defaults to: false', () => {
        expect(comp.showTrash).toEqual(false);
    });

});
