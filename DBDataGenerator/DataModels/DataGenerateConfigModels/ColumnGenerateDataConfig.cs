using DBDataGenerator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// 数据库列生成配置
    /// </summary>
    public class ColumnGenerateDataConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private DataGenerateTypeEnum _dataGenerateType;
        private IDataGenerateConfig _dataGenerateConfig;

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DatabaseName { get => this._databaseName; set { this._databaseName = value; } }

        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get => this._tableName; set { this._tableName = value; } }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get => this._columnName; set { this._columnName = value; } }

        /// <summary>
        /// 数据生成类型
        /// </summary>
        private DataGenerateTypeEnum DataGenerateType { get => this._dataGenerateType; set { this._dataGenerateType = value; } }

        /// <summary>
        /// 数据生成配置
        /// </summary>
        private IDataGenerateConfig DataGenerateConfig { get => this._dataGenerateConfig; set { this._dataGenerateConfig = value; } }

    }
}
