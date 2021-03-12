using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using DCF.Common.Exceptions;
using DCF.Common.Extensions;
using DCF.Core.Exceptions;

// ReSharper disable once CheckNamespace
namespace Dcf.Wwp.Api.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DefaultErrorInfoConverter : IErrorInfoConverter
    {
        public ErrorInfo Convert(Exception exception)
        {
            var errorInfo = CreateErrorInfoWithoutCode(exception);

            if (exception is IHasErrorCode code)
            {
                errorInfo.Code = code.Code;
            }

            return errorInfo;
        }

        internal ErrorInfo CreateErrorInfoWithoutCode(Exception exception)
        {
            ErrorInfo errorInfo = null;

            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = (exception as AggregateException).Flatten();

                if (aggException.InnerException is UserFriendlyException ||
                    aggException.InnerException is DCFValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (exception?.InnerException is DbUpdateException)
            {
                exception = exception.InnerException;
            }

            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                errorInfo                 = new ErrorInfo(userFriendlyException.Message, userFriendlyException.Details);
            }
            else if (exception is DCFValidationException)
            {
                errorInfo = new ErrorInfo("The request is not valid!")
                            {
                                ValidationErrors = GetValidationErrorInfos(exception as DCFValidationException),
                                //Details = GetValidationErrorNarrative(exception as DCFValidationException)
                            };
            }
            else if (exception is DbEntityValidationException)
            {
                errorInfo = new ErrorInfo("The request is not valid!")
                            {
                                ValidationErrors = GetValidationErrorInfos(exception as DbEntityValidationException),
                                //Details = GetValidationErrorNarrative(exception as DbEntityValidationException)
                            };
            }
            else if (exception is EntityNotFoundException)
            {
                var entityNotFoundException = exception as EntityNotFoundException;

                if (entityNotFoundException.EntityType != null)
                {
                    return new ErrorInfo($"There is not entity {entityNotFoundException.EntityType.Name} with id = {entityNotFoundException.Id}");
                }

                errorInfo = new ErrorInfo(
                                          entityNotFoundException.Message
                                         );
            }
            else if (exception is DCFAuthorizationException authorizationException)
            {
                errorInfo = new ErrorInfo(authorizationException.Message);
            }

            if (errorInfo != null && exception is IHasObjectContent contentException)
            {
                errorInfo.Content = contentException.Content;
            }

            // create a default from exception if needed
            if (errorInfo == null)
            {
                errorInfo = CreateDetailedErrorInfoFromException(new DCFApplicationException("Something unexpected happened. An internal error occurred during your request. Please try again or contact the help desk.", exception));
            }

            if (errorInfo.Details.IsNullOrEmpty())
            {
                var sb = new StringBuilder();
                AddExceptionToDetails(exception, sb);
                errorInfo.Details = sb.ToString();
            }

            return errorInfo;
        }


        private ErrorInfo CreateDetailedErrorInfoFromException(Exception exception)
        {
            var detailBuilder = new StringBuilder();

            AddExceptionToDetails(exception, detailBuilder);

            var errorInfo = new ErrorInfo(exception.Message, detailBuilder.ToString());

            if (exception is DCFValidationException validationException)
            {
                errorInfo.ValidationErrors = GetValidationErrorInfos(validationException);
            }
            else if (exception is DbEntityValidationException)
            {
                errorInfo.ValidationErrors = GetValidationErrorInfos((DbEntityValidationException) exception);
            }

            return errorInfo;
        }

        private void AddExceptionToDetails(Exception exception, StringBuilder detailBuilder)
        {
            //Exception Message
            detailBuilder.AppendLine(exception.GetType().Name + ": " + exception.Message);

            //Additional info for UserFriendlyException
            if (exception is UserFriendlyException)
            {
                var userFriendlyException = exception as UserFriendlyException;
                if (!string.IsNullOrEmpty(userFriendlyException.Details))
                {
                    detailBuilder.AppendLine(userFriendlyException.Details);
                }
            }

            //Additional info for DCFValidationException
            if (exception is DCFValidationException)
            {
                var validationException = exception as DCFValidationException;
                if (validationException.ValidationErrors.Any())
                {
                    detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
                }
            }

            //Additional info for DCFValidationException
            if (exception is DbEntityValidationException)
            {
                var validationException = (DbEntityValidationException) exception;
                if (validationException.EntityValidationErrors?.Any() == true)
                {
                    detailBuilder.AppendLine(GetValidationErrorNarrative(validationException));
                }
            }


            //Exception StackTrace
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailBuilder.AppendLine("STACK TRACE: " + exception.StackTrace);
            }

            //Inner exception
            if (exception.InnerException != null)
            {
                AddExceptionToDetails(exception.InnerException, detailBuilder);
            }

            //Inner exceptions for AggregateException
            if (exception is AggregateException)
            {
                var aggException = ((AggregateException) exception).Flatten();
                if (aggException.InnerExceptions.IsNullOrEmpty())
                {
                    return;
                }

                foreach (var innerException in aggException.InnerExceptions)
                {
                    AddExceptionToDetails(innerException, detailBuilder);
                }
            }
        }


        private ValidationErrorInfo[] GetValidationErrorInfos(DbEntityValidationException validationException)
        {
            var validationErrorInfos = new List<ValidationErrorInfo>();

            foreach (var validationResult in validationException.EntityValidationErrors.Where(x => !x.IsValid).SelectMany(x => x.ValidationErrors))
            {
                var validationError = new ValidationErrorInfo(validationResult.PropertyName, validationResult.PropertyName);
                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        private ValidationErrorInfo[] GetValidationErrorInfos(DCFValidationException validationException)
        {
            var validationErrorInfos = new List<ValidationErrorInfo>();

            foreach (var validationResult in validationException.ValidationErrors.Where(x => !x.IsValid).SelectMany(x => x.Errors))
            {
                var validationError = new ValidationErrorInfo(validationResult.PropertyName, validationResult.PropertyName);
                validationErrorInfos.Add(validationError);
            }

            return validationErrorInfos.ToArray();
        }

        private String GetValidationErrorNarrative(DbEntityValidationException validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine("ValidationNarrativeTitle");

            foreach (var validationResult in validationException.EntityValidationErrors.Where(x => !x.IsValid).SelectMany(x => x.ValidationErrors))
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }

        private string GetValidationErrorNarrative(DCFValidationException validationException)
        {
            var detailBuilder = new StringBuilder();
            detailBuilder.AppendLine("ValidationNarrativeTitle");

            foreach (var validationResult in validationException.ValidationErrors.Where(x => !x.IsValid).SelectMany(x => x.Errors))
            {
                detailBuilder.AppendFormat(" - {0}", validationResult.ErrorMessage);
                detailBuilder.AppendLine();
            }

            return detailBuilder.ToString();
        }
    }
}
