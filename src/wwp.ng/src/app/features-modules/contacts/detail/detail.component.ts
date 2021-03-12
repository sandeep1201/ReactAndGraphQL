import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { Contact } from '../models/contact';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { ContactsService } from '../services/contacts.service';

@Component({
  selector: 'app-contact-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css']
})
export class ContactDetailComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public contact: Contact;
  public goBackUrl: string;
  public isLoaded = false;

  private contactId: number;
  private contactSub: Subscription;
  public encodedContactAddress: string;
  private routeIdSub: Subscription;

  constructor(private detailRoute: ActivatedRoute, router: Router, partService: ParticipantService, private contactService: ContactsService) {
    super(detailRoute, router, partService);
  }

  ngOnInit() {
    super.onInit();

    this.routeIdSub = this.detailRoute.params.subscribe(params => {
      this.contactId = params['id'];

      if (this.pin == null || this.pin === '') {
        console.warn('PIN on ContactDetailComponent is null or empty');
      }

      this.contactSub = this.contactService.getContactById(this.contactId, this.pin).subscribe(con => {
        this.contact = con;

        if (this.contact.address != null && this.contact.address !== '') {
          this.encodedContactAddress = encodeURIComponent(this.contact.address);
        }
      });
    });
  }

  ngOnDestroy() {
    super.onDestroy();

    if (this.contactSub != null) {
      this.contactSub.unsubscribe();
    }

    if (this.routeIdSub != null) {
      this.routeIdSub.unsubscribe();
    }
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin + '/contacts';
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    this.isLoaded = true;
  }

  goBackToList() {
    this.router.navigateByUrl(`/pin/${this.pin}/contacts`);
  }
}
