using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tricentis.TCCore.BusinessObjects.ExecutionLists;
using Tricentis.TCCore.Persistency;
using Tricentis.TCCore.Persistency.AddInManager;

namespace Tricentis.TCAddIns.FloodTools.Tasks.Interceptors
{
    public class ExecutionListTaskInterceptor : TaskInterceptor
    {
        public ExecutionListTaskInterceptor(ExecutionList obj) { }

        public override void GetTasks(PersistableObject obj, List<Task> tasks)
        {
            if (obj is ExecutionList)
            {
                tasks.Add(TaskFactory.Instance.GetTask<ExportAutomationObjectsJsonTask>());
                tasks.Add(TaskFactory.Instance.GetTask<ExportElementJsTask>());
            }
                
        }
    }
}
