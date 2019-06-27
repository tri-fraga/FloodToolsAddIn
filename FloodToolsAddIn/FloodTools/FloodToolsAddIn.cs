using Tricentis.TCCore.Persistency;
using Tricentis.TCCore.Persistency.AddInManager;

namespace Tricentis.TCAddIns.FloodTools
{
    public class FloodToolsAddIn : TCAddIn
    {
        public override string UniqueName => "FloodTools";
        public override string DisplayedName => "Flood Tools";

        public static readonly TaskGroup AddInTaskGroup = new TaskGroup("Flood Tools", true);

        public static readonly TaskCategory AddInTaskCategory = new TaskCategory("FloodTools");

    }
}
