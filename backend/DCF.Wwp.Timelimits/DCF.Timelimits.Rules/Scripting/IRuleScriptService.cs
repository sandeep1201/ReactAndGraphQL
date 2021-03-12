using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCF.Timelimits.Rules.Scripting
{
    public interface IRuleScriptService
    {
        Task<List<String>> GetScriptFilesAsync();
    }
}