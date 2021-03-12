using System.Text;
using System.Threading.Tasks;
namespace DCF.Core.Plugins
{
    public interface IApplicationPluginManager
    {
        IPlugInSourceList PlugInSources { get; }
    }
}
