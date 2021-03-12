using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class AuthContract
    {
        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "token")]
        public string Token;

        [DataMember(Name = "user")]
        public UserProfileContract User;
    }
}