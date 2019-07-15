using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDiag
{
    public class PerfData
    {
        public string Id { get; set; }

        public string ClientName { get; set; }

        public DateTime LogDate { get; set; }

        public int ClientDuration { get; set; }

        public int AppDuration { get; set; }

        public int DbDuration { get; set; }

        public int ClientRequests { get; set; }

        public int AppResponds { get; set; }

        public int AppRequests { get; set; }

        public int DbResponds { get; set; }


        public int ReadClob { get; set; }

        public int ReadBlob { get; set; }

        public int WriteClob { get; set; }

        public int WriteBlob { get; set; }


        public int MaxClobLength { get; set; }
        public int MaxBlobLength { get; set; }
        public int RandomLobLength { get; set; }
    }
}