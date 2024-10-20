using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    public class FixedStringGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private string _stringVal = "生成器默认生成";

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
        /// 固定字符串值
        /// </summary>
        public string StringVal { get => this._stringVal; set => SetProperty(ref _stringVal, value); }
    }
}
