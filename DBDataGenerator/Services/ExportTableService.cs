using DBDataGenerator.Common;
using MySql.Data.MySqlClient;
using NPOI.OpenXmlFormats.Dml.Chart;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.ViewObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DBDataGenerator.Services
{
    public class SelectTableService
    {
        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="tables"></param>
        /// <param name="databaseName"></param>
        /// <param name="fileName"></param>
        public void SelectTableToExcel(MySqlConnection mySqlConnection, List<TableSelectorVO> tables, string databaseName, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook();// 创建Excel表
                CreateSheet_DatabaseInfo(mySqlConnection, databaseName, workbook);// 创建数据库说明
                CreateSheets_TableInfo(mySqlConnection,tables,databaseName,workbook);// 添加表结构说明

                workbook.Write(stream);// 保存内容到文件中
            }
        }


        /// <summary>
        /// 设置Excel表的第一个sheet：数据库描述信息
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="databaseName"></param>
        /// <param name="workbook"></param>
        private void CreateSheet_DatabaseInfo(MySqlConnection mySqlConnection, string databaseName, IWorkbook workbook)
        {
            ISheet sheet = workbook.CreateSheet("数据库表信息");
            IRow row = sheet.CreateRow(0);
            ICellStyle rowstyle = workbook.CreateCellStyle();
            rowstyle.FillForegroundColor = IndexedColors.BrightGreen.Index;
            rowstyle.FillPattern = FillPattern.SolidForeground;
            rowstyle.VerticalAlignment = VerticalAlignment.Center;

            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 16;
            rowstyle.SetFont(font);
            row.RowStyle = rowstyle;



            row.CreateCell(0).SetCellValue("表名");
            row.CreateCell(1).SetCellValue("引擎");
            row.CreateCell(2).SetCellValue("自增");
            row.CreateCell(3).SetCellValue("数据长度");
            row.CreateCell(4).SetCellValue("描述");

            List<TableSchema> list = GetTableSchemas(mySqlConnection, databaseName);

            int rowIndex = 1;
            foreach (var tableSchema in list)
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(tableSchema.TABLE_NAME);
                row.CreateCell(1).SetCellValue(tableSchema.ENGINE);
                row.CreateCell(2).SetCellValue(tableSchema.AUTO_INCREMENT.Value);
                row.CreateCell(3).SetCellValue(tableSchema.DATA_LENGTH.Value);
                row.CreateCell(4).SetCellValue(tableSchema.TABLE_COMMENT);

                rowIndex++;
            }
        }


        /// <summary>
        /// 创建数据库表结构说明
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="tables"></param>
        /// <param name="databaseName"></param>
        /// <param name="workbook"></param>
        private void CreateSheets_TableInfo(MySqlConnection mySqlConnection, List<TableSelectorVO> tables, string databaseName, IWorkbook workbook)
        {
            List<TableSchema> tableSchemas = GetTableSchemas(mySqlConnection, databaseName);//   获取数据库所有表的信息
            List<ColumnSchema> databaseColumns = GetDatabaseColumnSchemas(mySqlConnection, databaseName);            // 获取数据库所有列信息

            // 输出表结构信息
            foreach (TableSelectorVO table in tables)
            {
                TableSchema tableSchema =tableSchemas.Where(x => x.TABLE_NAME == table.Name).ToList().FirstOrDefault(); // 获取表结构信息

                // 在Excel表创建一个sheet，用来说明当前表结构信息
                string sheetName = string.IsNullOrEmpty(tableSchema.TABLE_COMMENT) ? tableSchema.TABLE_NAME : tableSchema.TABLE_COMMENT; 
                ISheet sheet = workbook.CreateSheet(sheetName);

                var tableColumns = databaseColumns.Where(x => x.TABLE_NAME == table.Name).ToList();// 获取该表的所有字段信息

                IRow row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("序号");
                row.CreateCell(1).SetCellValue("字段名");
                row.CreateCell(2).SetCellValue("注释");
                row.CreateCell(3).SetCellValue("数据类型");
                row.CreateCell(4).SetCellValue("允许NULL");
                row.CreateCell(5).SetCellValue("键");
                row.CreateCell(6).SetCellValue("字符集");
                row.CreateCell(7).SetCellValue("排序规则");

                int rowIndex = 1;
                foreach (ColumnSchema column in tableColumns)
                {
                    row = sheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(column.ORDINAL_POSITION.Value);
                    row.CreateCell(1).SetCellValue(column.COLUMN_NAME);
                    row.CreateCell(2).SetCellValue(column.COLUMN_COMMENT);
                    row.CreateCell(3).SetCellValue(column.COLUMN_TYPE);
                    row.CreateCell(4).SetCellValue(column.IS_NULLABLE);
                    row.CreateCell(5).SetCellValue(column.COLUMN_KEY);
                    row.CreateCell(6).SetCellValue(column.CHARACTER_SET_NAME);
                    row.CreateCell(7).SetCellValue(column.COLLATION_NAME);

                    rowIndex++;
                }
            }
        }


        /// <summary>
        /// 获取数据库所有表的结构信息
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private List<TableSchema> GetTableSchemas(MySqlConnection mySqlConnection, string databaseName)
        {
            string query = $"SELECT * FROM information_schema.TABLES WHERE table_schema = '{databaseName}';";
            var adapter = new MySqlDataAdapter(query, mySqlConnection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable.ConvertDataTableToList<TableSchema>();
        }



        /// <summary>
        /// 获取数据库所有列的结构信息
        /// </summary>
        /// <param name="mySqlConnection"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private List<ColumnSchema> GetDatabaseColumnSchemas(MySqlConnection mySqlConnection, string databaseName)
        {
            // 获取数据库所有列信息
            string query = $"SELECT * from information_schema.columns where table_schema = '{databaseName}'";
            var adapter = new MySqlDataAdapter(query, mySqlConnection);
            var dataTable = new DataTable();
            adapter.Fill(dataTable);
            return dataTable.ConvertDataTableToList<ColumnSchema>();
        }

    }
}
