using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// 随机整数生成配置
    /// </summary>
    public class RandomIntGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private int _upperLimit = 100;
        private int _lowerLimit = 0;

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
        /// 数值上限
        /// </summary>
        public int UpperLimit { get => this._upperLimit; set => SetProperty(ref _upperLimit, value); }

        /// <summary>
        /// 数值下限
        /// </summary>
        private int LowerLimit { get => this._lowerLimit; set => SetProperty(ref _lowerLimit, value); }
    }
}
