using System;
using System.Collections.Generic;
using System.Linq;
using DCF.Core;

namespace DCF.Timelimits.Rules.Domain
{
    public class ExtensionSequence
    {
        public readonly List<Extension> Extensions = new List<Extension>();
        private Extension _currentExtension = null;

        public ExtensionSequence(Int32 sequenceId, IEnumerable<Extension> extensions)
        {
            this.SequenceId = sequenceId;
            this.Extensions.AddRange(extensions);
            this.ClockType = this.Extensions.Select(x => x.ClockType).FirstOrDefault();
        }

        public Int32 SequenceId { get; set; }
        public ClockTypes ClockType { get; set; }

        public Extension CurrentExtension
        {
            get { return this._currentExtension ?? (this._currentExtension = this.Extensions.Where(x => !x.HasElapsed).GetMax(x => x.DecisionDate)); }
        }

        public IReadOnlyList<Extension> GetAllExtensions()
        {
            return this.Extensions.AsReadOnly();
        }
    }
}