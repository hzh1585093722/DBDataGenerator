using CommunityToolkit.Mvvm.ComponentModel;
using DBDataGenerator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// JSON属性配置
    /// </summary>
    public class JsonPropertiesConfig : ObservableObject
    {
        private string _propertyName="";
        private DataGenerateTypeEnum _dataGenerateType = DataGenerateTypeEnum.FixedString;
        private IDataGenerateConfig _propertyValueConfig = new FixedStringGenerateConfig() { StringVal=""};

        /// <summary>
        /// Json属性名
        /// </summary>
        public string PropertyName { get => this._propertyName; set => SetProperty(ref _propertyName, value); }

        /// <summary>
        /// Json属性的数据生成类型
        /// </summary>
        public DataGenerateTypeEnum DataGenerateType { get => this._dataGenerateType; set => SetProperty(ref _dataGenerateType, value); }

        /// <summary>
        /// Json属性配置
        /// </summary>
        public IDataGenerateConfig PropertyValueConfig { get => this._propertyValueConfig; set => SetProperty(ref _propertyValueConfig, value); }
    }
}
