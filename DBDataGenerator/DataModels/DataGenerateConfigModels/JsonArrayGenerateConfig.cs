using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// JSON数组生成配置
    /// </summary>
    public class JsonArrayGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private int _jsonArrayCount = 1;
        private JsonObjectGenerateConfig _jsonObjectConfig = new JsonObjectGenerateConfig();

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get => this._databaseName; set => SetProperty(ref _databaseName, value); }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get => this._tableName; set => SetProperty(ref _tableName, value); }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get => this._columnName; set => SetProperty(ref _columnName, value); }

        /// <summary>
        /// JSON数组元素个数
        /// </summary>
        public int JsonArrayCount { get => this._jsonArrayCount; set => SetProperty(ref _jsonArrayCount, value); }

        /// <summary>
        /// JSON对象配置
        /// </summary>
        public JsonObjectGenerateConfig JsonObjectConfig { get => this._jsonObjectConfig; set => SetProperty(ref _jsonObjectConfig, value); }

    }
}
