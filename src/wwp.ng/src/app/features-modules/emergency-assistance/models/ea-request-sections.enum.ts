export enum EARequestSections {
  Demographics = 'demographics',
  Emergency = 'type-of-emergency',
  Members = 'household-members',
  Financials = 'household-financials',
  AgencySummary = 'agency-summary'
}

export enum EAStatusCodes {
  Approved = 'AP',
  InProgress = 'IP',
  Pending = 'PN',
  WithDrawn = 'WD',
  Denied = 'DN'
}

export enum EAEmergencyCodes {
  ImpendingHomelessness = 'IHL',
  Homelessness = 'HLN',
  EnergyCrisis = 'ENC',
  // Initiated Drop
  ACCESS = 'AC'
}

export enum EAIndividualType {
  CaretakerRelative = 'CTR',
  OtherCaretakerRelative = 'OCTR',
  DependentChild = 'DC'
}

export enum EAIPVStatus {
  Pending = 'PN',
  Active = 'AC',
  Expired = 'EX',
  Overturned = 'OSM'
}

export enum EAViewModes {
  View = 'view',
  Edit = 'edit'
}
