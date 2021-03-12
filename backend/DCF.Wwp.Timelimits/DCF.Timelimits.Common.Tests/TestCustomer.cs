using System;

namespace DCF.Timelimits.Common.Tests
{
    public class TestCustomer
    {
        public virtual String Name { get;  set; }

        public virtual Byte Age { get; set; }

        public virtual Int64 Balance { get; set; }

        public virtual String Location { get; set; }

        public TestCustomer() {}
        public TestCustomer(String name, Byte age, Int64 balance, String location)
        {
            this.Name = name;
            this.Age = age;
            this.Balance = balance;
            this.Location = location;
        }
    }
}