using System;
using System.Reflection;
using System.Transactions;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// This attribute is used to indicate that declaring method is atomic and should be considered as a unit of work.
    /// A method that has this attribute is intercepted, a database connection is opened and a transaction is started before call the method.
    /// At the end of method call, transaction is committed and all changes applied to the database if there is no exception,
    /// otherwise it's rolled back. 
    /// </summary>
    /// <remarks>
    /// This attribute has no effect if there is already a unit of work before calling this method, if so, it uses the same transaction.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// Scope option.
        /// </summary>
        public TransactionScopeOption? Scope { get; set; }

        /// <summary>
        /// Is this UOW transactional?
        /// Uses default value if not supplied.
        /// </summary>
        public Boolean? IsTransactional { get; set; }

        /// <summary>
        /// Timeout of UOW As milliseconds.
        /// Uses default value if not supplied.
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// If this UOW is transactional, this option indicated the isolation level of the transaction.
        /// Uses default value if not supplied.
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// Used to prevent starting a unit of work for the method.
        /// If there is already a started unit of work, this property is ignored.
        /// Default: false.
        /// </summary>
        public Boolean IsDisabled { get; set; }

        /// <summary>
        /// Creates a new UnitOfWorkAttribute object.
        /// </summary>
        public UnitOfWorkAttribute()
        {

        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// </summary>
        /// <param name="isTransactional">
        /// Is this unit of work will be transactional?
        /// </param>
        public UnitOfWorkAttribute(Boolean isTransactional)
        {
            this.IsTransactional = isTransactional;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// </summary>
        /// <param name="timeout">As milliseconds</param>
        public UnitOfWorkAttribute(Int32 timeout)
        {
            this.Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// </summary>
        /// <param name="isTransactional">Is this unit of work will be transactional?</param>
        /// <param name="timeout">As milliseconds</param>
        public UnitOfWorkAttribute(Boolean isTransactional, Int32 timeout)
        {
            this.IsTransactional = isTransactional;
            this.Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            this.IsTransactional = true;
            this.IsolationLevel = isolationLevel;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="isolationLevel">Transaction isolation level</param>
        /// <param name="timeout">Transaction  timeout as milliseconds</param>
        public UnitOfWorkAttribute(IsolationLevel isolationLevel, Int32 timeout)
        {
            this.IsTransactional = true;
            this.IsolationLevel = isolationLevel;
            this.Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="scope">Transaction scope</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope)
        {
            this.IsTransactional = true;
            this.Scope = scope;
        }

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkAttribute"/> object.
        /// <see cref="IsTransactional"/> is automatically set to true.
        /// </summary>
        /// <param name="scope">Transaction scope</param>
        /// <param name="timeout">Transaction  timeout as milliseconds</param>
        public UnitOfWorkAttribute(TransactionScopeOption scope, Int32 timeout)
        {
            this.IsTransactional = true;
            this.Scope = scope;
            this.Timeout = TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Gets UnitOfWorkAttribute for given method or null if no attribute defined.
        /// </summary>
        /// <param name="methodInfo">Method to get attribute</param>
        /// <returns>The UnitOfWorkAttribute object</returns>
        internal static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MemberInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(typeof(UnitOfWorkAttribute), false);
            if (attrs.Length > 0)
            {
                return (UnitOfWorkAttribute)attrs[0];
            }

            if (UnitOfWorkHelper.IsConventionalUowClass(methodInfo.DeclaringType))
            {
                return new UnitOfWorkAttribute(); //Default
            }

            return null;
        }

        internal UnitOfWorkOptions CreateOptions()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = this.IsTransactional,
                IsolationLevel = this.IsolationLevel,
                Timeout = this.Timeout,
                Scope = this.Scope
            };
        }
    }
}