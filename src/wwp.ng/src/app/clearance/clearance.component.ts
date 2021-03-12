import { AppService } from './../core/services/app.service';
import { Component, OnInit, OnDestroy, ComponentRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { RegistrationComponent } from '../enrollment/registration/registration.component';
import { MciParticipantSearch } from '../shared/models/mciParticipantSearch.model';
import { DropDownField } from '../shared/models/dropdown-field';
import { MasterCustomerIdentifierService } from '../shared/services/master-customer-identifier.service';
import { FieldDataService } from '../shared/services/field-data.service';
import { MciParticipantReturn } from '../shared/models/mciParticipantReturn.model';
import { Status } from '../shared/models/status.model';
import { ModelErrors } from '../shared/interfaces/model-errors';
import { PersonName } from '../shared/models/person-name.model';
import { ValidationManager } from '../shared/models/validation-manager';
import { Utilities } from '../shared/utilities';
import { ParticipantGuard } from '../shared/guards/participant-guard';

@Component({
  selector: 'app-clearance',
  templateUrl: './clearance.component.html',
  styleUrls: ['./clearance.component.css'],
  providers: [MasterCustomerIdentifierService, FieldDataService]
})
export class ClearanceComponent implements OnInit, OnDestroy {
  public inEditView = false;
  public regId = 0;
  public model: MciParticipantSearch;
  public genderDrop: DropDownField[];
  public suffixTypesDrop: DropDownField[];
  public aliasTypesDrop: DropDownField[];
  public isSearchDisabled = true;
  public isSearching = false;
  public hasSearchLoaded = false;
  public hasErrored = false;
  public mciParticipants: MciParticipantReturn[] = [];
  public validationManager: ValidationManager = new ValidationManager(this.appService);

  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public isSectionValid: boolean;
  public selectedMciParticipant: MciParticipantReturn;
  public isErrored = false;
  private isSectionModified = false;
  public errorMessage = '';
  private mciParticipantsSub: Subscription;
  private _minScoreMatch = 96;
  public redirect;
  public eaRequestDetails: any;
  private inEAMode = false;

  constructor(
    private appService: AppService,
    private route: ActivatedRoute,
    private router: Router,
    private mciService: MasterCustomerIdentifierService,
    private fieldDataService: FieldDataService,
    private participantGuard: ParticipantGuard
  ) {
    if (this.router.getCurrentNavigation().extras && this.router.getCurrentNavigation().extras.state && this.router.getCurrentNavigation().extras.state.from === 'EA') {
      this.eaRequestDetails = this.router.getCurrentNavigation().extras.state;
      this.inEAMode = true;
    }
  }

  addReg(): void {
    this.regId = 0;
    this.inEditView = true;
  }
  exitReg(status: Status): void {
    this.inEditView = false;
    if (status != null && status.pinNumber != null && (status == null || status.errorMessages.length === 0)) {
      if (this.inEAMode)
        this.router.navigateByUrl(`/pin/${this.eaRequestDetails.pin}/emergency-assistance/ea-application-history/${this.eaRequestDetails.id}/edit/household-members`, {
          state: { id: this.eaRequestDetails.id, agency: this.eaRequestDetails.agency }
        });
      else
        this.participantGuard.showRequestElevatedAccess(status.pinNumber).subscribe(res => {
          if (res === true) {
            const pin = status.pinNumber.toString();
            this.goToPartSum(pin);
          } else {
            this.router.navigate(['/home']);
          }
        });
    } else {
      this.resetPage();
    }
  }

  // On save we want to go to particpant summary.
  goToPartSum(pin: string) {
    this.router.navigateByUrl(`/pin/${pin}`);
  }

  ngOnInit() {
    this.model = new MciParticipantSearch();
    this.model.name = new PersonName();

    this.loadSuffixTypes();
    this.loadAliasTypes();

    this.model.aliases = [];
    this.genderDrop = this.initGenderDrop();
    this.route.queryParams.subscribe(params => {
      if (params['redirect']) {
        this.redirect = params['redirect'];
      }
    });
  }
  private loadAliasTypes() {
    this.fieldDataService.getAliasTypes().subscribe(data => this.initAliasTypes(data));
  }
  private loadSuffixTypes() {
    this.fieldDataService.getSuffixTypes().subscribe(data => this.initSuffixTypes(data));
  }

  initGenderDrop(): DropDownField[] {
    const drop = new Array<DropDownField>();
    const m = new DropDownField();
    m.id = 'M';
    m.name = 'Male';
    drop.push(m);
    const f = new DropDownField();
    f.id = 'F';
    f.name = 'Female';
    drop.push(f);
    return drop;
  }

  search() {
    this.validate();
    if (this.isSectionValid) {
      this.isSearching = true;
      this.hasSearchLoaded = false;
      this.hasErrored = false;
      this.model = this.model.getCleansedSearchModel(this.model);
      this.mciParticipantsSub = this.mciService.getSearchResults(this.model).subscribe(
        data => this.initSearchResults(data),
        error => {
          this.isSearching = false;
          this.hasErrored = true;
        }
      );
    }
  }

  private initAliasTypes(data) {
    this.aliasTypesDrop = Utilities.createDropDownWithIdAsName(data);
  }

  private initSuffixTypes(data) {
    this.suffixTypesDrop = Utilities.createDropDownWithIdAsName(data);
  }

  initSearchResults(data) {
    this.isSearching = false;
    this.hasSearchLoaded = true;
    this.mciParticipants = [];
    this.mciParticipants = data;

    let addNewRecord = false;
    for (const p of this.mciParticipants) {
      if (p.score <= this._minScoreMatch) {
        addNewRecord = true;
      }
    }
    if (addNewRecord || (this.mciParticipants != null && this.mciParticipants.length === 0)) {
      this.mciParticipants.unshift(this.mapSearchItemAsReturn());
    }
  }

  private resetPage() {
    this.resetResults();
    this.model = new MciParticipantSearch();
    this.model.name = new PersonName();
    this.model.ssn = '';
    this.model.aliases = [];
  }

  private resetResults() {
    this.mciParticipants = [];
    this.hasSearchLoaded = false;
    this.hasErrored = false;
    this.selectedMciParticipant = null;
  }

  public checkState(): void {
    this.resetResults();

    this.isSectionModified = true;

    if (this.isSectionValid != null && this.isSectionValid !== true) {
      this.validate();
    }
  }

  validate() {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.isValidForSearch(this.validationManager, Utilities.currentDate.format('MM/DD/YYYY'));

    this.isSearchDisabled = !result.isValid;

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
  }

  selectMciParticipant(p) {
    if (this.selectedMciParticipant === p) {
      this.selectedMciParticipant = null;
    } else {
      this.selectedMciParticipant = p;
    }
  }

  ngOnDestroy() {
    if (this.mciParticipantsSub != null) {
      this.mciParticipantsSub.unsubscribe();
    }
  }

  private mapSearchItemAsReturn(): MciParticipantReturn {
    const r = new MciParticipantReturn();
    const pn = new PersonName();
    r.name = pn;
    r.id = 0;
    r.name.firstName = this.model.name.firstName;
    r.name.middleInitial = this.model.name.middleInitial;
    r.name.lastName = this.model.name.lastName;
    r.name.suffix = this.model.name.suffix;
    r.dateOfBirth = this.model.dateOfBirth;
    r.ssn = this.model.ssn;
    r.isNoSsn = this.model.isNoSsn;
    r.gender = this.model.gender;
    return r;
  }
}
