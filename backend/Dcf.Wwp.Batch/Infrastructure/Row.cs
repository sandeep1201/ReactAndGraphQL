﻿using System.Collections.Generic;

namespace Dcf.Wwp.Batch.Infrastructure
{
    /// <summary>
    /// Used by the DelimitedFile Reader/Writer classes
    /// </summary>
    public class Row : List<string>
    {
        public string LineText { get; set; }
    }
}
