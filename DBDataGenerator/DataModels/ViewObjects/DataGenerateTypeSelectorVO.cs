using CommunityToolkit.Mvvm.ComponentModel;
using DBDataGenerator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.ViewObjects
{
    /// <summary>
    /// 数据生成类型选项
    /// </summary>
    public class DataGenerateTypeSelectorVO : ObservableObject
    {
        private string _name;
        private DataGenerateTypeEnum _dataGenerateType;


        /// <summary>
        /// 生成配置名称
        /// </summary>
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        /// <summary>
        /// 生成配置类型
        /// </summary>
        public DataGenerateTypeEnum DataGenerateType { get => _dataGenerateType; set => SetProperty(ref _dataGenerateType, value); }
    }
}
