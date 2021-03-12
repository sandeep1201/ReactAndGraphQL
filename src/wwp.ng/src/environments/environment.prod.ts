// PROD
export const environment = {
  production: true,
  apiServer: 'https://wwp.wisconsin.gov/',
  lostPasswordURL: 'https://on.wisconsin.gov/WAMS/AccountRecoveryController?RSAction=FSI',
  requestIDURL: 'https://on.wisconsin.gov/WAMS/SelfRegController',
  showUnauthorizedSecurityDetails: false,
  showEnvLabel: false, // this is to show the backend you are pointing against
  tableauUrl: 'https://bi.wisconsin.gov/',
  tableauSiteId: 'DCF',
  isSimulatingCurrentDateEnabled: false
};
