using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDiag
{
    public partial class SimpleWebForm : System.Web.UI.Page
    {
        private static int refresh_count = 0;
        private static DataTable tb = new DataTable();

        private DateTime time;

        protected void Page_Load(object sender, EventArgs e)
        {
            time = DateTime.Now;

            if (IsPostBack == false)
            {
                if (tb.Columns.Count <= 0)
                {
                    tb.Columns.Add("Id", typeof(int));
                    tb.Columns.Add("Time", typeof(string));
                    tb.Columns.Add("Guid", typeof(string));
                    tb.Columns.Add("Elapsed", typeof(double));
                }

                refresh_count = 0;
                tb.Rows.Clear();

                int elapsed = (DateTime.Now - time).Milliseconds;
                Response.Write(string.Format("CurrentTime:{0}, Elapsed: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), elapsed));

            }

        }


        
        protected void Button1_Click(object sender, EventArgs e)
        {
            refresh_count++;
            int elapsed = (DateTime.Now - time).Milliseconds;
            Response.Write(string.Format("CurrentTime:{0}, Elapsed: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), elapsed));
            tb.Rows.Add(refresh_count, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), Guid.NewGuid().ToString(), elapsed);

            this.GridView1.DataSource = tb;
            this.GridView1.DataBind();
        }
    }
}