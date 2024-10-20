using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.Common
{
    public static class DataTableToListExtension
    {
        /// <summary>
        /// 讲datatable转为实体对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableToList<T>(this DataTable dataTable) where T : new()
        {
            List<T> entities = new List<T>();

            foreach (DataRow row in dataTable.Rows)
            {
                T entity = new T();

                foreach (DataColumn column in dataTable.Columns)
                {
                    string columnName = column.ColumnName;
                    object value = row[column];

                    // 查找对象的属性并设置值
                    string propertyName = FindPropertyNameByColumnName<T>(columnName);
                    var property = typeof(T).GetProperty(propertyName);
                    if (property != null && value != DBNull.Value)
                    {
                        property.SetValue(entity, value);
                    }
                }

                entities.Add(entity);
            }

            return entities;
        }


        /// <summary>
        /// 使用datatable的列名获取实体字段名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private static string FindPropertyNameByColumnName<T>(string columnName) where T : new()
        {
            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.Name.Replace("_", "").ToLower() == columnName.Replace("_", "").ToLower())
                {
                    return property.Name;
                }
            }

            return "";
        }
    }
}
