import { Component, Input, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

import * as _ from 'lodash';
import * as Fuse from 'fuse.js';
import { Subscription } from 'rxjs';

import { ContactsEditComponent } from '../edit/edit.component';
import { PaginationInstance } from 'ng2-pagination';
import { Contact } from '../models/contact';
import { ContactsService } from '../services/contacts.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-contacts-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ContactsListComponent implements OnInit, OnDestroy {
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() pin: string;
  @Input() isReadOnly = true;
  @ViewChild(ContactsEditComponent, { static: true }) editComponent: ContactsEditComponent;

  public isLoaded = false;
  public searchQuery = '';

  public allContacts: Contact[];
  private contactSub: Subscription;
  private contactDeleteSub: Subscription;
  public contacts: Contact[];
  public editContactId: number;

  public hasSortedByAscName: boolean;
  public hasSortedByAscTitle: boolean;
  public inEditView = false;
  public inConfirmDeleteView = false;

  constructor(private contactService: ContactsService, private router: Router, private participantService: ParticipantService, private appService: AppService) {}

  ngOnInit() {
    if (this.pin == null || this.pin === '') {
      console.warn('PIN on ContactsListComponent is null or empty');
    }

    // Per US965, the list view will default to sorted by Contact Title.
    this.hasSortedByAscTitle = true;

    this.loadData();
  }

  ngOnDestroy() {
    this.destroyContactSubscription();
    this.destroyContactDeleteSubscription();
  }

  private destroyContactSubscription() {
    if (this.contactSub != null) {
      this.contactSub.unsubscribe();
    }
  }

  private destroyContactDeleteSubscription() {
    if (this.contactDeleteSub != null) {
      this.contactDeleteSub.unsubscribe();
    }
  }

  onAdd() {
    // In order to set the edit contact component to Add mode, we
    // just need to clear out the contact ID.
    this.editContactId = null;
    this.inEditView = true;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  onConfirmDelete() {
    this.inConfirmDeleteView = false;

    this.destroyContactDeleteSubscription();

    this.contactDeleteSub = this.contactService.deleteContactById(this.editContactId, this.pin).subscribe(resp => {
      // We'll just reload the data.
      this.loadData();
    });
  }

  onDelete(contact: Contact) {
    this.editContactId = contact.id;
    this.inConfirmDeleteView = true;
  }

  onEdit(contact: Contact) {
    this.editContactId = contact.id;
    this.inEditView = true;
  }

  onContactSave(contactId: number) {
    this.inEditView = false;

    this.loadData();
  }

  onEditContactCancel() {
    this.inEditView = false;
  }

  getContactDetailUrl(contact): string {
    if (contact == null) {
      return '';
    }

    return `/pin/${this.pin}/contacts/${contact.id}`;
  }

  goToContactDetail(contact: Contact) {
    this.router.navigateByUrl(this.getContactDetailUrl(contact));
  }

  loadData() {
    this.destroyContactSubscription();
    this.participantService.getCachedParticipant(this.pin).subscribe(p => {
      this.contactSub = this.contactService.getContactsByPin(this.pin).subscribe(con => {
        const filteredcon = this.appService.filterTA(con, p);
        this.contacts = filteredcon;
        this.allContacts = filteredcon;
        this.applySortingOnData();
        this.onSearch();
      });
    });
  }

  sortByName() {
    // If the title was being sorted we need to reset it.
    this.hasSortedByAscTitle = null;

    if (this.hasSortedByAscName == null) {
      this.hasSortedByAscName = true;
    } else {
      this.hasSortedByAscName = !this.hasSortedByAscName;
    }

    this.applySortingOnData();
  }

  sortByTitle() {
    // If the name was being sorted we need to reset it.
    this.hasSortedByAscName = null;

    if (this.hasSortedByAscTitle == null) {
      this.hasSortedByAscTitle = true;
    } else {
      this.hasSortedByAscTitle = !this.hasSortedByAscTitle;
    }

    this.applySortingOnData();
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }
  applySortingOnData() {
    // We'll look at the class variables to determine how we should
    // be sorting.  Note we only sort one dimension at a time (title
    // or name).
    if (this.hasSortedByAscTitle != null) {
      if (this.hasSortedByAscTitle) {
        this.contacts.reverse();
        this.contacts.sort(function(a, b) {
          const nameA = a.title.toLowerCase();
          const nameB = b.title.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });
      } else {
        this.contacts.sort(function(a, b) {
          const nameA = a.title.toLowerCase();
          const nameB = b.title.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });

        this.contacts.reverse();
      }
    } else if (this.hasSortedByAscName != null) {
      if (this.hasSortedByAscName) {
        this.contacts.sort(function(a, b) {
          const nameA = a.name.toLowerCase();
          const nameB = b.name.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });
      } else {
        this.contacts.sort(function(a, b) {
          const nameA = a.name.toLowerCase();
          const nameB = b.name.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });

        this.contacts.reverse();
      }
    }
  }

  onSearch() {
    // Search via text input goes here.
    if (this.searchQuery != null && this.searchQuery.trim() !== '' && this.allContacts != null) {
      const query = this.searchQuery.trim().toLowerCase();

      const options = {
        shouldSort: true,
        findAllMatches: true,
        threshold: 0.1,
        location: 0,
        distance: 100,
        maxPatternLength: 140,
        minMatchCharLength: 1,
        keys: ['name', 'email', 'phoneNumber', 'title']
      };
      const fuse = new Fuse(this.allContacts, options);
      const searchResults = fuse.search<Contact>(query);
      this.contacts = [];

      // We have to find the deserialized object using the results of the search.
      for (const searchResult of searchResults) {
        // const c = _.find(this.allContacts, searchResult);
        const c = this.allContacts.find(x => x.id === searchResult.id);
        this.contacts.push(c);
      }
    } else {
      this.contacts = this.allContacts;
    }
  }
}
