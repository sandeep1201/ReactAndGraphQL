import { ActivatedRoute, Router } from '@angular/router';
import { Component, EventEmitter, Output, Input, OnInit, OnDestroy } from '@angular/core';
import { Subscription, forkJoin, of } from 'rxjs';

import { AppService } from './../../core/services/app.service';
import { Authorization } from '../../shared/models/authorization';
import { BaseParticipantComponent } from '../../shared/components/base-participant-component';
import { DropDownField } from '../../shared/models/dropdown-field';
import { FieldDataService } from '../../shared/services/field-data.service';
import { ParticipantService } from '../../shared/services/participant.service';
import { UIKitUtilities } from '../../shared/ui-kit.utilities';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { MciParticipantSearch } from '../../shared/models/mciParticipantSearch.model';
import { Participant } from '../../shared/models/participant';
import { Person } from '../../shared/models/person.model';
import { ClientRegistration } from '../../shared/models/client-registration.model';
import { FinalistAddress } from '../../shared/models/finalist-address.model';
import { MciParticipantReturn } from '../../shared/models/mciParticipantReturn.model';
import { ClientRegistrationService } from '../../shared/services/client-registration.service';
import { Status } from '../../shared/models/status.model';
import { Utilities } from '../../shared/utilities';
import { PadPipe } from '../../shared/pipes/pad.pipe';
import { take } from 'rxjs/operators';
import { SsnVerificationCodes } from '../enums/ssn-verification-codes.enum';
import { EmergencyAssistanceService } from 'src/app/features-modules/emergency-assistance/services/emergancy-assistance.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  providers: [FieldDataService, ClientRegistrationService]
})
export class RegistrationComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public secondaryPhoneRequired = false;
  public primaryPhoneRequired = false;
  Auth = Authorization;
  @Output()
  saveAndExitAndStatus = new EventEmitter<Status>();
  @Input()
  regId = 0;
  @Input()
  mciParticipantSearchModel: MciParticipantSearch;
  @Input()
  selectedMciParticipant: MciParticipantReturn;
  @Input() eaRequestDetails: any;

  public inEAMode = false;
  public doWorkerHaveEARole = false;
  public isEASameAgency = false;
  private countiesSub: Subscription;
  private countriesSub: Subscription;
  private suffixTypesSub: Subscription;
  private aliasTypesSub: Subscription;
  private langSub: Subscription;
  private tribeSub: Subscription;
  private ssnSub: Subscription;
  public genderDrop: DropDownField[];
  public aliasTypesDrop: DropDownField[];
  public aliasTypesDropNameAsId: DropDownField[];
  public suffixTypesDrop: DropDownField[];
  public suffixTypesDropNameAsId: DropDownField[];
  public countiesDrop: DropDownField[];
  public countriesDrop: DropDownField[];
  public languagesDrop: DropDownField[];
  public ssnTypesDrop: DropDownField[];
  public tribeDrop: DropDownField[];
  public hispanicDropdowns: DropDownField[];
  public isCollapsed = false;
  public hadSaveError = false;
  public isSaving = false;
  private hasTriedSave = false;
  private isSectionModified = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public model: ClientRegistration;
  public originalModel: ClientRegistration;
  public isEthnicityAndRaceReadOnly = true;
  public isPinConfidentialReadOnly = false;
  public isPersonInformationReadOnly = true;
  public isAliasReadOnly = true;
  public englishId: number;
  public otherId: number;
  public isSectionValid: boolean;
  private fieldDataLoaded = false;
  public savingStatus: Status;
  private ggZipSub: Subscription;
  public pin;
  public isEditDemographicsMode = false;
  public isReadOnly = false;
  public suggestedAddressFromFinalist: string;
  public isAddressValidated = true;
  public showSaveButton = true;
  public addressIsValid: boolean;
  public errorMsg: string[];
  public userEnteredAddressLine1: string;
  public userEnteredAddressLine2: string;
  public userEnteredZip: string;
  public userEnteredCity: string;
  public userEnteredMailingAddressLine1: string;
  public userEnteredMailingAddressLine2: string;
  public userEnteredMailingZip: string;
  public userEnteredMailingCity: string;
  public useSuggestedAddress = false;
  public useEnteredAddress = false;
  public resubmitAddress = false;
  public finalistAddressLoaded = false;
  public addressForFinalist: FinalistAddress;
  public isAddressDisabled = false;
  public isHouseHoldAddrValidated = false;
  public isMailingAddrValidated = false;
  public isReadOnlyBasedOnSsnVerificationCode = false;
  constructor(
    public appService: AppService,
    private fdService: FieldDataService,
    private padPipe: PadPipe,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService,
    private clientRegistrationService: ClientRegistrationService,
    private eaService: EmergencyAssistanceService
  ) {
    super(route, router, partService);
  }

  get isLoaded() {
    return this.participant != null && this.model != null && this.fieldDataLoaded === true;
  }

  ngOnInit() {
    this.initHispanicDropdowns();
    this.genderDrop = this.initGenderDrop();
    this.doWorkerHaveEARole = this.appService.isUserEASupervisor() || this.appService.isUserEAWorker() || this.appService.isUserHDWorker();
    this.inEAMode = !!this.eaRequestDetails || (this.doWorkerHaveEARole && !this.appService.isUserCFWorker());

    this.route.params.subscribe(params => {
      if (params['pin']) {
        this.pin = params['pin'];
        this.isEditDemographicsMode = true;
      }
    });

    // Load field data first because we need it when make search fields to our model.
    forkJoin(
      this.fdService.getSuffixTypes().pipe(take(1)),
      this.fdService.getAliasTypes().pipe(take(1)),
      this.fdService.getSsnTypes().pipe(take(1)),
      this.fdService.getCounties('numeric').pipe(take(1)),
      this.fdService.getCountries().pipe(take(1)),
      this.fdService.getLanguages().pipe(take(1)),
      this.fdService.getTribeNames().pipe(take(1)),
      this.doWorkerHaveEARole && this.pin ? this.eaService.getEARequestList(this.pin) : of(null)
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initSuffixTypesDrop(results[0]);
        this.initAliasTypesDrop(results[1]);
        this.initSsnTypesDrop(results[2]);
        this.initWiCountiesDrop(results[3]);
        this.initCountriesDrop(results[4]);
        this.initLanguagesDrop(results[5]);
        this.initTribeDrop(results[6]);
        if (results[7]) {
          this.isEASameAgency = results[7].length > 0 && results[7][0].organizationCode === this.appService.user.agencyCode;
          this.setEAReadOnly();
        }

        this.fieldDataLoaded = true;

        if (this.pin != null) {
          this.selectedMciParticipant = new MciParticipantReturn();
        }
        if (this.selectedMciParticipant != null) {
          if (this.selectedMciParticipant.mciId != null) {
            // We have a participant in MCI.
            this.loadModel();
          } else if (this.pin != null) {
            this.getMciIdFromPin();
          } else {
            // Map what was entered on the clearance to client reg.
            this.createNewModelAndParticipant();
            this.mapMciToModel();
            this.originalModel = new ClientRegistration();
            ClientRegistration.clone(this.model, this.originalModel);
          }
        }
      });
  }

  onParticipantInit() {
    // If the participant is enrolled/referred in TMJ/TJ/CF Race and Ethnicity are editable.
    if (
      this.participant.enrolledTmjProgram != null ||
      this.participant.enrolledTJProgram != null ||
      this.participant.enrolledCFProgram != null ||
      this.participant.enrolledFCDPProgram != null
    ) {
      this.isEthnicityAndRaceReadOnly = false;
      this.isPersonInformationReadOnly = false;
      this.isAliasReadOnly = false;
    } else if (
      this.participant.referredTmjProgram != null ||
      this.participant.referredTJProgram != null ||
      this.participant.referredCFProgram != null ||
      this.participant.referredFCDPProgram != null
    ) {
      this.isEthnicityAndRaceReadOnly = false;
      this.isPersonInformationReadOnly = false;
      this.isAliasReadOnly = false;
    }

    this.setEAReadOnly();
  }

  setEAReadOnly() {
    if (this.doWorkerHaveEARole) {
      this.isEthnicityAndRaceReadOnly = true;
      this.isPersonInformationReadOnly = true;
      this.isAliasReadOnly = true;
      this.isPinConfidentialReadOnly = true;
      if (
        this.isEASameAgency ||
        this.appService.isUserHDWorker() ||
        (this.eaRequestDetails && this.eaRequestDetails.id) ||
        (this.appService.isUserCFWorker() &&
          this.participant &&
          this.appService.isUserAuthorizedToEditClientReg(this.participant, this.isEditDemographicsMode, this.isEASameAgency))
      ) {
        this.isEthnicityAndRaceReadOnly = false;
        this.isPersonInformationReadOnly = false;
        this.isAliasReadOnly = false;
        this.isPinConfidentialReadOnly = false;
      }
    }
  }

  scroll(selector: string) {
    UIKitUtilities.scrollIntoView(selector);
  }

  private initHispanicDropdowns() {
    this.hispanicDropdowns = [];
    const d1 = new DropDownField();
    d1.name = 'Hispanic or Latino';
    d1.id = true;
    this.hispanicDropdowns.push(d1);

    const d2 = new DropDownField();
    d2.name = 'Not Hispanic or Latino';
    d2.id = false;
    this.hispanicDropdowns.push(d2);
  }

  checkIfPhoneNumberRequired() {
    if (this.model.primaryPhone.canText || this.model.primaryPhone.canVoiceMail) {
      this.primaryPhoneRequired = true;
    } else {
      this.primaryPhoneRequired = false;
    }
    if (this.model.secondaryPhone.canText || this.model.secondaryPhone.canVoiceMail) {
      this.secondaryPhoneRequired = true;
    } else {
      this.secondaryPhoneRequired = false;
    }
  }

  loadModel() {
    this.clientRegistrationService.getClientRegistration(this.selectedMciParticipant).subscribe(data => this.initModel(data));
  }

  getMciIdFromPin() {
    this.partSub = this.partService.getCachedParticipant(this.pin).subscribe(p => {
      this.initParticipant(p);
      this.onParticipantInit();
      if (p.mciId) {
        this.selectedMciParticipant.mciId = p.mciId;
        this.loadModel();
      }
      return of(true);
    });
  }

  // For new participants not known to CWW or WWP.
  createNewModelAndParticipant() {
    this.model = new ClientRegistration();
    this.model.raceEthnicity.historySequenceNumber = 1;
    this.model.householdAddress.state = 'WI';
    this.participant = new Participant();
  }

  initModel(data: ClientRegistration) {
    if (data == null && data.mciId != null) {
      this.mapMciToModel();
    } else {
      this.model = data;
      this.isReadOnlyBasedOnSsnVerificationCode = this.model.ssnVerificationCode === SsnVerificationCodes.V;
      // We cant get pin from url, so we get it from our response here.
      if (data.pinNumber != null && this.partSub == null) {
        // this.emitPin.emit(data.pinNumber);
        this.partSub = this.partService.getParticipant(data.pinNumber.toString()).subscribe(part => {
          this.initParticipant(part);
          this.onParticipantInit();
        });
      } else if (this.pin == null) {
        // We dont have the pin if the participant isn't known to CWW Or WWP.
        this.createNewModelAndParticipant();
      }

      this.mapMciToModel(); // might get removed for when we dont get to client reg from clear screen
    }

    // We should have a model at this point.
    if (this.model.isMailingAddressDisplayed) {
      if (this.model.mailingAddress.state === null || this.model.mailingAddress.state === undefined) this.model.mailingAddress.state = 'WI';
    } else {
      if (this.model.householdAddress.state === null || this.model.householdAddress.state === undefined) this.model.householdAddress.state = 'WI';
    }

    this.originalModel = new ClientRegistration();
    ClientRegistration.clone(this.model, this.originalModel);

    // Determine if the screen is read only if we are in edit demo mode.
    if (this.isEditDemographicsMode) {
      if (this.appService.isUserAuthorizedToEditClientReg(this.participant, this.isEditDemographicsMode, this.isEASameAgency)) {
        this.isReadOnly = false;
      } else if (this.appService.isUserAuthorizedToViewClientReg(this.participant)) {
        this.isReadOnly = true;
        this.isEthnicityAndRaceReadOnly = true;
        this.isAliasReadOnly = true;
        this.isPersonInformationReadOnly = true;
        this.isPinConfidentialReadOnly = true;
      }

      this.setEAReadOnly();
    }
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();

      // Call the model's validate method.
      const result = this.model.validate(this.validationManager, this.englishId, this.isPinConfidentialReadOnly, Utilities.currentDate.format('MM/DD/YYYY'), this.inEAMode);

      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;

      if (this.isSectionValid === true) {
        this.hasTriedSave = false;
      }
    }
  }

  sidebarToggle() {
    this.isCollapsed = !this.isCollapsed;
    return this.isCollapsed;
  }

  saveAndExit() {
    this.hasTriedSave = true;
    this.savingStatus = null;
    this.validate();
    if (this.isSectionValid) {
      this.isSaving = true;
      const postModel = new ClientRegistration();
      ClientRegistration.clone(this.model, postModel);
      postModel.cleanseForPost(this.suffixTypesDrop);
      postModel.inEAMode = this.inEAMode;
      if (this.inEAMode && this.eaRequestDetails && this.eaRequestDetails.id) {
        postModel.eaPin = this.eaRequestDetails.pin;
        postModel.eaRequestId = this.eaRequestDetails.id;
      }
      this.clientRegistrationService.saveClientRegistration(postModel).subscribe(
        data => {
          this.savingStatus = data;
          this.isSaving = false;
          // Stay on client reg when errored.
          if (this.savingStatus.errorMessages == null || this.savingStatus.errorMessages.length === 0) {
            if (this.isEditDemographicsMode) {
              this.router.navigateByUrl(`/pin/${this.pin}`);
            } else {
              this.saveAndExitAndStatus.emit(this.savingStatus);
            }
          }
        },
        error => {
          this.savingStatus = null;
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }

  exitRegistration() {
    if (this.isSectionModified === true) {
      this.appService.isDialogPresent = true;
    } else {
      this.exitRegistrationIgnoreChanges(this.savingStatus);
    }
  }

  exitRegistrationIgnoreChanges($event) {
    if (this.isEditDemographicsMode) {
      // This is a non-normal situation when we are in this mode.
      // The super modal is normally shown over an existing route but
      // so the parent component would be listening for this event.  In
      // this case we don't have one so we will re-direct to the Participant
      // Summary.
      this.router.navigateByUrl(`/pin/${this.pin}`);
    } else {
      this.saveAndExitAndStatus.emit($event);
    }
  }

  ngOnDestroy() {
    // TODO: Find the Angular way!
    // let body = document.getElementsByTagName('body')[0];
    // body.classList.remove('noscroll');
    if (this.suffixTypesSub != null) {
      this.suffixTypesSub.unsubscribe();
    }
    if (this.aliasTypesSub != null) {
      this.aliasTypesSub.unsubscribe();
    }
    if (this.countiesSub != null) {
      this.countiesSub.unsubscribe();
    }
    if (this.langSub != null) {
      this.langSub.unsubscribe();
    }
    if (this.tribeSub != null) {
      this.tribeSub.unsubscribe();
    }
    if (this.ssnSub != null) {
      this.ssnSub.unsubscribe();
    }
    if (this.countriesSub != null) {
      this.countriesSub.unsubscribe();
    }
    if (this.ggZipSub != null) {
      this.ggZipSub.unsubscribe();
    }
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

  private initSuffixTypesDrop(data) {
    this.suffixTypesDrop = data;
    this.suffixTypesDropNameAsId = Utilities.createDropDownWithIdAsName(data);
  }

  private initAliasTypesDrop(data) {
    this.aliasTypesDrop = data;
    this.aliasTypesDropNameAsId = Utilities.createDropDownWithIdAsName(data);
  }

  private initSsnTypesDrop(data) {
    this.ssnTypesDrop = data;
  }

  private initWiCountiesDrop(data) {
    this.countiesDrop = data;
  }

  private initCountriesDrop(data) {
    this.countriesDrop = data;
  }

  private initLanguagesDrop(data) {
    this.englishId = Utilities.idByFieldDataName('English', data);
    this.languagesDrop = data;
  }

  private initTribeDrop(data) {
    this.otherId = Utilities.idByFieldDataName('Other', data);
    this.tribeDrop = data;
  }

  private getSuffixIdByName(suffix: string): number {
    if (Utilities.isStringEmptyOrNull(suffix)) {
      return 0;
    }

    if (this.suffixTypesDrop != null) {
      for (const s of this.suffixTypesDrop) {
        if (s.name.trim().toLocaleLowerCase() === suffix.trim().toLocaleLowerCase()) {
          return s.id;
        }
      }
    }
  }

  mapMciToModel() {
    if (this.mciParticipantSearchModel != null) {
      if (this.model == null) {
        this.model = new ClientRegistration();
        this.model.name = new Person();
      }

      this.model.name.firstName = this.selectedMciParticipant.name.firstName;
      this.model.name.middleInitial = this.selectedMciParticipant.name.middleInitial;
      this.model.name.lastName = this.selectedMciParticipant.name.lastName;
      this.model.name.suffixTypeId = this.getSuffixIdByName(this.selectedMciParticipant.name.suffix);
      this.model.dateOfBirthMmDdYyyy = this.selectedMciParticipant.dateOfBirthMmDdYyyy;

      // A zero from mci search means no ssn.
      if (this.selectedMciParticipant.ssn != null && this.selectedMciParticipant.ssn.length > 1) {
        if (this.selectedMciParticipant.ssn.length < 9) {
          this.selectedMciParticipant.ssn = this.padPipe.transform(this.selectedMciParticipant.ssn, 9);
        }
        this.model.ssn = this.selectedMciParticipant.ssn;
      } else if ((this.selectedMciParticipant.ssn != null && this.selectedMciParticipant.ssn.length === 1) || this.selectedMciParticipant.isNoSsn) {
        this.model.isNoSsn = true;
      }

      this.model.genderIndicator = this.selectedMciParticipant.gender;

      // Comes from return but we wont have it for new participants.
      this.model.mciId = this.selectedMciParticipant.mciId;

      this.model.ssnVerificationCode = this.selectedMciParticipant.ssnVerificationCode;
      this.model.ssnVerificationCodeDescription = this.selectedMciParticipant.ssnVerificationCodeDescription;
    }
  }
  validateAddressFromFinalist() {
    if (
      this.model.isMailingAddressDisplayed &&
      this.model.mailingAddress.addressLine1 !== null &&
      this.model.mailingAddress.city !== null &&
      this.model.mailingAddress.zip !== null
    ) {
      this.addressForFinalist = this.model.mailingAddress;
      this.isMailingAddrValidated = true;
      this.isHouseHoldAddrValidated = false;
    } else if (
      !this.model.isMailingAddressDisplayed &&
      this.model.householdAddress.addressLine1 !== null &&
      this.model.householdAddress.city !== null &&
      this.model.householdAddress.zip !== null
    ) {
      this.addressForFinalist = this.model.householdAddress;
      this.isHouseHoldAddrValidated = true;
      this.isMailingAddrValidated = false;
    }

    this.clientRegistrationService.getFinalistAddress(this.addressForFinalist, this.pin).subscribe(res => {
      this.suggestedAddressFromFinalist = res.fullAddress;
      this.addressIsValid = res.isValid;
      this.errorMsg = res.errorMsg;
      this.useSuggestedAddress = false;
      this.useEnteredAddress = false;
      this.resubmitAddress = false;
      this.finalistAddressLoaded = true;
      if (this.errorMsg.length > 0) {
        this.suggestedAddressFromFinalist = this.errorMsg.join('\n');
      }
    });

    // We'll store the address when the user enters validate button because the address can change until the user hits Save.
    if (this.isHouseHoldAddrValidated) {
      this.userEnteredAddressLine1 = this.model.householdAddress.addressLine1;
      this.userEnteredZip = this.model.householdAddress.zip;
      this.userEnteredCity = this.model.householdAddress.city;
    }
    if (this.isMailingAddrValidated) {
      this.userEnteredMailingAddressLine1 = this.model.mailingAddress.addressLine1;
      this.userEnteredMailingZip = this.model.mailingAddress.zip;
      this.userEnteredMailingCity = this.model.mailingAddress.city;
    }
  }

  public validateHouseholdMailingSwitch() {
    if ((!this.model.isMailingSameAsHouseholdAddress || this.model.isHomeless) && (this.model.mailingAddress.state === null || this.model.mailingAddress.state === undefined))
      this.model.mailingAddress.state = 'WI';
    if (this.model.isMailingSameAsHouseholdAddress !== this.originalModel.isMailingSameAsHouseholdAddress || this.model.isHomeless !== this.originalModel.isHomeless) {
      this.isAddressValidated = false;
      this.showSaveButton = false;
    } else {
      this.validateFinalistAddress();
    }

    this.isAddressDisabled = false;
    this.finalistAddressLoaded = false;
    this.useSuggestedAddress = false;
    this.useEnteredAddress = false;
    this.resubmitAddress = false;
  }

  public validateFinalistAddress() {
    if (this.model.isMailingAddressDisplayed) {
      if (
        JSON.stringify(Utilities.cleanseModelForApi(this.model.mailingAddress)) !== JSON.stringify(Utilities.cleanseModelForApi(this.originalModel.mailingAddress)) &&
        !this.useSuggestedAddress &&
        !this.useEnteredAddress
      ) {
        this.isAddressValidated = false;
        this.showSaveButton = false;
      } else {
        this.isAddressValidated = true;
        this.showSaveButton = true;
      }
    } else {
      if (
        JSON.stringify(Utilities.cleanseModelForApi(this.model.householdAddress)) !== JSON.stringify(Utilities.cleanseModelForApi(this.originalModel.householdAddress)) &&
        !this.useSuggestedAddress &&
        !this.useEnteredAddress
      ) {
        this.isAddressValidated = false;
        this.showSaveButton = false;
      } else {
        this.isAddressValidated = true;
        this.showSaveButton = true;
      }
    }
  }

  validateUserClickSuggested() {
    if (this.useSuggestedAddress) {
      this.resubmitAddress = false;
      this.useEnteredAddress = false;
      this.isAddressDisabled = true;
      const suggestedAddress = this.suggestedAddressFromFinalist.split(', ');
      if (this.isHouseHoldAddrValidated) {
        this.model.householdAddress.addressLine1 = suggestedAddress[0];
        this.model.householdAddress.city = suggestedAddress[1];
        this.model.householdAddress.zip = suggestedAddress[2].split('WI ')[1];
      } else if (this.isMailingAddrValidated) {
        this.model.mailingAddress.addressLine1 = suggestedAddress[0];
        this.model.mailingAddress.city = suggestedAddress[1];
        this.model.mailingAddress.zip = suggestedAddress[2].split('WI ')[1];
      }
    } else this.isAddressDisabled = false;
  }
  validateUserClickResubmit() {
    if (this.resubmitAddress) {
      this.useSuggestedAddress = false;
      this.useEnteredAddress = false;
      this.isAddressDisabled = false;
      this.finalistAddressLoaded = false;
      if (this.isHouseHoldAddrValidated) {
        this.model.householdAddress.addressLine1 = this.userEnteredAddressLine1;
        this.model.householdAddress.city = this.userEnteredCity;
        this.model.householdAddress.zip = this.userEnteredZip;
      } else if (this.isMailingAddrValidated) {
        this.model.mailingAddress.addressLine1 = this.userEnteredMailingAddressLine1;
        this.model.mailingAddress.city = this.userEnteredMailingCity;
        this.model.mailingAddress.zip = this.userEnteredMailingZip;
      }
    } else this.useEnteredAddress = false;
  }
  validateUserClickEntered() {
    if (this.useEnteredAddress) {
      this.useSuggestedAddress = false;
      this.resubmitAddress = false;
      this.isAddressDisabled = true;
      if (this.isHouseHoldAddrValidated) {
        this.model.householdAddress.addressLine1 = this.userEnteredAddressLine1;
        this.model.householdAddress.city = this.userEnteredCity;
        this.model.householdAddress.zip = this.userEnteredZip;
      } else if (this.isMailingAddrValidated) {
        this.model.mailingAddress.addressLine1 = this.userEnteredMailingAddressLine1;
        this.model.mailingAddress.city = this.userEnteredMailingCity;
        this.model.mailingAddress.zip = this.userEnteredMailingZip;
      }
    } else this.isAddressDisabled = false;
  }
}
