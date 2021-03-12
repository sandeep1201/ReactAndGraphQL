// tslint:disable: no-output-on-prefix
import { Input, Output, EventEmitter } from '@angular/core';

import { GoogleLocation } from '../../shared/models/google-location';
import { InformalAssessment } from '../../shared/models/informal-assessment';
import * as _ from 'lodash';
import { Utilities } from '../utilities';

const noop = () => {};

export class GenericBaseComponent<T> {
  public innerValue: T = null;
  public onTouchedCallback: () => void = noop;
  public onChangeCallback: (_: T) => void = noop;
}

export abstract class BaseComponent extends GenericBaseComponent<any> {
  readonly languagesSectionName: string = InformalAssessment.LanguagesSectionName;
  readonly workHistorySectionName: string = InformalAssessment.WorkHistorySectionName;
  readonly workProgramsSectionName: string = InformalAssessment.WorkProgramsSectionName;
  readonly educationHistorySectionName: string = InformalAssessment.EducationHistorySectionName;
  readonly postSecondaryEducationSectionName: string = InformalAssessment.PostSecondaryEducationSectionName;
  readonly militaryTrainingSectionName: string = InformalAssessment.MilitaryTrainingSectionName;
  readonly housingSectionName: string = InformalAssessment.HousingSectionName;
  readonly transportationSectionName: string = InformalAssessment.TransportationSectionName;
  readonly legalIssuesSectionName: string = InformalAssessment.LegalIssuesSectionName;
  readonly participantBarriersSectionName: string = InformalAssessment.ParticipantBarriersSectionName;
  readonly childYouthSupportsSectionName: string = InformalAssessment.ChildYouthSupportsSectionName;
  readonly familyBarriersSectionName: string = InformalAssessment.FamilyBarriersSectionName;
  readonly nonCustodialParentsSectionName: string = InformalAssessment.NonCustodialParentsSectionName;
  readonly nonCustodialParentsReferralSectionName: string = InformalAssessment.NonCustodialParentsReferralSectionName;

  @Output() onDirty = new EventEmitter<boolean>();

  public cachedInnerValue: number | string | GoogleLocation | number[];
  private isValueCached = false;

  @Input('cacheInnerValue')
  set cacheInnerValue(value: any) {
    this.cachedInnerValue = value;
    this.isValueCached = true;
  }

  get isModified(): boolean {
    let isModified = false;

    // Return if cachced value is not set.
    if (!this.isValueCached) {
      return false;
    }

    // If cachedModel is set then we can detect dirty.
    let innerValue = this.innerValue;
    let cachedInnerValue = this.cachedInnerValue;
    // if (typeof innerValue === 'object') {
    //     console.log(cachedInnerValue);
    //     console.log(innerValue);
    // }

    // Null is same as empty string.
    if (innerValue == null && !Array.isArray(innerValue)) {
      innerValue = '';
    }
    if (cachedInnerValue == null && !Array.isArray(cachedInnerValue)) {
      cachedInnerValue = '';
    }

    // Numbers that are strings are same as numbers that are numbers.
    if (+this.innerValue !== NaN && this.innerValue != null && !Array.isArray(innerValue) && typeof innerValue !== 'object') {
      innerValue = this.innerValue.toString();
    }
    if (+this.cachedInnerValue !== NaN && this.cachedInnerValue != null && !Array.isArray(cachedInnerValue) && typeof cachedInnerValue !== 'object') {
      cachedInnerValue = this.cachedInnerValue.toString();
    }

    // Sort both arrays.
    if (cachedInnerValue != null && Array.isArray(cachedInnerValue)) {
      cachedInnerValue = Array.from(cachedInnerValue);
      cachedInnerValue = cachedInnerValue.sort();
    }

    if (innerValue != null && Array.isArray(innerValue)) {
      innerValue = Array.from(innerValue);
      innerValue = innerValue.sort();
    }
    // if (typeof innerValue === 'object') {
    //     console.log(cachedInnerValue);
    //     console.log(innerValue);
    // }
    if (JSON.stringify(cachedInnerValue) !== JSON.stringify(innerValue)) {
      isModified = true;
    } else {
      isModified = false;
    }
    // console.log(isModified);
    this.onDirty.emit(isModified);
    return isModified;
  }

  public isStringEmptyOrNull(str: string) {
    return Utilities.isStringEmptyOrNull(str);
  }
}
