// tslint:disable:no-unused-variable
// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Router } from '@angular/router';
import { AppService } from './../../core/services/app.service';
import { LanguagesSection } from '../../shared/models/languages-section';
import { ValidationManager } from '../../shared/models/validation-manager';
import { empty } from 'rxjs';
import {} from 'jasmine';
import * as TypeMoq from 'typemoq';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';
const englishId = 1; // Doesn't really matter what it is when testing.
const spanishId = 2; // Just needs to be different than English ID.

describe('LanguagesSection: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;

  const model = new LanguagesSection();
  model.homeLanguageId = englishId;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
  });

  it('Empty section is not valid.', () => {
    // Given: An empty Language section.
    validationManager.resetErrors();

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Only home language set to English is not valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with only the Home language selected as English.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Home language and Read set is not valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language as English selected and Read set only.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Home language, read and write set is not valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as English and Read and Write set.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = true;
    model.isAbleToWriteHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Home language set, read, write and speak set is valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as English and Read,
    // Write and Speak set to yes.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = true;
    model.isAbleToWriteHomeLanguage = true;
    model.isAbleToSpeakHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is valid.
    expect(result.isValid).toEqual(true);
  });

  it('Home language set, read, write and speak set is valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as English and Read=no,
    // Write=yes and Speak=yes.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = false;
    model.isAbleToWriteHomeLanguage = true;
    model.isAbleToSpeakHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is valid.
    expect(result.isValid).toEqual(true);
  });

  it('Home language set, read, write and speak set is valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as English and Read=no,
    // Write=no and Speak=yes.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = false;
    model.isAbleToWriteHomeLanguage = false;
    model.isAbleToSpeakHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is valid.
    expect(result.isValid).toEqual(true);
  });

  it('Home language set, read, write and speak set to no is not valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as English and Read=no,
    // Write=no and Speak=no.
    model.homeLanguageTypeId = englishId;
    model.homeLanguageName = 'English';
    model.isAbleToReadHomeLanguage = false;
    model.isAbleToWriteHomeLanguage = false;
    model.isAbleToSpeakHomeLanguage = false;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Home language set to Spanish, read, write and speak set is not valid.', () => {
    validationManager.resetErrors();

    // Given: A Language section with the Home language selected as Spanish and Read=yes,
    // Write=yes and Speak=yes.
    model.homeLanguageTypeId = spanishId;
    model.homeLanguageName = 'Spanish';
    model.isAbleToReadHomeLanguage = true;
    model.isAbleToWriteHomeLanguage = true;
    model.isAbleToSpeakHomeLanguage = true;

    // When: The section is validated.
    const result = model.validate(validationManager, englishId);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });
});
