using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tricentis.Automation.Contract;
using Tricentis.Automation.Contract.Serialize;
using Tricentis.Automation.TCAddIns.ExecutionAddIn.Execution.Builder;
using Tricentis.TCCore.BusinessObjects.ExecutionLists;
using Tricentis.TCCore.Persistency;
using Tricentis.TCCore.Persistency.Tasks;

namespace Tricentis.TCAddIns.FloodTools.Tasks
{
    public class ExportAutomationObjectsJsonTask : Task
    {
        public override string Name => "Export AutomationObjects Json";

        public override TaskGroup Group => TaskGroup.Advanced;

        public override TaskCategory Category => FloodToolsAddIn.AddInTaskCategory;

        public override object Execute(PersistableObject obj, ITaskContext context)
        {
            var filePath = context.GetFilePath("Save AutomationObjects", false, "%TOSCA_PROJECTS%", false, "json", true);
            var jsonText = FloodToolHelper.ExecutionToJson(obj);

            File.AppendAllText(filePath, jsonText);

            return null;
        }
        
    }
}
