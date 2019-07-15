using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDiag
{
    /// <summary>
    /// LongTimeResponseHandler 的摘要说明
    /// </summary>
    public class LongTimeResponseHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Server.ScriptTimeout = 600;
            context.Response.Write("Hello 111");
            context.Response.Flush();

            //context.Response.Redirect("", false);
            //context.Response.End();
            //context.ApplicationInstance.CompleteRequest();
            //context.Server.Transfer("");
            //context.Server.Execute("");

            for (int i = 1; i <= 60; i++)
            {
                //context.Response.Write(string.Format("Hello tkk {0}", i));
                //context.Response.Flush();
                DbEx.Insert(1, false, false);

                System.Threading.Thread.Sleep(1000);
            }

            context.Response.Write("Hello World End");
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