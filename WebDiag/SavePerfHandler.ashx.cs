using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace WebDiag
{
    /// <summary>
    /// SavePerfHandler 的摘要说明
    /// </summary>
    public class SavePerfHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";


            string key = context.Request.Form["content"];
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PerfData));
                MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(key));
                PerfData data = serializer.ReadObject(mStream) as PerfData;
                data.Id = Guid.NewGuid().ToString();
                data.LogDate = DateTime.Now;

                DbEx.SavePerf(data);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}