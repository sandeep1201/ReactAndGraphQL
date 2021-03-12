import { Serializable } from 'src/app/shared/interfaces/serializable';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';

export class EAHouseholdFinancialsSection implements Serializable<EAHouseholdFinancialsSection> {
  hasNoIncome: boolean;
  hasNoAssets: boolean;
  hasNoVehicles: boolean;
  eaHouseHoldIncomes: EAHouseholdIncomes[];
  eaAssets: EAAssets[];
  eaVehicles: EAVehicles[];
  requestId: number;
  modifiedBy: string;
  modifiedDate: string;
  isSubmittedViaDriverFlow: boolean;

  public static create() {
    const eaHouseholdFinancials = new EAHouseholdFinancialsSection();
    eaHouseholdFinancials.requestId = 0;
    eaHouseholdFinancials.eaHouseHoldIncomes = [];
    eaHouseholdFinancials.eaAssets = [];
    eaHouseholdFinancials.eaVehicles = [];
    return eaHouseholdFinancials;
  }

  public static clone(input: any, instance: EAHouseholdFinancialsSection) {
    instance.hasNoIncome = input.hasNoIncome;
    instance.hasNoAssets = input.hasNoAssets;
    instance.hasNoVehicles = input.hasNoVehicles;
    instance.eaHouseHoldIncomes = Utilities.deserilizeChildren(input.eaHouseHoldIncomes, EAHouseholdIncomes, 0);
    instance.eaAssets = Utilities.deserilizeChildren(input.eaAssets, EAAssets, 0);
    instance.eaVehicles = Utilities.deserilizeChildren(input.eaVehicles, EAVehicles, 0);
    instance.requestId = input.requestId;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
  }

  public deserialize(input: any) {
    EAHouseholdFinancialsSection.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    if (!this.hasNoIncome) {
      if (this.eaHouseHoldIncomes == null || this.eaHouseHoldIncomes.length === 0) {
        const errArr = result.createErrorsArray('eaHouseholdIncomes');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Income');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Monthly Income');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Group Member');

        const me = result.createErrorsArrayItem(errArr);

        result.addErrorForParent(me, 'incomeType');
        result.addErrorForParent(me, 'monthlyIncome');
        result.addErrorForParent(me, 'verificationTypeId');
        result.addErrorForParent(me, 'groupMember');
      } else {
        const errArr = result.createErrorsArray('eaHouseholdIncomes');
        let atLeastOneDegreeIsNonEmpty = false;

        for (const income of this.eaHouseHoldIncomes) {
          const me = result.createErrorsArrayItem(errArr);

          if (!income.isEmpty()) {
            atLeastOneDegreeIsNonEmpty = true;
            if (income.incomeType == null || income.incomeType.trim() === '') {
              result.addErrorForParent(me, 'incomeType');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Income');
            }
            if (income.monthlyIncome == null || income.monthlyIncome.toString() === '') {
              result.addErrorForParent(me, 'monthlyIncome');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Monthly Income');
            } else if (income.monthlyIncome && (isNaN(Utilities.currencyToNumber(income.monthlyIncome)) || +income.monthlyIncome < 0)) {
              validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Monthly Income');
              result.addErrorForParent(me, 'monthlyIncome');
              result.addError('disableSave');
            }
            if (income.verificationTypeId == null || income.verificationTypeId.toString() === '') {
              result.addErrorForParent(me, 'verificationTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
            }
            if (income.groupMember == null || income.groupMember.toString() === '') {
              result.addErrorForParent(me, 'groupMember');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Group Member');
            }
          }
        }

        if (!atLeastOneDegreeIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Income');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Monthly Income');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Group Member');
          const me = errArr[0];
          result.addErrorForParent(me, 'incomeType');
          result.addErrorForParent(me, 'monthlyIncome');
          result.addErrorForParent(me, 'verificationTypeId');
          result.addErrorForParent(me, 'groupMember');
        }
      }
    }

    if (!this.hasNoAssets) {
      if (this.eaAssets == null || this.eaAssets.length === 0) {
        const errArr = result.createErrorsArray('eaAssets');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Asset');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Current Value');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Asset Owner');

        const me = result.createErrorsArrayItem(errArr);

        result.addErrorForParent(me, 'assetType');
        result.addErrorForParent(me, 'currentValue');
        result.addErrorForParent(me, 'assetVerificationTypeId');
        result.addErrorForParent(me, 'assetOwner');
      } else {
        const errArr = result.createErrorsArray('eaAssets');
        let atLeastOneDegreeIsNonEmpty = false;

        for (const asset of this.eaAssets) {
          const me = result.createErrorsArrayItem(errArr);

          if (!asset.isEmpty()) {
            atLeastOneDegreeIsNonEmpty = true;

            if (asset.assetType == null || asset.assetType.trim() === '') {
              result.addErrorForParent(me, 'assetType');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Asset');
            }

            if (asset.currentValue == null || asset.currentValue.toString() === '') {
              result.addErrorForParent(me, 'currentValue');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Current Value');
            } else if (asset.currentValue && (isNaN(Utilities.currencyToNumber(asset.currentValue)) || +asset.currentValue < 0)) {
              validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Current Value');
              result.addErrorForParent(me, 'currentValue');
              result.addError('disableSave');
            }
            if (asset.verificationTypeId == null || asset.verificationTypeId.toString() === '') {
              result.addErrorForParent(me, 'assetVerificationTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
            }
            if (asset.assetOwner == null || asset.assetOwner.toString() === '') {
              result.addErrorForParent(me, 'assetOwner');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Asset Owner');
            }
          }
        }

        if (!atLeastOneDegreeIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Asset');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Current Value');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Asset Owner');
          const me = errArr[0];
          result.addErrorForParent(me, 'assetType');
          result.addErrorForParent(me, 'currentValue');
          result.addErrorForParent(me, 'assetVerificationTypeId');
          result.addErrorForParent(me, 'assetOwner');
        }
      }
    }

    if (!this.hasNoVehicles) {
      if (this.eaVehicles == null || this.eaVehicles.length === 0) {
        const errArr = result.createErrorsArray('eaVehicles');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Vehicle');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Value');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Owner');
        const me = result.createErrorsArrayItem(errArr);
        result.addErrorForParent(me, 'vehicleType');
        result.addErrorForParent(me, 'vehicleValue');
        result.addErrorForParent(me, 'ownerVerificaionTypeId');
        result.addErrorForParent(me, 'vehicleOwner');
      } else {
        const errArr = result.createErrorsArray('eaVehicles');
        let atLeastOneVehicleIsNonEmpty = false;

        for (const vehicle of this.eaVehicles) {
          const me = result.createErrorsArrayItem(errArr);

          if (!vehicle.isEmpty()) {
            atLeastOneVehicleIsNonEmpty = true;

            if (Utilities.isStringEmptyOrNull(vehicle.vehicleType)) {
              result.addErrorForParent(me, 'vehicleType');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Vehicle');
            }
            if (Utilities.isNumberEmptyOrNull(vehicle.vehicleOwner)) {
              result.addErrorForParent(me, 'vehicleOwner');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Owner');
            }
            if (Utilities.isNumberEmptyOrNull(vehicle.ownerVerificationTypeId)) {
              result.addErrorForParent(me, 'ownerVerificaionTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
            }
            if (Utilities.isStringEmptyOrNull(vehicle.vehicleValue)) {
              result.addErrorForParent(me, 'vehicleValue');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Value');
            } else if (vehicle.vehicleValue && (isNaN(Utilities.currencyToNumber(vehicle.vehicleValue)) || +vehicle.vehicleValue < 0)) {
              validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Vehicle Value');
              result.addErrorForParent(me, 'vehicleValue');
              result.addError('disableSave');
            }
            if (Utilities.isNumberEmptyOrNull(vehicle.vehicleValueVerificationTypeId)) {
              result.addErrorForParent(me, 'vehicleValueVerificationTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
            }
            if (vehicle.owedVerificationTypeId) {
              if (Utilities.isStringEmptyOrNull(vehicle.amountOwed)) {
                result.addErrorForParent(me, 'amountOwed');
                validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Amount Owed');
              } else if (vehicle.amountOwed && (isNaN(Utilities.currencyToNumber(vehicle.amountOwed)) || +vehicle.amountOwed < 0)) {
                validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Amount Owed');
                result.addErrorForParent(me, 'amountOwed');
                result.addError('disableSave');
              }
            }
            if (vehicle.amountOwed && Utilities.isNumberEmptyOrNull(vehicle.owedVerificationTypeId)) {
              result.addErrorForParent(me, 'owedVerificaionTypeId');
              validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
            }
          }
        }

        if (!atLeastOneVehicleIsNonEmpty) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Type of Vehicle');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Value');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Verification');
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Vehicle Owner');
          const me = errArr[0];
          result.addErrorForParent(me, 'vehicleType');
          result.addErrorForParent(me, 'vehicleValue');
          result.addErrorForParent(me, 'ownerVerificaionTypeId');
          result.addErrorForParent(me, 'vehicleOwner');
        }
      }
    }
    return result;
  }
}

export class EAHouseholdIncomes implements Serializable<EAHouseholdIncomes> {
  id: number;
  incomeType: string;
  monthlyIncome: string;
  verificationTypeId: number;
  verificationTypeName: string;
  groupMember: number;

  public static create(): EAHouseholdIncomes {
    const income = new EAHouseholdIncomes();
    income.id = 0;
    return income;
  }

  public static clone(input: any, instance: EAHouseholdIncomes) {
    instance.id = input.id;
    instance.incomeType = input.incomeType;
    instance.monthlyIncome = input.monthlyIncome;
    instance.verificationTypeId = input.verificationTypeId;
    instance.verificationTypeName = input.verificationTypeName;
    instance.groupMember = input.groupMember;
  }

  public clear(): void {
    this.incomeType = null;
    this.monthlyIncome = null;
    this.verificationTypeId = null;
    this.verificationTypeName = null;
    this.groupMember = null;
  }

  /**
   * Detects whether or not a FamilyMember object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf FamilyMember
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.incomeType == null || this.incomeType.trim() === '') &&
      (this.monthlyIncome == null || this.monthlyIncome.toString() === '') &&
      (this.verificationTypeId == null || this.verificationTypeId.toString() === '') &&
      (this.verificationTypeName == null || this.verificationTypeName.trim() === '') &&
      (this.groupMember == null || this.groupMember.toString() === '')
    );
  }

  public deserialize(input: any) {
    EAHouseholdIncomes.clone(input, this);
    return this;
  }
}

export class EAAssets implements Serializable<EAAssets> {
  id: number;
  assetType: string;
  currentValue: string;
  verificationTypeId: number;
  verificationTypeName: string;
  assetOwner: number;

  public static create(): EAAssets {
    const asset = new EAAssets();
    asset.id = 0;
    return asset;
  }

  public static clone(input: any, instance: EAAssets) {
    instance.id = input.id;
    instance.assetType = input.assetType;
    instance.currentValue = input.currentValue;
    instance.verificationTypeId = input.verificationTypeId;
    instance.verificationTypeName = input.verificationTypeName;
    instance.assetOwner = input.assetOwner;
  }

  public clear(): void {
    this.assetType = null;
    this.currentValue = null;
    this.verificationTypeId = null;
    this.verificationTypeName = null;
    this.assetOwner = null;
  }

  /**
   * Detects whether or not a FamilyMember object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf FamilyMember
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.assetType == null || this.assetType.trim() === '') &&
      (this.currentValue == null || this.currentValue.toString() === '') &&
      (this.verificationTypeId == null || this.verificationTypeId.toString() === '') &&
      (this.verificationTypeName == null || this.verificationTypeName.trim() === '') &&
      (this.assetOwner == null || this.assetOwner.toString() === '')
    );
  }

  public deserialize(input: any) {
    EAAssets.clone(input, this);
    return this;
  }
}

export class EAVehicles implements Serializable<EAVehicles> {
  id: number;
  vehicleType: string;
  vehicleValue: string;
  amountOwed: string;
  vehicleEquity: string;
  ownerVerificationTypeId: number;
  ownerVerificationTypeName: string;
  owedVerificationTypeId: number;
  owedVerificationTypeName: string;
  vehicleValueVerificationTypeId: number;
  vehicalValueVerificationTypeName: string;
  vehicleOwner: number;

  public static create(): EAVehicles {
    const vehicle = new EAVehicles();
    vehicle.id = 0;
    return vehicle;
  }

  public static clone(input: any, instance: EAVehicles) {
    instance.id = input.id;
    instance.vehicleType = input.vehicleType;
    instance.vehicleValue = input.vehicleValue;
    instance.amountOwed = input.amountOwed;
    instance.ownerVerificationTypeId = input.ownerVerificationTypeId;
    instance.ownerVerificationTypeName = input.ownerVerificationTypeName;
    instance.owedVerificationTypeId = input.owedVerificationTypeId;
    instance.owedVerificationTypeName = input.owedVerificationTypeName;
    instance.vehicleValueVerificationTypeId = input.vehicleValueVerificationTypeId;
    instance.vehicalValueVerificationTypeName = input.vehicalValueVerificationTypeName;
    instance.vehicleOwner = input.vehicleOwner;
  }

  public clear(): void {
    this.vehicleType = null;
    this.vehicleValue = null;
    this.amountOwed = null;
    this.ownerVerificationTypeId = null;
    this.ownerVerificationTypeName = null;
    this.owedVerificationTypeId = null;
    this.owedVerificationTypeName = null;
    this.vehicleValueVerificationTypeId = null;
    this.vehicalValueVerificationTypeName = null;
    this.vehicleOwner = null;
  }

  /**
   * Detects whether or not a Vehicle object is effectively empty.
   *
   * @returns {boolean}
   *
   * @memberOf Vehicle
   */
  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      Utilities.isStringEmptyOrNull(this.vehicleType) &&
      Utilities.isStringEmptyOrNull(this.vehicleValue) &&
      Utilities.isStringEmptyOrNull(this.amountOwed) &&
      Utilities.isStringEmptyOrNull(this.vehicleEquity) &&
      Utilities.isNumberEmptyOrNull(this.ownerVerificationTypeId) &&
      Utilities.isStringEmptyOrNull(this.ownerVerificationTypeName) &&
      Utilities.isNumberEmptyOrNull(this.owedVerificationTypeId) &&
      Utilities.isStringEmptyOrNull(this.owedVerificationTypeName) &&
      Utilities.isNumberEmptyOrNull(this.vehicleValueVerificationTypeId) &&
      Utilities.isStringEmptyOrNull(this.vehicalValueVerificationTypeName) &&
      Utilities.isNumberEmptyOrNull(this.vehicleOwner)
    );
  }

  public deserialize(input: any) {
    EAVehicles.clone(input, this);
    return this;
  }
}
