using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

using System.Data.OracleClient;
using ManagedOralceConnection = Oracle.ManagedDataAccess.Client.OracleConnection;
using ManagedOracleCommand = Oracle.ManagedDataAccess.Client.OracleCommand;
using ManagedOracleDataReader = Oracle.ManagedDataAccess.Client.OracleDataReader;
using System.Diagnostics;
using Dapper;


namespace WebDiag
{
    public class DbEx
    {
        internal static void Insert(int num, bool writeClob, bool writeBlob)
        {
            Stopwatch sw = new Stopwatch();
            
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
            {
                switch (ConfigurationManager.AppSettings["dbtype"].ToUpper().Trim())
                {
                    case "SYSTEM.DATA.ORACLECLIENT":
                        using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                        {
                            OracleCommand cmd = conn.CreateCommand();
                            conn.Open();
                            cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                            object tableInited = cmd.ExecuteScalar();
                            if (tableInited == null || tableInited == DBNull.Value)
                            {
                                cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();

                            cmd.CommandText = string.Format("insert into TEMP_TestTKK0719(code, name, age, birthday, salary, summary, created {0} {1}) values(:code, :name, :age, :birthday, :salary, :summary, sysdate {2} {3})", (writeClob ? ", remark" : string.Empty), (writeBlob ? ", extends" : string.Empty), (writeClob ? ", :remark" : string.Empty), (writeBlob ? ", :extends" : string.Empty));
                            cmd.Parameters.Add(":code", Guid.NewGuid().ToString());
                            cmd.Parameters.Add(":name", StringUtils.GetRandomString(rnd.Next(10, 50), false, true, true, false, string.Empty));
                            cmd.Parameters.Add(":age", rnd.Next(20, 65));
                            cmd.Parameters.Add(":birthday", DateTime.Now.AddYears(-1 * rnd.Next(20, 60)));
                            cmd.Parameters.Add(":salary", Math.Round(50000 * rnd.NextDouble(), 2));
                            cmd.Parameters.Add(":summary", new string('x', rnd.Next(10, 200)));

                            if (writeClob)
                                cmd.Parameters.Add(":remark", bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('y', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["clobMax"]))) : new string('y', int.Parse(ConfigurationManager.AppSettings["clobMax"])));
                            if (writeBlob)
                                cmd.Parameters.Add(":extends", Encoding.Default.GetBytes(bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('z', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["blobMax"]))) : new string('z', int.Parse(ConfigurationManager.AppSettings["blobMax"]))));

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        break;
                    case "ODP.NET":
                        using (IDbConnection conn = ODPClientFactory.CreateConnection())
                        {
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
                            IDbCommand cmd = conn.CreateCommand();
                            conn.Open();
                            cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                            object tableInited = cmd.ExecuteScalar();
                            if (tableInited == null || tableInited == DBNull.Value)
                            {
                                cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();

                            cmd.CommandText = string.Format("insert into TEMP_TestTKK0719(code, name, age, birthday, salary, summary, created {0} {1}) values(:code, :name, :age, :birthday, :salary, :summary, sysdate {2} {3})", (writeClob ? ", remark" : string.Empty), (writeBlob ? ", extends" : string.Empty), (writeClob ? ", :remark" : string.Empty), (writeBlob ? ", :extends" : string.Empty));
                            //cmd.Parameters.Add(":code", Guid.NewGuid().ToString());
                            var p0 = cmd.CreateParameter();
                            p0.DbType = DbType.String;
                            p0.ParameterName = ":code";
                            p0.Value = Guid.NewGuid().ToString();
                            cmd.Parameters.Add(p0);

                            //cmd.Parameters.Add(":name", StringUtils.GetRandomString(rnd.Next(10, 50), false, true, true, false, string.Empty));
                            var p1 = cmd.CreateParameter();
                            p1.DbType = DbType.String;
                            p1.ParameterName = ":name";
                            p1.Value = StringUtils.GetRandomString(rnd.Next(10, 50), false, true, true, false, string.Empty);
                            cmd.Parameters.Add(p1);

                            //cmd.Parameters.Add(":age", rnd.Next(20, 65));
                            var p2 = cmd.CreateParameter();
                            p2.DbType = DbType.Int32;
                            p2.ParameterName = ":age";
                            p2.Value = rnd.Next(20, 65);
                            cmd.Parameters.Add(p2);

                            //cmd.Parameters.Add(":birthday", DateTime.Now.AddYears(-1 * rnd.Next(20, 60)));
                            var p3 = cmd.CreateParameter();
                            p3.DbType = DbType.DateTime;
                            p3.ParameterName = ":birthday";
                            p3.Value = DateTime.Now.AddYears(-1 * rnd.Next(20, 60));
                            cmd.Parameters.Add(p3);

                            //cmd.Parameters.Add(":salary", Math.Round(50000 * rnd.NextDouble(), 2));
                            var p4 = cmd.CreateParameter();
                            p4.DbType = DbType.Double;
                            p4.ParameterName = ":salary";
                            p4.Value = Math.Round(50000 * rnd.NextDouble(), 2);
                            cmd.Parameters.Add(p4);

                            //cmd.Parameters.Add(":summary", new string('x', rnd.Next(10, 200)));
                            var p5 = cmd.CreateParameter();
                            p5.DbType = DbType.String;
                            p5.ParameterName = ":summary";
                            p5.Value = new string('x', rnd.Next(10, 200));
                            cmd.Parameters.Add(p5);

                            if (writeClob)
                            {
                                //cmd.Parameters.Add(":remark", bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('y', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["clobMax"]))) : new string('y', int.Parse(ConfigurationManager.AppSettings["clobMax"])));
                                var p7 = cmd.CreateParameter();
                                p7.ParameterName = ":remark";
                                p7.DbType = DbType.String;
                                p7.Value = bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('y', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["clobMax"]))) : new string('y', int.Parse(ConfigurationManager.AppSettings["clobMax"]));
                                cmd.Parameters.Add(p7);
                            }

                            if (writeBlob)
                            {
                                //cmd.Parameters.Add(":extends", Encoding.Default.GetBytes(bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('z', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["blobMax"]))) : new string('z', int.Parse(ConfigurationManager.AppSettings["blobMax"]))));
                                var p8 = cmd.CreateParameter();
                                p8.ParameterName = ":extends";
                                p8.DbType = DbType.Binary;
                                p8.Value = Encoding.Default.GetBytes(bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('z', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["blobMax"]))) : new string('z', int.Parse(ConfigurationManager.AppSettings["blobMax"])));
                                cmd.Parameters.Add(p8);
                            }
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        break;
                    case "ODAC":
                        using (ManagedOralceConnection conn = new ManagedOralceConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                        {
                            ManagedOracleCommand cmd = conn.CreateCommand();
                            conn.Open();
                            cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                            object tableInited = cmd.ExecuteScalar();
                            if (tableInited == null || tableInited == DBNull.Value)
                            {
                                cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();

                            cmd.CommandText = string.Format("insert into TEMP_TestTKK0719(code, name, age, birthday, salary, summary, created {0} {1}) values(:code, :name, :age, :birthday, :salary, :summary, sysdate {2} {3})", (writeClob ? ", remark" : string.Empty), (writeBlob ? ", extends" : string.Empty), (writeClob ? ", :remark" : string.Empty), (writeBlob ? ", :extends" : string.Empty));
                            cmd.Parameters.Add(":code", Guid.NewGuid().ToString());
                            cmd.Parameters.Add(":name", StringUtils.GetRandomString(rnd.Next(10, 50), false, true, true, false, string.Empty));
                            cmd.Parameters.Add(":age", rnd.Next(20, 65));
                            cmd.Parameters.Add(":birthday", DateTime.Now.AddYears(-1 * rnd.Next(20, 60)));
                            cmd.Parameters.Add(":salary", Math.Round(50000 * rnd.NextDouble(), 2));
                            cmd.Parameters.Add(":summary", new string('x', rnd.Next(10, 200)));

                            if (writeClob)
                                cmd.Parameters.Add(":remark", bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('y', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["clobMax"]))) : new string('y', int.Parse(ConfigurationManager.AppSettings["clobMax"])));
                            if (writeBlob)
                                cmd.Parameters.Add(":extends", Encoding.Default.GetBytes(bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('z', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["blobMax"]))) : new string('z', int.Parse(ConfigurationManager.AppSettings["blobMax"]))));

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        break;
                    default:
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                        {
                            SqlCommand cmd = conn.CreateCommand();
                            conn.Open();
                            cmd.CommandText = string.Format("select 1 from sys.tables t where t.name = 'TEMP_TestTKK0719'");
                            object tableInited = cmd.ExecuteScalar();
                            if (tableInited == null || tableInited == DBNull.Value)
                            {
                                cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
	                                                    code		varchar(50) NOT NULL,
	                                                    name		varchar(100) NULL,
	                                                    age			int   NULL,
	                                                    birthday	datetime   NULL,
	                                                    salary		decimal(10, 2) NULL,
	                                                    summary		nvarchar(1000) NULL,
	                                                    remark		varchar(max) NULL,
	                                                    extends		varbinary(max) NULL,
                                                        created     datetime,
	                                                    CONSTRAINT PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                    )";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();

                            cmd.CommandText = string.Format("insert into TEMP_TestTKK0719(code, name, age, birthday, salary, summary, created {0} {1}) values(@code, @name, @age, @birthday, @salary, @summary, getdate() {2} {3})", (writeClob ? ", remark" : string.Empty), (writeBlob ? ", extends" : string.Empty), (writeClob ? ", @remark" : string.Empty), (writeBlob ? ", @extends" : string.Empty));
                            cmd.Parameters.AddWithValue("@code", Guid.NewGuid().ToString());
                            cmd.Parameters.AddWithValue("@name", StringUtils.GetRandomString(rnd.Next(10, 50), false, true, true, false, string.Empty));
                            cmd.Parameters.AddWithValue("@age", rnd.Next(20, 65));
                            cmd.Parameters.AddWithValue("@birthday", DateTime.Now.AddYears(-1 * rnd.Next(20, 60)));
                            cmd.Parameters.AddWithValue("@salary", Math.Round(50000 * rnd.NextDouble(), 2));
                            cmd.Parameters.AddWithValue("@summary", new string('x', rnd.Next(10, 200)));

                            if (writeClob)
                                cmd.Parameters.AddWithValue("@remark", bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('y', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["clobMax"]))) : new string('y', int.Parse(ConfigurationManager.AppSettings["clobMax"])));
                            if (writeBlob)
                                cmd.Parameters.AddWithValue("@extends", Encoding.Default.GetBytes(bool.Parse(ConfigurationManager.AppSettings["randomLob"]) ? new string('z', rnd.Next(1, int.Parse(ConfigurationManager.AppSettings["blobMax"]))) : new string('z', int.Parse(ConfigurationManager.AppSettings["blobMax"]))));

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                        break;
                }
            }
        }

        internal static void GetData(int num, bool readClob, bool readBlob)
        {

            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
            //{
            //    DataTable table = new DataTable();
            //    SqlDataAdapter adp = new SqlDataAdapter(string.Format("select top {0} * from TEMP_TestTKK0719-- order by newid()", num), conn);
            //    adp.Fill(table);
            //}

            switch (ConfigurationManager.AppSettings["dbtype"].ToUpper().Trim())
            {
                case "SYSTEM.DATA.ORACLECLIENT":
                    using (OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                    {
                        OracleCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                        object tableInited = cmd.ExecuteScalar();
                        if (tableInited == null || tableInited == DBNull.Value)
                        {
                            cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();

                        cmd.CommandText = string.Format("select code, name, age, birthday, salary, summary, created {1} {2} from TEMP_TestTKK0719 where rownum <= {0}", num, (readClob ? ", remark" : string.Empty), (readBlob ? ", extends" : string.Empty));
                        conn.Open();
                        using (OracleDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object[] objs = new object[500];
                                dr.GetValues(objs);
                            }
                        }
                        conn.Close();
                    }

                    break;
                case "ODP.NET":
                    using (IDbConnection conn = ODPClientFactory.CreateConnection())
                    {
                        conn.ConnectionString = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
                        IDbCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                        object tableInited = cmd.ExecuteScalar();
                        if (tableInited == null || tableInited == DBNull.Value)
                        {
                            cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();

                        cmd.CommandText = string.Format("select code, name, age, birthday, salary, summary, created {1} {2} from TEMP_TestTKK0719 where rownum <= {0}", num, (readClob ? ", remark" : string.Empty), (readBlob ? ", extends" : string.Empty));
                        conn.Open();
                        using (IDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object[] objs = new object[500];
                                dr.GetValues(objs);
                            }
                        }
                        conn.Close();
                    }

                    break;
                case "ODAC":
                    using (ManagedOralceConnection conn = new ManagedOralceConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                    {
                        ManagedOracleCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = string.Format("select 1 from user_tables t where t.table_name = upper('TEMP_TestTKK0719')");
                        object tableInited = cmd.ExecuteScalar();
                        if (tableInited == null || tableInited == DBNull.Value)
                        {
                            cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
                                                    code        varchar2(50) NOT NULL,
                                                    name		varchar2(100) NULL,
                                                    age         int   NULL,
                                                    birthday    timestamp   NULL,
                                                    salary      decimal(10, 2) NULL,
                                                    summary     varchar2(1000) NULL,
                                                    remark      clob NULL,
                                                    extends     blob NULL,
                                                    created     timestamp,
                                                    CONSTRAINT  PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                                )";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();


                        cmd.CommandText = string.Format("select code, name, age, birthday, salary, summary, created {1} {2} from TEMP_TestTKK0719 where rownum <= {0}", num, (readClob ? ", remark" : string.Empty), (readBlob ? ", extends" : string.Empty));
                        conn.Open();
                        using (ManagedOracleDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object[] objs = new object[500];
                                dr.GetValues(objs);
                            }
                        }
                        conn.Close();
                    }

                    break;
                default:
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connstr"].ConnectionString))
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        conn.Open();
                        cmd.CommandText = string.Format("select 1 from sys.tables t where t.name = 'TEMP_TestTKK0719'");
                        object tableInited = cmd.ExecuteScalar();
                        if (tableInited == null || tableInited == DBNull.Value)
                        {
                            cmd.CommandText = @"CREATE TABLE TEMP_TestTKK0719(
	                                            code		varchar(50) NOT NULL,
	                                            name		varchar(100) NULL,
	                                            age			int   NULL,
	                                            birthday	datetime   NULL,
	                                            salary		decimal(10, 2) NULL,
	                                            summary		nvarchar(1000) NULL,
	                                            remark		varchar(max) NULL,
	                                            extends		varbinary(max) NULL,
                                                created     datetime,
	                                            CONSTRAINT PK__TEMP_TestTKK0719   PRIMARY KEY  (code)
                                            )";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_Name on TEMP_TestTKK0719(name)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_age on TEMP_TestTKK0719(age)";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "create index idx_TEMP_TestTKK0719_birthday on TEMP_TestTKK0719(birthday)";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();


                        cmd.CommandText = string.Format("select top {0} code, name, age, birthday, salary, summary, created {1} {2} from TEMP_TestTKK0719", num, (readClob ? ", remark" : string.Empty), (readBlob ? ", extends" : string.Empty));
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object[] objs = new object[500];
                                dr.GetValues(objs);
                            }
                        }
                        conn.Close();
                    }
                    break;
            }
        }

        internal static void SavePerf(PerfData perf)
        {
            string dbtype = ConfigurationManager.AppSettings["dbtype"].ToUpper().Trim();
            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            if (dbtype == "MSSQL")
            {
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    object tableInited = conn.ExecuteScalar("select 1 from sys.tables t where t.name = 'TEMP_TKK_PerfData0719'");
                    if (tableInited == null || tableInited == DBNull.Value)
                    {
                        conn.Execute(@"CREATE TABLE TEMP_TKK_PerfData0719(
                                                        code            varchar(36) NOT NULL,
                                                        LogDate		    datetime not null,
                                                        ClientName      varchar(100) null,
                                                        ClientDuration  int     NULL,
                                                        AppDuration     int     NULL,
                                                        DbDuration      int     NULL,
                                                        ClientRequests  int     NULL,
                                                        AppResponds     int     NULL,
                                                        AppRequests     int     NULL,
                                                        DbResponds      int     null,
                                                        ReadClob        int     null,
                                                        ReadBlob        int     null,
                                                        WriteClob       int     null,
                                                        WriteBlob       int     null,
                                                        ClobMax         int     null,
                                                        BlobMax         int     null,
                                                        RandomLob       int     null,
	                                                    CONSTRAINT PK__TEMP_TKK_PerfData0719   PRIMARY KEY  (code)
                                                    )");
                        conn.Execute("create index idx1_TEMP_TKK_PerfData0719 on TEMP_TKK_PerfData0719(LogDate)");
                        conn.Execute("create index idx2_TEMP_TKK_PerfData0719 on TEMP_TKK_PerfData0719(ClientName)");
                    }

                    string sql = "insert into TEMP_TKK_PerfData0719 (code, LogDate, ClientName, ClientDuration, AppDuration, DbDuration"
                        + ", clientrequests, appresponds, apprequests, dbresponds, readclob, readblob, writeclob, writeblob, clobmax, blobmax, randomlob)"
                        + " values(@Id, @LogDate, @ClientName, @ClientDuration, @AppDuration, @DbDuration"
                        + " , @ClientRequests, @AppResponds, @AppRequests, @DbResponds, @ReadClob, @ReadBlob, @WriteClob, @WriteBlob"
                        + " , @MaxClobLength, @MaxBlobLength, @RandomLobLength)";
                    conn.Execute(sql, perf);
                }
            }
            else
            {
                IDbConnection conn;
                switch (dbtype)
                {
                    case "ODAC":
                        conn = new ManagedOralceConnection(connstr);
                        break;
                    case "ODP.NET":
                        conn = ODPClientFactory.CreateConnection();
                        conn.ConnectionString = connstr;
                        break;
                    default:
                        conn = new OracleConnection(connstr);
                        break;
                }

                try
                {
                    object tableInited = conn.ExecuteScalar("select 1 from user_tables t where t.table_name = upper('TEMP_TKK_PerfData0719')");
                    if (tableInited == null || tableInited == DBNull.Value)
                    {
                        conn.Execute(@"CREATE TABLE TEMP_TKK_PerfData0719(
                                                        code            varchar2(36) NOT NULL,
                                                        LogDate		    timestamp not null,
                                                        ClientName      varchar2(100) null,
                                                        ClientDuration  int     NULL,
                                                        AppDuration     int     NULL,
                                                        DbDuration      int     NULL,
                                                        ClientRequests  int     NULL,
                                                        AppResponds     int     NULL,
                                                        AppRequests     int     NULL,
                                                        DbResponds      int     null,
                                                        ReadClob        int     null,
                                                        ReadBlob        int     null,
                                                        WriteClob       int     null,
                                                        WriteBlob       int     null,
                                                        ClobMax         int     null,
                                                        BlobMax         int     null,
                                                        RandomLob       int     null,
	                                                    CONSTRAINT PK__TEMP_TKK_PerfData0719   PRIMARY KEY  (code)
                                                    )");
                        conn.Execute("create index idx1_TEMP_TKK_PerfData0719 on TEMP_TKK_PerfData0719(LogDate)");
                        conn.Execute("create index idx2_TEMP_TKK_PerfData0719 on TEMP_TKK_PerfData0719(ClientName)");
                    }

                    string sql = "insert into TEMP_TKK_PerfData0719 (code, LogDate, ClientName, ClientDuration, AppDuration, DbDuration"
                        + ", clientrequests, appresponds, apprequests, dbresponds, readclob, readblob, writeclob, writeblob, clobmax, blobmax, randomlob)"
                        + " values(:Id, :LogDate, :ClientName, :ClientDuration, :AppDuration, :DbDuration"
                        + " , :ClientRequests, :AppResponds, :AppRequests, :DbResponds, :ReadClob, :ReadBlob, :WriteClob, :WriteBlob"
                        + " , :MaxClobLength, :MaxBlobLength, :RandomLobLength)";
                    conn.Execute(sql, perf);

                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}