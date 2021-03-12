// /* tslint:disable:no-unused-variable */
// import { async, ComponentFixture, TestBed } from '@angular/core/testing';
// import { By } from '@angular/platform-browser';
// import { DebugElement } from '@angular/core';
// import { RouterLink } from '@angular/router';

// import { ContactsListComponent } from './list.component';
// import { ContactsService } from '../../shared/services/contacts.service';
// import { MockedContactsService } from '../../mocked/services/contacts.service';

// describe('ContactsListComponent', () => {
//     let component: ContactsListComponent;
//     let fixture: ComponentFixture<ContactsListComponent>;
//     let mockContactsServce: MockedContactsService;

//     beforeEach(async(() => {
//         mockContactsServce = new MockedContactsService();
//         TestBed.configureTestingModule({
//             declarations: [ContactsListComponent,
//                 RouterLink],
//                 providers: [
//                     {provide: ContactsService, useValue: mockContactsServce},
//                 ]
//         })
//             .compileComponents();
//     }));

//     beforeEach(() => {
//         fixture = TestBed.createComponent(ContactsListComponent);
//         component = fixture.componentInstance;
//         fixture.detectChanges();
//     });

//     it('should create', () => {
//         expect(component).toBeTruthy();
//     });
// });
