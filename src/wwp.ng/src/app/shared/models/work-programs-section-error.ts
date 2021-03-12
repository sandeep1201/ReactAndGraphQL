
export class WorkProgramSectionError {
    isInOtherPrograms: boolean;
    workPrograms: WorkProgramError[];

    constructor() {
        this.isInOtherPrograms = false;
        this.workPrograms = [];
    }
}

export class WorkProgramError {
    startDate: boolean;
    endDate: boolean;
    contactId: boolean;
    location: boolean;
    details: boolean;
    workProgram: boolean;
    workStatus: boolean;

    constructor() {
        this.startDate = false;
        this.endDate = false;
        this.contactId = false;
        this.location = false;
        this.details = false;
        this.workProgram = false;
        this.workStatus = false;
    }
}
