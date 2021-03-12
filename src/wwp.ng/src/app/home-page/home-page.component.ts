// tslint:disable: import-blacklist
// tslint:disable: deprecation
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { trigger, style, animate, transition, state } from '@angular/animations';

import * as _ from 'lodash';
import * as Fuse from 'fuse.js';
import { Subscription, Observable, empty } from 'rxjs';
import { catchError, delay, take, concatMap } from 'rxjs/operators';

import { AgencyService } from '../shared/services/agency.service';
import { DropDownField } from '../shared/models/dropdown-field';
import { PaginationInstance } from 'ng2-pagination';
import { Participant } from '../shared/models/participant';
import { ParticipantService } from '../shared/services/participant.service';
import { Utilities } from '../shared/utilities';
import { Authorization } from '../shared/models/authorization';
import { UserPreferencesService } from '../shared/services/user-preferences.service';
import { Agency } from '../shared/models/office';
import { ProgramWorker } from '../shared/models/program-workers.model';
import { HttpErrorResponse } from '@angular/common/http';
import { AppService } from '../core/services/app.service';
import { Person } from '../shared/models/person.model';
import { ValidationManager } from '../shared/models/validation-manager';
import { ModelErrors } from '../shared/interfaces/model-errors';

@Component({
  selector: 'app-home-page',
  animations: [
    trigger('enterAnimation', [
      transition(':enter', [style({ transform: 'translateY(10%)', opacity: 0 }), animate('150ms', style({ transform: 'translateY(0)', opacity: 1 }))]),
      transition(':leave', [style({ transform: 'translateY(0)', opacity: 1 }), animate('150ms', style({ transform: 'translateY(100%)', opacity: 0 }))])
    ])
  ],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css'],
  providers: [AgencyService]
})
export class HomePageComponent implements OnInit, OnDestroy {
  Auth = Authorization;
  readonly recentTabName = 'recentTab';
  readonly tabByWorkerName = 'tabByWorkerName';
  readonly referralTabName = 'referralTab';
  readonly searchTabName = 'searchTab';

  public isLoading = false;
  public isSearching = false;
  public isErrored = false;
  public errorMessage: string;
  public workerDrop: DropDownField[] = [];
  public agenciesDrop: DropDownField[] = [];
  public agencyProgramsDrop: DropDownField[] = [];
  private allParticipants: Participant[];
  private programWorkers: ProgramWorker[];
  public genderDrop: DropDownField[];
  public isReferredParticipantsReversed = false;
  public isWorkerParticipantsReversed = false;
  private isFirstLoad = true;
  private rPartSub: Subscription;
  private mPartSub: Subscription;
  private refPartSub: Subscription;
  private sPartSub: Subscription;
  private routeSub: Subscription;
  private wSub: Subscription;
  private partDetailsSub: Subscription;
  private partSub: Subscription;
  public currentTab = 'recent';
  public searchModel = new Person();
  public clonedSearchModel = new Person();
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelError: ModelErrors = {};

  public configMy: PaginationInstance = {
    id: 'my',
    itemsPerPage: 10,
    currentPage: 1
  };

  public configReferred: PaginationInstance = {
    id: 'referred',
    itemsPerPage: 100,
    currentPage: 1
  };

  public configSearch: PaginationInstance = {
    id: 'search',
    itemsPerPage: 10,
    currentPage: 1
  };

  public recentParticipants: Participant[] = [];
  public workerParticipants: Participant[] = [];
  public allWorkerParticipants: Participant[] = [];
  public referredParticipants: Participant[] = [];
  public allReferredParticipants: Participant[] = [];
  public searchedParticipants: Participant[] = [];
  public advSearchParticipant: Participant;

  private _advSearchQuery = '';
  public get advSearchQuery() {
    return this._advSearchQuery;
  }
  public set advSearchQuery(val) {
    if (val) {
      this._advSearchQuery = val.trim();
      // this._advSearchQuery = val.replace(/[^0-9\\.]+/g, '');
    } else {
      this._advSearchQuery = '';
    }
  }

  public get canAdvSearch() {
    return (
      !this.isSearching &&
      ((this.isParticipantFieldsDisabled && (!this.advSearchParticipant || this.advSearchQuery !== this.advSearchParticipant.pin)) ||
        (this.isPinFieldDisabled &&
          !(
            Utilities.lowerCaseTrimAsNotNull(this.clonedSearchModel.firstName) === Utilities.lowerCaseTrimAsNotNull(this.searchModel.firstName) &&
            Utilities.lowerCaseTrimAsNotNull(this.clonedSearchModel.middleInitial) === Utilities.lowerCaseTrimAsNotNull(this.searchModel.middleInitial) &&
            Utilities.lowerCaseTrimAsNotNull(this.clonedSearchModel.lastName) === Utilities.lowerCaseTrimAsNotNull(this.searchModel.lastName) &&
            Utilities.lowerCaseTrimAsNotNull(this.clonedSearchModel.dateOfBirth) === Utilities.lowerCaseTrimAsNotNull(this.searchModel.dateOfBirth) &&
            Utilities.lowerCaseTrimAsNotNull(this.clonedSearchModel.gender) === Utilities.lowerCaseTrimAsNotNull(this.searchModel.gender)
          )))
    );
  }

  public searchQuery = '';
  public selectedWorker = '';
  public selectedAgency = '';
  public selectedProgramName = '';
  private searchOptions = {
    shouldSort: true,
    findAllMatches: true,
    threshold: 0.1,
    location: 0,
    distance: 100,
    maxPatternLength: 140,
    minMatchCharLength: 1,
    keys: [
      'pinStringified',
      'firstAndLastName'
      // 'programs.status',
      // 'programs.statusDateMmDdYyyy',
      // 'programs.programCode',
      // 'programs.assignedWorker.firstAndLastName',
      // 'programs.assignedWorker.county',
      // 'programs.assignedWorker.agency'
    ]
  };

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    public appService: AppService,
    private aService: AgencyService,
    private participantService: ParticipantService,
    private userPreferencesService: UserPreferencesService,
    private partService: ParticipantService
  ) {}

  ngOnInit() {
    //this.loadAllParticipantsByType();
    this.routeSub = this.route.params.subscribe(params => {
      const pin = params.pin;
      if (pin) {
        this.searchQuery = pin;
      }
    });
    // Set Default tab.
    this.selectTab(this.recentTabName, true);
    this.genderDrop = [
      { ...new DropDownField(), id: 'M', name: 'Male' },
      { ...new DropDownField(), id: 'F', name: 'Female' }
    ];

    this.selectedWorker = this.appService.user.username.toLowerCase();
  }

  canViewTab(tab: string) {
    if (tab === this.tabByWorkerName || tab === this.referralTabName) {
      return true;
    }

    if (tab === this.searchTabName || tab === this.recentTabName) {
      return true; // No logic around this for now
    }

    return true; // must be a new non-permission based tab. allow it
  }

  selectTab(tab: string, reload: boolean) {
    if (reload === true) {
      this.isLoading = true;
    }
    this.errorMessage = '';
    this.isErrored = false;
    this.searchQuery = '';
    this.currentTab = tab;
    if (this.currentTab === this.recentTabName) {
      if (reload === true) {
        this.loadRecentParticipants();
      }
    } else if (this.currentTab === this.tabByWorkerName) {
      if (reload === true) {
        this.loadParticipantsByWorkerTab();
      }
    } else if (this.currentTab === this.referralTabName) {
      if (reload === true) {
        this.loadReferredParticipants();
      }
    } else if (this.currentTab === this.searchTabName) {
    }
    this.userPreferencesService.currentHomeTab = this.currentTab;
  }

  onSelect(participant: Participant) {
    // if (!this.appService.isParticipantAccessibleIfConfidential(participant)) {
    //   this.errorMessage = 'You are not authorized to view this confidential case.';
    //   this.isErrored = true;
    // } else {
    this.router.navigate(['pin', participant.pin]);
    // }
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  validate() {
    this.isErrored = false;
    this.modelError = {};
    if (this.searchModel.dateOfBirthMmDdYyyy && this.clonedSearchModel.dateOfBirthMmDdYyyy) {
      this.validationManager.resetErrors();
      const result = this.searchModel.isValidForSearch(this.validationManager, Utilities.currentDate.format('MM/DD/YYYY'));
      if (!result.isValid) {
        this.isErrored = true;
        this.modelError = result.errors;
        this.errorMessage = this.validationManager.errors[0].formatted;
      }
    }
  }

  resetAdvSearchResults() {
    this.advSearchParticipant = null;
    this.searchedParticipants = [];
    this.isLoading = false;
    this.isErrored = false;
    this.modelError = {};
  }

  onAdvSearch() {
    this.isErrored = false;
    this.resetAdvSearchResults();
    if (!(this.advSearchQuery || (this.searchModel.firstName && this.searchModel.lastName))) {
      this.isErrored = true;
      this.errorMessage = 'Data must be entered in either the PIN field or the First Name and Last Name fields to get search results.';
      return;
    }
    if (this.advSearchQuery && this.canAdvSearch) {
      this.getParticipantByPin(this.advSearchQuery);
    } else if (this.searchModel.firstName || this.searchModel.lastName) {
      this.searchByName();
    }
  }

  searchByName() {
    Person.clone(this.searchModel, this.clonedSearchModel);
    this.validate();
    if (this.isErrored === false) {
      this.isSearching = true;
      this.participantService
        .getParticipantsBySearch(this.searchModel)
        .pipe(take(1))
        .subscribe(
          parts => {
            if (!parts.length) {
              this.isSearching = false;
              this.isErrored = true;
              this.errorMessage = 'No match found. Check your search criterion and try again.';
            } else {
              this.searchedParticipants = [...parts];
              this.configMy.currentPage = 1;
              this.isSearching = false;
            }
          },
          error => {
            this.isErrored = true;
            this.isSearching = false;
          }
        );
    }
  }

  clearSearch() {
    this.searchModel = new Person();
    this.clonedSearchModel = new Person();
    this.advSearchQuery = '';
    this.resetAdvSearchResults();
  }

  get isPinFieldDisabled(): boolean {
    return (
      this.isSearching ||
      !!this.searchModel.firstName ||
      !!this.searchModel.lastName ||
      !!this.searchModel.dateOfBirthMmDdYyyy ||
      !!this.searchModel.gender ||
      !!this.searchModel.middleInitial
    );
  }

  get isParticipantFieldsDisabled(): boolean {
    return !!this.advSearchQuery || this.isSearching;
  }

  getParticipantByPin(pinQuery) {
    let isPin = false;
    // pad 9 digits with a leading 0
    // if (pinQuery && /^\d{9}$/.test(pinQuery)) {
    //   pinQuery = '0' + pinQuery;
    //   this.advSearchQuery = pinQuery;
    //   isPin = true;
    // }
    if (pinQuery && /^\d{10}$/.test(pinQuery)) {
      isPin = /^\d{10}$/.test(pinQuery); // exactly 10 digits
      this.advSearchQuery = pinQuery;
    }
    if (isPin) {
      this.isSearching = true;
      this.isErrored = false;
      // see if we have it already
      const isSamePin = this.advSearchParticipant && this.advSearchParticipant.pinStringified === pinQuery;
      if (!isSamePin) {
        // get it from the server
        this.participantService
          .getParticipant(pinQuery.toString(), true)
          .pipe(take(1))
          .pipe(
            catchError(error => this.handleSearchError(error)),
            delay(500),
            concatMap(part => {
              this.partService.isFromPartSumGuard.next(false);
              if (!part) {
                this.isSearching = false;
                this.isErrored = true;
                throw new Error('PIN not found!');
              } else {
                this.advSearchParticipant = part;
                this.searchedParticipants = [part];
                this.configMy.currentPage = 1;
              }
              return this.partService.getParticipantSummaryDetails(pinQuery.toString()).pipe(take(1));
            })
          )
          .subscribe(() => {
            this.isSearching = false;
          });
      } else {
        this.isSearching = false;
        this.isErrored = true;
      }
    } else {
      this.errorMessage = 'PIN not valid. Please enter a valid 10 digit PIN number.';
      this.isErrored = true;
      this.isSearching = false;
    }
  }

  onReferralSearch() {
    this.isErrored = false;
    this.errorMessage = null;
    // Search via text input goes here.
    if (this.searchQuery != null && this.searchQuery.trim() !== '') {
      const query = this.searchQuery.trim().toLowerCase();

      this.referredParticipants = [];
      const nonUnqiueSearchResults = [];
      const fuse = new Fuse(this.allReferredParticipants, this.searchOptions);
      const searchResults = fuse.search<Participant>(query);

      // We have to find the deserialized object using the results of the search.
      for (const searchResult of searchResults) {
        const participants = this.allReferredParticipants.filter(p => {
          return p.pin === searchResult.pin;
        });
        for (const p of participants) {
          nonUnqiueSearchResults.push(p);
        }
      }

      this.referredParticipants = nonUnqiueSearchResults.filter(Utilities.onlyUnique);
    } else {
      this.referredParticipants = this.allReferredParticipants; // Resets search if we don't have a searchQuery
    }
  }

  onMySearch() {
    this.isErrored = false;
    this.errorMessage = null;
    // Search via text input goes here.
    if (this.searchQuery != null && this.searchQuery.trim() !== '') {
      const query = this.searchQuery.trim().toLowerCase();
      // const pinQuery = query.replace(/\W+/g, '');
      // const isPin = /^\d{10}$/.test(pinQuery); // exactly 10 digits
      this.workerParticipants = [];

      const fuse = new Fuse(this.allWorkerParticipants, this.searchOptions);
      const searchResults = fuse.search(query);

      // We have to find the deserialized object using the results of the search.
      for (const searchResult of searchResults) {
        const participant = this.allWorkerParticipants.find(p => {
          return p.pin === searchResult.pin;
        });
        this.workerParticipants.push(participant);
      }
    } else {
      this.workerParticipants = this.allWorkerParticipants; // Resets search if we don't have a searchQuery
    }
  }

  public reverseReferredParticipantsList(order: string) {
    this.referredParticipants = Participant.sortBySortAndGroupOrder(this.referredParticipants, order);
    this.isReferredParticipantsReversed = !this.isReferredParticipantsReversed;
  }

  public reverseWorkerParticipantsList(order: string) {
    this.workerParticipants = Participant.sortBySortAndGroupOrder(this.workerParticipants, order);
    this.isWorkerParticipantsReversed = !this.isWorkerParticipantsReversed;
  }

  public handleSearchError(error: HttpErrorResponse): Observable<Participant> {
    this.isErrored = true;
    this.isSearching = false;
    let errorMessage = 'Unexpected error.';
    //Meessage for PIN not found
    const pinNotValid = 'PIN not found.';
    if (error != null) {
      if (error instanceof Error) {
        errorMessage = error.message;
      } else {
        const body = error as any;
        if (body.error != null) {
          errorMessage = body.error.message;
        }
        //Check for 404 Not Found
        if (error.status === 404) {
          errorMessage = pinNotValid;
        } else {
          errorMessage = body.message;
        }
      }
    }
    this.errorMessage = errorMessage;
    return empty();
  }

  private destroyParticipantSubscription() {
    if (this.rPartSub != null) {
      this.rPartSub.unsubscribe();
    }
    if (this.mPartSub != null) {
      this.mPartSub.unsubscribe();
    }
    if (this.refPartSub != null) {
      this.refPartSub.unsubscribe();
    }
    if (this.sPartSub != null) {
      this.sPartSub.unsubscribe();
    }
    if (this.wSub != null) {
      this.wSub.unsubscribe();
    }
  }

  private loadAllParticipantsByType() {
    this.loadRecentParticipants();
    this.loadParticipantsByWorkerTab();
    this.loadReferredParticipants();
  }

  private loadRecentParticipants() {
    this.isLoading = true;
    this.rPartSub = this.participantService.getRecentParticipants().subscribe(parts => {
      this.recentParticipants = parts;
      this.isLoading = false;
    });
  }

  // Defaults to logged in user.
  public getParticipantsByWorker(wamsId: string, program: string, agency: string) {
    this.isLoading = true;
    this.isWorkerParticipantsReversed = false;
    if (program != null || program !== '') {
      this.mPartSub = this.participantService.getParticipantsByWorkerByProgram(wamsId, program, agency).subscribe(parts => {
        Participant.sortBySortAndGroupOrder(parts);
        this.workerParticipants = parts;
        this.allWorkerParticipants = Array.from(parts);
        this.configMy.currentPage = 1;
        this.isLoading = false;
      });
    } else {
      this.mPartSub = this.participantService.getParticipantsByWorker(wamsId, agency).subscribe(parts => {
        Participant.sortBySortAndGroupOrder(parts);
        this.workerParticipants = parts;
        this.allWorkerParticipants = Array.from(parts);
        this.configMy.currentPage = 1;
        this.isLoading = false;
      });
    }
  }

  public selectedAgencyChange() {
    this.selectedWorker = null;
    this.selectedProgramName = null;
    this.agencyProgramsDrop = [];
    this.workerDrop = [];
    this.workerParticipants = [];
    this.loadWorkers(this.selectedAgency);
  }

  public selectedProgramNameChange() {
    if (this.programWorkers == null) {
      return;
    }
    this.selectedWorker = null;
    this.workerParticipants = [];
    this.initWorkerDrop();
  }

  public selectedWorkerChange() {
    this.getParticipantsByWorker(this.selectedWorker, this.selectedProgramName, this.selectedAgency);
  }

  public initProgramsDrop(data: ProgramWorker[]) {
    // For State Worker show all the programs that Agency supports and rest only Programs user has access to should show thus
    // we filter the programs here.
    if (this.appService.isUserAuthorized(Authorization.canChangeAgencyOnHomePage)) {
      this.programWorkers = data;
    } else {
      this.programWorkers = this.appService.filterProgramsForUserAuthorized(data);
    }
    this.agencyProgramsDrop = [];

    let isAllPrograms = false;

    // Add All Programs to the DropDown if the user has access to more than one program
    if (this.programWorkers.length > 1 && !this.appService.isUserAuthorized(Authorization.canChangeAgencyOnHomePage)) {
      const dd1 = new DropDownField();
      dd1.id = 'All Programs';
      dd1.name = 'All Programs';
      this.agencyProgramsDrop.push(dd1);
      isAllPrograms = true;
    }
    // Looping through the filtered workers that user has access to.
    for (const x of this.programWorkers) {
      const dd2 = new DropDownField();
      dd2.id = x.programName;
      dd2.name = x.programName;
      this.agencyProgramsDrop.push(dd2);
    }
    if (this.isFirstLoad) {
      if (isAllPrograms) {
        this.selectedProgramName = 'All Programs';
      } else {
        this.setDefaultProgram();
      }
    }
    this.isFirstLoad = false;
  }

  public loadWorkers(selectedAgency: string) {
    if (Utilities.isStringEmptyOrNull(selectedAgency)) {
      return;
    }

    this.wSub = this.aService.getAgencyWorkersByAgencyCode(selectedAgency).subscribe(data => {
      this.initProgramsDrop(data);
      this.setParticipantsByWorker();
    });
  }

  public loadParticipantsByWorkerTab() {
    this.isFirstLoad = true;
    this.setDefaultWorkerAndAgency();
    this.loadWorkers(this.selectedAgency);
    this.loadOrganizations();
  }

  private setParticipantsByWorker() {
    if (this.selectedWorker != null && this.selectedWorker.trim() !== '') {
      if ((this.selectedAgency != null && this.selectedAgency.trim() !== '') || (this.selectedProgramName != null && this.selectedProgramName.trim() !== '')) {
        this.getParticipantsByWorker(this.selectedWorker, this.selectedProgramName, this.selectedAgency);
      } else {
        this.getParticipantsByWorker(this.appService.user.username, '', this.appService.user.agencyCode);
        this.selectedAgency = this.appService.user.agencyCode;
        this.selectedProgramName = '';
      }
    } else {
      this.getParticipantsByWorker(this.appService.user.username, '', this.appService.user.agencyCode);
      this.selectedWorker = this.appService.user.username;
    }
  }

  private setDefaultWorkerAndAgency() {
    this.selectedAgency = this.appService.user.agencyCode;
    this.selectedWorker = this.appService.user.username.toLowerCase();
  }

  private setDefaultProgram() {
    let foundUsersProgram = false;
    let programName = '';

    if (!Utilities.isStringEmptyOrNull(this.selectedAgency)) {
      const ps = this.appService.filterProgramsForUserAuthorized<ProgramWorker>(this.programWorkers);
      for (const pro of ps) {
        if (foundUsersProgram) {
          break;
        }
        if (pro.agencyWorkers != null) {
          for (const w of pro.agencyWorkers) {
            if (w.wamsId == null || this.appService.user.username == null) {
              console.warn('Cannot set default program because of missing wamsId');
              return;
            }
            if (w.wamsId.toLowerCase() === this.appService.user.username.toLowerCase()) {
              foundUsersProgram = true;
              if (foundUsersProgram) {
                programName = pro.programName;
                break;
              }
            }
          }
        }
      }
    } else {
      console.warn('We can only set program if Agency is known.');
    }

    if (foundUsersProgram) {
      this.selectedProgramName = programName;
    }
    this.initWorkerDrop();
  }

  public loadOrganizations() {
    if (!this.workerDrop || (!this.workerDrop.length && this.appService != null && this.appService.user != null)) {
      this.wSub = this.aService.getAgencies().subscribe(data => this.initAgenciesDrop(data));
    }

    // if (this.selectedWorker != null && this.selectedWorker.trim() !== '') {
    //   this.getParticipantsByWorker(this.selectedWorker);
    // } else {
    //   this.getParticipantsByWorker(this.appService.user.username);
    //   this.selectedWorker = this.appService.user.username;
    // }
  }

  private loadReferredParticipants() {
    this.isLoading = true;
    this.isReferredParticipantsReversed = false;
    this.refPartSub = this.participantService.getReferredParticipants().subscribe(parts => {
      this.referredParticipants = Participant.sortRefPart(parts);
      if (parts != null) {
        this.allReferredParticipants = Array.from(this.referredParticipants);
      }
      this.isLoading = false;
    });
  }

  private initWorkerDrop() {
    // Start by clearing dropdown.
    this.workerDrop = [];

    // Workers list which will populate based on security authorization.
    const workers = [];

    // Change to program based worker drop if we have canChangeAgencyOnHomePage.
    const displayWorkersByProgram = this.appService.isUserAuthorized(this.Auth.canChangeAgencyOnHomePage);

    if (displayWorkersByProgram) {
      const program = this.programWorkers.find(x => x.programName === this.selectedProgramName);
      if (program != null && program.agencyWorkers != null) {
        for (const item of program.agencyWorkers) {
          workers.push(item);
        }
      }
    } else {
      for (const pw of this.programWorkers) {
        for (const w of pw.agencyWorkers) {
          // Only add unique objects because some workers can handle multi-programs.
          if (workers.find(x => x.id === w.id) == null) {
            workers.push(w);
          }
        }
      }
    }

    // Sort by last name.
    const sortedWorkers = workers.sort((obj1, obj2) => {
      if (obj1.lastName > obj2.lastName) {
        return 1;
      }
      if (obj1.lastName < obj2.lastName) {
        return -1;
      }
      return 0;
    });

    // Now we have our sorted list of workers, let's make our dropdown.
    for (const item of sortedWorkers) {
      const dd = new DropDownField();
      dd.id = item.wamsId.toLowerCase();
      dd.name = item.fullNameWithMiddleInitialTitleCase;
      this.workerDrop.push(dd);
    }
  }

  private initAgenciesDrop(agencies: Agency[]) {
    this.agenciesDrop = [];
    if (agencies != null) {
      agencies.filter(i => i.agencyCode !== 'DCF').map(({ agencyCode, agencyName }) => this.agenciesDrop.push(new DropDownField(agencyCode, agencyName)));
    }
  }

  ngOnDestroy() {
    this.destroyParticipantSubscription();
    if (this.routeSub) {
      this.routeSub.unsubscribe();
    }
  }
}
