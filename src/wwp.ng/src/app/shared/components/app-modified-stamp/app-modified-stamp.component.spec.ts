import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { AppModifiedStampComponent } from './app-modified-stamp.component';

describe('ModifiedStampComponent', () => {
    let comp: AppModifiedStampComponent;
    let fixture: ComponentFixture<AppModifiedStampComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ AppModifiedStampComponent ],
            schemas: [ NO_ERRORS_SCHEMA ]
        });
        fixture = TestBed.createComponent(AppModifiedStampComponent);
        comp = fixture.componentInstance;
    });

    it('can load instance', () => {
        expect(comp).toBeTruthy();
    });

    it('readOnly defaults to: false', () => {
        expect(comp.readOnly).toEqual(false);
    });

    it('showPencil defaults to: true', () => {
        expect(comp.showPencil).toEqual(true);
    });

});
