using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DBDataGenerator.DataModels;
using Mapster;
using NPOI.SS.Formula.Functions;
using NPOI.POIFS.Storage;
using DBDataGenerator.DataModels.ViewObjects;
using DBDataGenerator.Common;

namespace DBDataGenerator.Services
{
    public class DataBaseService
    {
        private SelectTableService _SelectTableService;
        public MySqlConnection MySqlConnection { get; set; }
        public string ConnectionString { get; set; }

        public DataBaseService(SelectTableService SelectTableService)
        {
            _SelectTableService = SelectTableService;
        }


        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public MySqlConnection CreateDataBaseConnection(string hostName, string port, string user, string password)
        {
            ConnectionString = $"Server={hostName};Uid={user};Pwd={password};";
            MySqlConnection = new MySqlConnection(ConnectionString);
            MySqlConnection.Open();
            return MySqlConnection;
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void DisconnectDataBase()
        {
            if (MySqlConnection != null)
            {
                MySqlConnection.Dispose();
            }
        }

        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <returns></returns>
        public List<DatabaseEntity> GetDatabases()
        {
            string query = "SELECT schema_name FROM information_schema.schemata";
            var adapter = new MySqlDataAdapter(query, MySqlConnection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable.ConvertDataTableToList<DatabaseEntity>();

        }

        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <returns></returns>
        public List<TableEntity> GetTables(DatabaseEntity databaseEntity)
        {
            string query = $"SHOW TABLE STATUS FROM `{databaseEntity.SCHEMA_NAME}`;";
            var adapter = new MySqlDataAdapter(query, MySqlConnection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable.ConvertDataTableToList<TableEntity>();
        }


        /// <summary>
        /// 获取数据库列的结构信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="tableName">表格名称，可空，空值时将获取数据库所有表格的字段信息</param>
        /// <returns></returns>
        public List<ColumnSchema> GetDatabaseColumnSchemas(string databaseName, string tableName = null)
        {
            // 获取数据库所有列信息
            string query = $"SELECT * from information_schema.columns where table_schema = '{databaseName}'";

            // 如果指定了表名，则只获取某个表的字段信息
            if (!string.IsNullOrEmpty(tableName))
            {
                query += $" and table_name = '{tableName}'";
            }


            var adapter = new MySqlDataAdapter(query, MySqlConnection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            List<ColumnSchema> list = dataTable.ConvertDataTableToList<ColumnSchema>().OrderBy(x=>x.ORDINAL_POSITION).ToList();
            return list;
        }


        /// <summary>
        /// 导出表格结构到Excel表
        /// </summary>
        /// <param name="tables">选中的表</param>
        /// <param name="databaseName">数据库名</param>
        /// <param name="fileName">文件名</param>
        public void SelectTableToExcel(List<TableSelectorVO> tables, string databaseName, string fileName)
        {
            if (tables == null)
            {
                throw new Exception("选中的表格不能为空");
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new Exception("数据库名称不能为空");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("文件名称不能为空");
            }

            _SelectTableService.SelectTableToExcel(MySqlConnection, tables, databaseName, fileName);
        }
    }
}
