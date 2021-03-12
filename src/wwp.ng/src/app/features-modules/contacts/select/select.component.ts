import { Component, EventEmitter, Input, OnInit, OnDestroy, Output } from '@angular/core';
import { Subscription } from 'rxjs';

import { PaginationInstance } from 'ng2-pagination';
import { Contact } from '../models/contact';
import { ContactsService } from '../services/contacts.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-contacts-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css']
})
export class ContactsSelectComponent implements OnInit, OnDestroy {
  public config: PaginationInstance = {
    id: 'contacts',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() pin: string;
  @Output() contactSelect = new EventEmitter<number>();
  @Output() contactCancel = new EventEmitter();

  public isLoaded: boolean = false;
  public searchQuery: string = '';

  private allContacts: Contact[];
  private contactSub: Subscription;
  public contacts: Contact[];

  public hasSortedByAscName: boolean;
  public hasSortedByAscTitle: boolean;

  constructor(private contactService: ContactsService, private participantService: ParticipantService, private appService: AppService) {}

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
  }

  private destroyContactSubscription() {
    if (this.contactSub != null) {
      this.contactSub.unsubscribe();
    }
  }

  onCloseClick() {
    this.contactCancel.emit();
  }

  onSelect(contact: Contact) {
    this.contactSelect.emit(contact.id);
  }

  loadData() {
    this.destroyContactSubscription();
    this.participantService.getCachedParticipant(this.pin).subscribe(p => {
      this.contactSub = this.contactService.getContactsByPin(this.pin).subscribe(con => {
        const filteredcon = this.appService.filterTA(con, p);
        this.contacts = filteredcon;
        this.allContacts = filteredcon;
        this.applySortingOnData();
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

  applySortingOnData() {
    // We'll look at the class variables to determine how we should
    // be sorting.  Note we only sort one dimension at a time (title
    // or name).
    if (this.hasSortedByAscTitle != null) {
      if (this.hasSortedByAscTitle) {
        this.contacts.reverse();
        this.contacts.sort(function(a, b) {
          let nameA = a.title.toLowerCase();
          let nameB = b.title.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });
      } else {
        this.contacts.sort(function(a, b) {
          let nameA = a.title.toLowerCase();
          let nameB = b.title.toLowerCase();
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
          let nameA = a.name.toLowerCase();
          let nameB = b.name.toLowerCase();
          if (nameA < nameB) {
            return -1;
          } else if (nameA > nameB) {
            return 1;
          }
          return 0;
        });
      } else {
        this.contacts.sort(function(a, b) {
          let nameA = a.name.toLowerCase();
          let nameB = b.name.toLowerCase();
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

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  onSearch() {
    let query = this.searchQuery.trim();

    if (query !== '' && this.allContacts != null) {
      this.contacts = [];

      let filteredNameList = this.allContacts.filter(
        function(el: Contact) {
          return el.name.toLowerCase().indexOf(query.toLowerCase()) > -1;
        }.bind(this)
      );

      let filteredEmailList = this.allContacts.filter(
        function(el: Contact) {
          return el.phoneNumber.toString().indexOf(query.toLowerCase()) > -1;
        }.bind(this)
      );

      let filteredPhoneList = this.allContacts.filter(
        function(el: Contact) {
          return el.email.toLowerCase().indexOf(query.toLowerCase()) > -1;
        }.bind(this)
      );

      let filteredContactTitleList = this.allContacts.filter(
        function(el: Contact) {
          return el.title.toLowerCase().indexOf(query.toLowerCase()) > -1;
        }.bind(this)
      );

      for (let fnl of filteredNameList) {
        this.contacts.push(fnl);
      }

      for (let fpl of filteredPhoneList) {
        if (filteredNameList.indexOf(fpl) === -1) {
          this.contacts.push(fpl);
        }
      }

      for (let fel of filteredEmailList) {
        if (filteredNameList.indexOf(fel) === -1 && filteredPhoneList.indexOf(fel) === -1) {
          this.contacts.push(fel);
        }
      }

      for (let fctl of filteredContactTitleList) {
        if (filteredNameList.indexOf(fctl) === -1 && filteredPhoneList.indexOf(fctl) === -1 && filteredEmailList.indexOf(fctl) === -1) {
          this.contacts.push(fctl);
        }
      }
    } else {
      this.contacts = this.allContacts;
    }
  }
}
