import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { Contact } from '../../features-modules/contacts/models/contact';


@Injectable()
export class MockedContactsService {

    constructor() {
    }

    getContactById(id: number, pin: string): Observable<Contact> {
        let contact = new Contact();
        contact.id = id;
        contact.name = 'Mocked Contact';
        return of(contact);
    }

    getContactsByPin(pin: string): Observable<Contact[]> {
        let contacts = [];

        let contact = new Contact();
        contact.id = 1;
        contact.name = 'Mocked Contact1';

        contacts.push(contact);

        contact = new Contact();
        contact.id = 2;
        contact.name = 'Mocked Contact2';

        contacts.push(contact);

        return of(contacts);
    }


    deleteContactById(id: string, pin: string) {
    }
}
