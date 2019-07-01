using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            if (obj is ExecutionEntryFolder folder)
            {
                executionEntries = folder.GetAllExecutionEntries().Where(e => !e.Disabled).ToList();
            }

            if (obj is ExecutionList list)
            {
                executionEntries = list.AllExecutionEntries.Where(e => !e.Disabled).ToList();
            }

            if (obj is ExecutionEntry entry)
            {
                executionEntries = new List<ExecutionEntry> { entry };
            }

            executionTasks = executionEntries.Select(e => executionTaskBuilder.Build(e, true)).ToList();
            
            var jsonText = AutomationObjectsSerializer.Serialize(executionTasks);

            return jsonText;
        }
        
        public static string MessagePost(string url, string postData)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            byte[] dataBytes = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/json";
            request.ContentLength = dataBytes.Length;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
                requestBody.Close();
            }

            return MessageResponse(request);
        }
        public static string MessageGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return MessageResponse(request);
        }

        public static string MessageResponse(WebRequest request)
        {
            string result = "";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
