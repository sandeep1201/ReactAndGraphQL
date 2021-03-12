export class WhyReason {
  public status: boolean;
  public warnings: string[];
  public errors: string[];
  public activityEndDate?: string;
  public canAddWorkHistory?: boolean;
  public canSaveEP?: boolean;
  // canSaveWithWarnings is onlu being used to show the warning if the calculatedHouleWage is more than 50 per hour.
  public canSaveWithWarnings?: boolean;
  public static clone(input: any, instance: WhyReason) {
    instance.status = input.status;
    if (input.warnings != null) {
      instance.warnings = Array.from(input.warnings as string[]);
    }
    if (input.errors != null) {
      instance.errors = Array.from(input.errors as string[]);
    }
    if (input.canAddWorkHistory != null) {
      instance.canAddWorkHistory = input.canAddWorkHistory;
    }
    if (input.canSaveEP != null) {
      instance.canSaveEP = input.status;
    }
    if (input.canSaveWithWarnings != null) {
      instance.canSaveWithWarnings = input.canSaveWithWarnings;
    }

    instance.activityEndDate = input.activityEndDate;
  }

  public deserialize(input: any) {
    WhyReason.clone(input, this);
    return this;
  }
}
