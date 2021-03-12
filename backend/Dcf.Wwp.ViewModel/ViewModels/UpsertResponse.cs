using System;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class UpsertResponse<T>
    {
        public bool HasConcurrencyError { get; set; }
        public T UpdatedModel { get; set; }

        public UpsertResponse()
        {
            // Start out assuming there is no error.
            HasConcurrencyError = false;
        }
        public String ErrorMessage { get; set; }
    }
}