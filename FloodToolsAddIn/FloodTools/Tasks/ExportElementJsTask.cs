using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript.V8;
using Tricentis.Automation.Contract.Serialize;
using Tricentis.Automation.TCAddIns.ExecutionAddIn.Execution.Builder;
using Tricentis.TCCore.BusinessObjects.ExecutionLists;
using Tricentis.TCCore.Persistency;
using Tricentis.TCCore.Persistency.Tasks;

namespace Tricentis.TCAddIns.FloodTools.Tasks
{
    public class ExportElementJsTask : Task
    {
        public override string Name => "Export Element Script";

        public override TaskGroup Group => TaskGroup.Advanced;

        public override TaskCategory Category => FloodToolsAddIn.AddInTaskCategory;

        public override object Execute(PersistableObject obj, ITaskContext context)
        {
            var dllDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var tosca2ElementScript = File.ReadAllText(dllDir + @"\Tricentis.FloodTools.tosca2element.js");
            var toscaJson = FloodToolHelper.ExecutionToJson(obj);
            var element = "";

            using (var engine = new V8ScriptEngine())
            {
                engine.Execute("window = {};");
                engine.Evaluate(tosca2ElementScript);
                element = engine.Script.window.tosca2element(toscaJson);
            }

            var filePath = context.GetFilePath("Save Element Script", false, "%TOSCA_PROJECTS%", false, "ts", true);
            File.WriteAllText(filePath, element);

            return null;
        }
    }
}
