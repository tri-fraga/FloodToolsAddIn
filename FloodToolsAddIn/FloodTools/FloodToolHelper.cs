using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.Contract;
using Tricentis.Automation.Contract.Serialize;
using Tricentis.Automation.TCAddIns.ExecutionAddIn.Execution.Builder;
using Tricentis.TCCore.BusinessObjects.ExecutionLists;
using Tricentis.TCCore.Persistency;
using ExecutionEntry = Tricentis.TCCore.BusinessObjects.ExecutionLists.ExecutionEntry;

namespace Tricentis.TCAddIns.FloodTools
{
    public class FloodToolHelper
    {

        public static string ExecutionToJson(PersistableObject obj)
        {
            var executionTaskBuilder = new ExecutionTaskBuilder();
            var executionEntries = new List<ExecutionEntry>();
            var executionTasks = new List<ExecutionTask>();

            if (obj is ExecutionList list)
            {
                executionEntries = list.AllExecutionEntries.Where(e => !e.Disabled).ToList();
            }

            if (obj is ExecutionEntry exeObj)
            {
                executionEntries = new List<ExecutionEntry>() { exeObj };
            }

            executionTasks = executionEntries.Select(entry => executionTaskBuilder.Build(entry, true)).ToList();
            
            var jsonText = AutomationObjectsSerializer.Serialize(executionTasks);

            return jsonText;
        }
    }
}
