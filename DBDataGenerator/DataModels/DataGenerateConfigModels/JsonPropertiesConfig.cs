using CommunityToolkit.Mvvm.ComponentModel;
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
        public string _propertyName;
        public IDataGenerateConfig _propertyValueConfig;

        /// <summary>
        /// Json属性名
        /// </summary>
        public string PropertyName { get => this._propertyName; set => SetProperty(ref _propertyName, value); }

        /// <summary>
        /// Json属性配置
        /// </summary>
        public IDataGenerateConfig PropertyValueConfig { get => this._propertyValueConfig; set => SetProperty(ref _propertyValueConfig, value); }
    }
}
