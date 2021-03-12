// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `angular-cli.json`.

//   ACC: https://wwpacc.wisconsin.gov/
//   DEV: http://wwpdev.wisconsin.gov/
// local: http://localhost:11001/

export const environment = {
  production: false,
  apiServer: 'http://localhost:11001/',
  lostPasswordURL: 'https://devon.wisconsin.gov/WAMS/AccountRecoveryController?RSAction=FSI',
  requestIDURL: 'https://devon.wisconsin.gov/WAMS/SelfRegController',
  showUnauthorizedSecurityDetails: true,
  showEnvLabel: true,
  tableauUrl: 'https://devdoaenterprisebi.wi.gov/',
  tableauSiteId: 'DEV-DCF',
  isSimulatingCurrentDateEnabled: true
};
