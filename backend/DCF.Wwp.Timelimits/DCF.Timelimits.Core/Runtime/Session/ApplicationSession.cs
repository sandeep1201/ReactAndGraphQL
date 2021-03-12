using System;

namespace DCF.Core.Runtime.Session
{
    public class DefaultApplicationSession : IApplicationSession
    {
        private static DefaultApplicationSession _instance;
        static DefaultApplicationSession()
        {
            DefaultApplicationSession._instance = new DefaultApplicationSession();
        }

        public static DefaultApplicationSession Instance
        {
            get { return DefaultApplicationSession._instance; }
        }

        public Int64? UserId {get; set;}
        public String Username { get; set; }

        public Int64? ImpersonatorUserId { get; set; }

        public DefaultApplicationSession(String username = null, Int64? userId = null, Int64? impersonatorUserId = null) {
            this.UserId = userId;
            this.Username = username;
            this.ImpersonatorUserId = impersonatorUserId;
        }
    }
}
