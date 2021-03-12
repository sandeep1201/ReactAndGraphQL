export interface SectionComponent {
  validate(): void;
  isValid(): boolean;
  scrollToTop(): any;
  prepareToSaveWithErrors(): any;
  openHelp();
  refreshModel();
  modifiedTrackerForcedValidation();
}
