using CommunityToolkit.Mvvm.ComponentModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// 用户自定义数据配置
    /// </summary>
    public class UserDataGenerateConfig : ObservableObject, IDataGenerateConfig
    {
        private string _databaseName;
        private string _tableName;
        private string _columnName;
        private string _dataName = "custonData";
        private List<string> _dataList = new List<string>();

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
        /// 数据名称
        /// </summary>
        public string DataName { get => this._dataName; set => SetProperty(ref _dataName, value); }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<string> DataList { get => this._dataList; set { this._dataList = value; } }
    }
}
