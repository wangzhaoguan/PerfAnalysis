using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace WebDiag
{
    class ODPClientFactory
    {
        /// <summary>
        /// Oracle Client版本
        /// Oracle 11g以上支持OracleBulkCopy
        /// </summary>
        private static int oracleVersion = -1;
        private static string dllPath = string.Empty;
        private static Assembly assembly;
        private static object lockObject = new object();

        private static readonly string ORACLECONNECTION = "Oracle.DataAccess.Client.OracleConnection";
        private static readonly string ORACLEPARAMETER = "Oracle.DataAccess.Client.OracleParameter";

        private static Type oracleConnection = null;
        private static Type oracleParameter = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ODPClientFactory()
        {
            Initialize();
        }

        #region 反射Oracle.DataAccess.dll相关

        /// <summary>
        /// 已经加载的Oracle.DataAccess
        /// </summary>
        private static Assembly OracleAssembly
        {
            get
            {
                if (assembly == null)
                {
                    lock (lockObject)
                    {
                        if (assembly == null)
                        {
                            string oracleDataAccessFileName = DllPath.TrimEnd('\\') + @"\Oracle.DataAccess.dll";
                            assembly = Assembly.LoadFile(oracleDataAccessFileName);
                        }
                    }
                }
                return assembly;
            }
        }

        /// <summary>
        /// Oracle.DataAccess.dll所在路径
        /// </summary>
        private static string DllPath
        {
            get
            {
                if (string.IsNullOrEmpty(dllPath))
                {
                    Initialize();
                }
                return dllPath;
            }
        }


        /// <summary>
        /// 初始Oracle驱动的相关信息
        /// </summary>
        private static void Initialize()
        {
            try
            {
                //读取环境变量
                string pathEnviroment = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
                string[] pathArray = pathEnviroment.Split(new string[] { ";" }, StringSplitOptions.None);
                string binPath = string.Empty;
                foreach (string pathVal in pathArray)
                {
                    string sqlplusExe = Path.Combine(pathVal, "sqlplus.exe");
                    if (File.Exists(sqlplusExe))
                    {
                        binPath = pathVal;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(binPath))
                {
                    //环境变量Path中存在oracle客户端配置信息
                    FileVersionInfo ociFileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(binPath, "oci.dll"));
                    string fileVersion = ociFileVersionInfo.FileVersion;
                    string[] fileBigVersion = fileVersion.Split(new string[] { "." }, StringSplitOptions.None);
                    if (fileBigVersion.Length > 1)
                    {
                        oracleVersion = Convert.ToInt32(fileBigVersion[0]);
                    }
                    DirectoryInfo directoryInfo = new DirectoryInfo(Directory.GetParent(binPath).ToString() + @"\ODP.NET\bin\");
                    if (directoryInfo.Exists)
                    {
                        foreach (var item in directoryInfo.GetDirectories())
                        {
                            foreach (var fileInfo in item.GetFiles())
                            {
                                if (fileInfo.Name.Equals("ORACLE.DATAACCESS.DLL", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    dllPath = Convert.ToString(fileInfo.Directory);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //屏蔽错误，防止出现进程崩溃的问题
            }
        }


        private static Type OracleConnection
        {
            get
            {
                if (oracleConnection == null)
                {
                    oracleConnection = OracleAssembly.GetType(ORACLECONNECTION);
                }
                return oracleConnection;
            }
        }

        private static Type OracleParameter
        {
            get
            {
                if (oracleParameter == null)
                {
                    oracleParameter = OracleAssembly.GetType(ORACLEPARAMETER);
                }
                return oracleParameter;
            }
        }

        #endregion

        #region 数据库相关接口的创建

        /// <summary>
        /// 建立数据库连接。
        /// </summary>
        /// <returns>数据库连接接口</returns>
        public static IDbConnection CreateConnection()
        {
            return Activator.CreateInstance(OracleConnection, new object[] { }) as IDbConnection;
        }

        /// <summary>
        /// 建立数据库连接。
        /// </summary>
        /// <returns>数据库参数接口</returns>
        public static IDataParameter CreateParameter()
        {
            return Activator.CreateInstance(OracleParameter, new object[] { }) as IDataParameter;
        }

        #endregion

    }
}