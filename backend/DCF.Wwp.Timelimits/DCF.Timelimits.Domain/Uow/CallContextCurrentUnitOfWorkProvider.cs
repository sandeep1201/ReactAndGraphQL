using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using DCF.Common.Exceptions;
using DCF.Common.Logging;
using DCF.Core.Logging;

namespace DCF.Core.Domain.Uow
{
    /// <summary>
    /// CallContext implementation of <see cref="ICurrentUnitOfWorkProvider"/>. 
    /// This is the default implementation.
    /// </summary>
    public class CallContextCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider
    {
        private ILog Logger { get; set; }

        private const String ContextKey = "Abp.UnitOfWork.Current";

        private static readonly ConcurrentDictionary<String, IUnitOfWork> UnitOfWorkDictionary = new ConcurrentDictionary<String, IUnitOfWork>();

        public CallContextCurrentUnitOfWorkProvider()
        {
            this.Logger = LogProvider.GetLogger(this.GetType());
        }

        private static IUnitOfWork GetCurrentUow(ILog logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(CallContextCurrentUnitOfWorkProvider.ContextKey) as String;
            if (unitOfWorkKey == null)
            {
                return null;
            }

            IUnitOfWork unitOfWork;
            if (!CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on GetCurrentUow)! UnitOfWork key: " + unitOfWorkKey);
                CallContext.FreeNamedDataSlot(CallContextCurrentUnitOfWorkProvider.ContextKey);
                return null;
            }

            if (unitOfWork.IsDisposed)
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but the UOW was disposed!");
                CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                CallContext.FreeNamedDataSlot(CallContextCurrentUnitOfWorkProvider.ContextKey);
                return null;
            }

            return unitOfWork;
        }

        private static void SetCurrentUow(IUnitOfWork value, ILog logger)
        {
            if (value == null)
            {
                CallContextCurrentUnitOfWorkProvider.ExitFromCurrentUowScope(logger);
                return;
            }

            var unitOfWorkKey = CallContext.LogicalGetData(CallContextCurrentUnitOfWorkProvider.ContextKey) as String;
            if (unitOfWorkKey != null)
            {
                IUnitOfWork outer;
                if (CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out outer))
                {
                    if (outer == value)
                    {
                        logger.Warn("Setting the same UOW to the CallContext, no need to set again!");
                        return;
                    }

                    value.Outer = outer;
                }
                else
                {
                    //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on SetCurrentUow)! UnitOfWork key: " + unitOfWorkKey);
                }
            }

            unitOfWorkKey = value.Id;
            if (!CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
            {
                throw new DCFApplicationException("Can not set unit of work! UnitOfWorkDictionary.TryAdd returns false!");
            }

            CallContext.LogicalSetData(CallContextCurrentUnitOfWorkProvider.ContextKey, unitOfWorkKey);
        }

        private static void ExitFromCurrentUowScope(ILog logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(CallContextCurrentUnitOfWorkProvider.ContextKey) as String;
            if (unitOfWorkKey == null)
            {
                logger.Warn("There is no current UOW to exit!");
                return;
            }

            IUnitOfWork unitOfWork;
            if (!CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                //logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on ExitFromCurrentUowScope)! UnitOfWork key: " + unitOfWorkKey);
                CallContext.FreeNamedDataSlot(CallContextCurrentUnitOfWorkProvider.ContextKey);
                return;
            }

            CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
            if (unitOfWork.Outer == null)
            {
                CallContext.FreeNamedDataSlot(CallContextCurrentUnitOfWorkProvider.ContextKey);
                return;
            }

            //Restore outer UOW

            var outerUnitOfWorkKey = unitOfWork.Outer.Id;
            if (!CallContextCurrentUnitOfWorkProvider.UnitOfWorkDictionary.TryGetValue(outerUnitOfWorkKey, out unitOfWork))
            {
                //No outer UOW
                logger.Warn("Outer UOW key could not found in UnitOfWorkDictionary!");
                CallContext.FreeNamedDataSlot(CallContextCurrentUnitOfWorkProvider.ContextKey);
                return;
            }

            CallContext.LogicalSetData(CallContextCurrentUnitOfWorkProvider.ContextKey, outerUnitOfWorkKey);
        }

        /// <inheritdoc />
        public IUnitOfWork Current
        {
            get { return CallContextCurrentUnitOfWorkProvider.GetCurrentUow(this.Logger); }
            set { CallContextCurrentUnitOfWorkProvider.SetCurrentUow(value, this.Logger); }
        }
    }
}