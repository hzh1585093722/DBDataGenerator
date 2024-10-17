using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// 条码生成配置
    /// </summary>
    public class BarcodeGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private string _prefix = "TEST";
        private int _suffixNum = 10000;

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
        /// 条码前缀
        /// </summary>
        public string Prefix { get => this._prefix; set => SetProperty(ref _prefix, value); }

        /// <summary>
        /// 条码后缀自增编号
        /// </summary>
        public int SuffixNum { get => this._suffixNum; set => SetProperty(ref _suffixNum, value); }
    }
}
