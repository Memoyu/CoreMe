/**************************************************************************  
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：Memoyu.Infrastructure.FreeSqlExt
*   文件名称 ：FreeSqlExtension.cs
*   =================================
*   创 建 者 ：Memoyu
*   创建日期 ：2020-12-28 0:34:42
*   邮箱     ：mmy6076@outlook.com
*   功能描述 ：
***************************************************************************/
using FreeSql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Serilog;
using System;
using System.Data;
using System.IO;
using System.Reflection;

namespace Memoyu.Extensions.FreeSqlExt
{
    public static class FreeSqlExtension
    {

        public static FreeSqlBuilder UseConnectionString(this FreeSqlBuilder @this, IConfiguration configuration)
        {
            IConfigurationSection dbTypeCode = configuration.GetSection("ConnectionStrings:DefaultDB");
            if (Enum.TryParse(dbTypeCode.Value, out DataType dataType))
            {
                if (!Enum.IsDefined(typeof(DataType), dataType))
                {
                    Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dataType}无效");
                }

                IConfigurationSection configurationSection = configuration.GetSection($"ConnectionStrings:{dataType}");
                @this.UseConnectionString(dataType, configurationSection.Value);
            }
            else
            {
                Log.Error($"数据库配置ConnectionStrings:DefaultDB:{dbTypeCode.Value}无效");
            }

            return @this;
        }
        /// <summary>
        /// 请在UseConnectionString配置后调用此方法
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static FreeSqlBuilder CreateDatabaseIfNotExists(this FreeSqlBuilder @this)
        {
            FieldInfo dataTypeFieldInfo = @this.GetType().GetField("_dataType", BindingFlags.NonPublic | BindingFlags.Instance);

            if (dataTypeFieldInfo is null)
            {
                throw new ArgumentException("_dataType is null");
            }

            string connectionString = GetConnectionString(@this);
            DataType dbType = (DataType)dataTypeFieldInfo.GetValue(@this);

            switch (dbType)
            {
                case DataType.MySql:
                    return @this.CreateDatabaseIfNotExistsMySql(connectionString);
                case DataType.SqlServer:
                    return @this.CreateDatabaseIfNotExistsSqlServer(connectionString);
                case DataType.PostgreSQL:
                    break;
                case DataType.Oracle:
                    break;
                case DataType.Sqlite:
                    return @this;
                case DataType.OdbcOracle:
                    break;
                case DataType.OdbcSqlServer:
                    break;
                case DataType.OdbcMySql:
                    break;
                case DataType.OdbcPostgreSQL:
                    break;
                case DataType.Odbc:
                    break;
                case DataType.OdbcDameng:
                    break;
                case DataType.MsAccess:
                    break;
                case DataType.Dameng:
                    break;
                case DataType.OdbcKingbaseES:
                    break;
                case DataType.ShenTong:
                    break;
                case DataType.KingbaseES:
                    break;
                case DataType.Firebird:
                    break;
                default:
                    break;
            }

            Log.Error($"不支持创建数据库");
            return @this;
        }
        private static string GetConnectionString(FreeSqlBuilder @this)
        {
            Type type = @this.GetType();
            FieldInfo fieldInfo =
                type.GetField("_masterConnectionString", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo is null)
            {
                throw new ArgumentException("_masterConnectionString is null");
            }
            return fieldInfo.GetValue(@this).ToString();
        }

        public static FreeSqlBuilder CreateDatabaseIfNotExistsMySql(this FreeSqlBuilder @this,
           string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = GetConnectionString(@this);
            }

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);

            string createDatabaseSql =
                $"USE mysql;CREATE DATABASE IF NOT EXISTS `{builder.Database}` CHARACTER SET '{builder.CharacterSet}' COLLATE 'utf8mb4_general_ci'";

            using MySqlConnection cnn = new MySqlConnection(
                $"Data Source={builder.Server};Port={builder.Port};User ID={builder.UserID};Password={builder.Password};Initial Catalog=mysql;Charset=utf8;SslMode=none;Max pool size=1");
            cnn.Open();
            using (MySqlCommand cmd = cnn.CreateCommand())
            {
                cmd.CommandText = createDatabaseSql;
                cmd.ExecuteNonQuery();
            }

            return @this;
        }

        public static FreeSqlBuilder CreateDatabaseIfNotExistsSqlServer(this FreeSqlBuilder @this, string connectionString = "")
        {
            if (connectionString == "")
            {
                connectionString = GetConnectionString(@this);
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            string createDatabaseSql;
            if (!string.IsNullOrEmpty(builder.AttachDBFilename))
            {
                string fileName = ExpandFileName(builder.AttachDBFilename);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string logFileName = Path.ChangeExtension(fileName, ".ldf");
                createDatabaseSql = @$"CREATE DATABASE {builder.InitialCatalog}   on  primary   
                (
                    name = '{name}',
                    filename = '{fileName}'
                )
                log on
                (
                    name= '{name}_log',
                    filename = '{logFileName}'
                )";
            }
            else
            {
                createDatabaseSql = @$"CREATE DATABASE {builder.InitialCatalog}";
            }

            using SqlConnection cnn =
                new SqlConnection(
                    $"Data Source={builder.DataSource};Integrated Security = True;User ID={builder.UserID};Password={builder.Password};Initial Catalog=master;Min pool size=1");
            cnn.Open();
            using SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = $"select * from sysdatabases where name = '{builder.InitialCatalog}'";

            SqlDataAdapter apter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            apter.Fill(ds);

            if (ds.Tables[0].Rows.Count == 0)
            {
                cmd.CommandText = createDatabaseSql;
                cmd.ExecuteNonQuery();
            }

            return @this;
        }
        private static string ExpandFileName(string fileName)
        {
            if (fileName.StartsWith("|DataDirectory|", StringComparison.OrdinalIgnoreCase))
            {
                var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                if (string.IsNullOrEmpty(dataDirectory))
                {
                    dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
                string name = fileName.Replace("\\", "").Replace("/", "").Substring("|DataDirectory|".Length);
                fileName = Path.Combine(dataDirectory, name);
            }
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            return Path.GetFullPath(fileName);
        }
    }

}
