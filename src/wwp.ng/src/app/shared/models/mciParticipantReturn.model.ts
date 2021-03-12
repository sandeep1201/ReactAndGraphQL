import * as _ from 'lodash';
import * as moment from 'moment';
import { PersonName } from './person-name.model';
import { SsnPipe } from '../pipes/ssn.pipe';
import { PadPipe } from '../pipes/pad.pipe';

import { Utilities } from '../utilities';
export class MciParticipantReturn {
    id: number;
    pin: string;
    name: PersonName;
    dateOfBirth: string;
    ssn: string;
    // isNoSsn is set by the search model on clearance.
    isNoSsn: boolean;
    ssnVerificationCode: string;
    ssnVerificationCodeDescription: string;
    gender: string;
    hasAlias: boolean;
    score: number;
    mciId: number;
    isMciKnownToCww: boolean;

    set dateOfBirthMmDdYyyy(date) {
        this.dateOfBirth = Utilities.mmDdYyyyToDateTime(date);
    }

    get dateOfBirthMmDdYyyy() {
        return Utilities.toMmDdYyyy(this.dateOfBirth);
    }

    get displayScore() {
        // Displays 99 when 100.
        if (this.score === 100) {
            return 99;
        } else {
            return this.score;
        }
    }

    public static clone(input: any, instance: MciParticipantReturn) {
        instance.id = input.id;
        instance.pin = input.pin;
        instance.name = Utilities.deserilizeChild(input.name, PersonName);
        instance.dateOfBirth = input.dateOfBirth;
        instance.ssn = input.ssn;
        instance.isNoSsn = input.isNoSsn;
        instance.ssnVerificationCode = input.ssnVerificationCode;
        instance.ssnVerificationCodeDescription = input.ssnVerificationCodeDescription;
        instance.gender = input.gender;
        instance.hasAlias = input.hasAlias;
        instance.score = input.score;
        instance.mciId = input.mciId;
        instance.isMciKnownToCww = input.isMciKnownToCww;
    }

    public deserialize(input: any) {
        MciParticipantReturn.clone(input, this);
        return this;
    }

    get fullNameWithMiddleInitialSuffixTitleCase(): string {
        if (this.name != null) {
            return Utilities.formatDisplayPersonName(this.name.firstName, this.name.middleInitial, this.name.lastName, this.name.suffix);
        }
    }

    get displaySsn(): string {
        if (this.ssn != null) {
            if (+this.ssn === 0) {
                return 'No SSN';
            } else {
                // Pad first before formatting.
                const pp = new PadPipe();
                const paddedSsn = pp.transform(this.ssn, 9);
                const sp = new SsnPipe();
                return sp.transform(paddedSsn);
            }
        } else if (this.isNoSsn) {
            return 'No SSN';
        }
    }

}
