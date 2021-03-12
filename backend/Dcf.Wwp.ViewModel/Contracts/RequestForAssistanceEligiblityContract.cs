using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class RequestForAssistanceEligiblityContract
    {
        public int                         RfaId            { get; set; }
        public bool                        IsEligible       { get; set; }
        public List<ServerMessageContract> ServerMessages   { get; set; }
        public string                      EligibilityCodes { get; set; }

        public RequestForAssistanceEligiblityContract()
        {
            ServerMessages = new List<ServerMessageContract>();
        }

        public void HandleIneligibility(string code, string message)
        {
            IsEligible = true;
            ServerMessages.Add(new ServerMessageContract { Code = code, Message = message });
        }
    }

    public class ServerMessageContract
    {
        public string Code    { get; set; }
        public string Message { get; set; }
    }
}
