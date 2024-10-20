using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// JSON对象配置
    /// </summary>
    public class JsonObjectGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private List<JsonPropertiesConfig> _properties;

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
        /// JSON属性配置
        /// </summary>
        public List<JsonPropertiesConfig> Properties { get => this._properties; set { this._properties = value; } }
    }
}
