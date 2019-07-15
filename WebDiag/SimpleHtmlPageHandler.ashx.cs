using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebDiag
{
    /// <summary>
    /// SimpleHtmlPageHandler 的摘要说明
    /// </summary>
    public class SimpleHtmlPageHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            DateTime reqBegin = DateTime.Now;
            context.Response.ContentType = "text/plain";

            string key = context.Request["key"];
            int rst_num = 10;
            rst_num = int.TryParse(context.Request["num"], out rst_num) ? rst_num : 10;

            int sqls = int.TryParse(context.Request["sqls"], out sqls) ? sqls : 0;
            int sqlr = int.TryParse(context.Request["sqlr"], out sqlr) ? sqlr : 0;
            bool readClob = bool.TryParse(context.Request["readc"], out readClob) ? readClob : false;
            bool readBlob = bool.TryParse(context.Request["readb"], out readBlob) ? readBlob : false;
            bool writeClob = bool.TryParse(context.Request["writec"], out writeClob) ? writeClob : false;
            bool writeBlob = bool.TryParse(context.Request["writeb"], out writeBlob) ? writeBlob : false;
            DateTime dbBegin = DateTime.Now;
            if (sqls > 0)
                DbEx.Insert(sqls, writeClob, writeBlob);
            if (sqlr > 0)
                DbEx.GetData(sqlr, readClob, readBlob);
            DateTime dbEnd = DateTime.Now;
            
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = int.MaxValue;
            context.Response.Write(
                jsSerializer.Serialize(
                    new {
                        AppBegin = reqBegin,
                        DbBegin = dbBegin, DbEnd = dbEnd, DbDuration = (Int64)((dbEnd - dbBegin).TotalMilliseconds),
                        Nodes = MakeData(key, rst_num),
                        AppEnd = DateTime.Now, AppDuration = (Int64)((DateTime.Now - reqBegin).TotalMilliseconds)
                    }));
        }


        public class Node
        {
            public int id { get; set; }
            public string name { get; set; }
            public string desc { get; set; }
            public string remark { get; set; }
        }

        static string str1 = "abcdefghijklmnopqrstuvwxyz";
        static string str2 = "chijkbmnostuvlpqrzadefgwxy";

        public object MakeData(string key, int num)
        {
            var list = new List<Node>();
            for (var i = 0; i < num; i++)
            {
                var r1 = str1.Substring(i % str1.Length, 1);
                var r2 = str2.Substring(i % str2.Length, 1);

                list.Add(new Node
                {
                    name = r1 + i + r2 + "name",
                    desc = i + "desc",
                    remark = Guid.NewGuid().ToString(),
                    id = i
                });
            }
            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(c => c.name.Contains(key)).ToList();
            }
            
            return list;
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