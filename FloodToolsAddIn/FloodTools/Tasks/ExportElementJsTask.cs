using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Tricentis.Automation.Contract.Serialize;
using Tricentis.Automation.TCAddIns.ExecutionAddIn.Execution.Builder;
using Tricentis.TCCore.BusinessObjects.ExecutionLists;
using Tricentis.TCCore.Persistency;
using Tricentis.TCCore.Persistency.Tasks;

namespace Tricentis.TCAddIns.FloodTools.Tasks
{
    public class ExportElementJsTask : Task
    {
        public static readonly string Tosca2ElementUrl = "https://tosca2element.flood.io";

        public override string Name => "Export Element Script";

        public override TaskGroup Group => TaskGroup.Advanced;

        public override TaskCategory Category => FloodToolsAddIn.AddInTaskCategory;

        public override object Execute(PersistableObject obj, ITaskContext context)
        {
            var filePath = context.GetFilePath("Save ElementJs", false, "%TOSCA_PROJECTS%", false, "ts", true);

            context.ShowWaitCursor();
            context.ShowProgressInfo(100, 10, "Generating Json");
            var toscaJson = FloodToolHelper.ExecutionToJson(obj);

            try
            {
                context.ShowProgressInfo(100, 40, "Generating Element Script");
                var jsonPostResponse = FloodToolHelper.MessagePost(Tosca2ElementUrl + "/generate", toscaJson);
                dynamic postResponse = JsonConvert.DeserializeObject(jsonPostResponse);
                var downloadPath = postResponse.scripts[0];

                context.ShowProgressInfo(100, 70, "Downloading Element Script");
                var elementJs = FloodToolHelper.MessageGet(Tosca2ElementUrl + downloadPath);

                context.ShowProgressInfo(100, 90, "Saving Element Script");
                File.AppendAllText(filePath, elementJs);

                context.ShowProgressInfo(100, 100, "Element Script successfully generated");
            }
            catch (Exception e)
            {
                context.ShowErrorMessage("Error", "The element script could not be generated. " +
                                                  "Check your internet conection or try it manually at https://tosca2element.flood.io \r\n" +
                                                  "Error: '" + e.Message + "'");
            }

            context.HideWaitCursor();

            return null;
        }


    }
}
