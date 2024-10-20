using CommunityToolkit.Mvvm.ComponentModel;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DBDataGenerator.DataModels.ViewObjects
{
    /// <summary>
    /// 文本生成配置输入项，输入项太多了，占用VM，所以分一个对象出来专门保存
    /// </summary>
    public class TextGenerateFormVO : ObservableObject
    {
        private int? _intValueMin = 0;
        private int? _intValueMax = 100;
        private double? _realValueMin = 0.0;
        private double? _realValueMax = 100.0;
        private int _jsonArrayCount = 1;
        private ObservableCollection<JsonPropertiesConfig> _properties = new ObservableCollection<JsonPropertiesConfig>();
        private JsonPropertiesConfig? _selectedJsonProperty;
        private string _prefix = "TEST";
        private int _suffixNum = 10000;
        private DateTime? _fixedDatetimeVal = DateTime.Now;
        private string _stringVal = "生成器默认生成";
        private JsonPropertyVO? _editingJsonProperty;
        private bool _showJsonPropertyEditArea = false;


        private ObservableCollection<DataGenerateTypeSelectorVO> _jsonPropertyDataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>(
            new List<DataGenerateTypeSelectorVO>() {
                    new DataGenerateTypeSelectorVO() { Name = "随机整数", DataGenerateType = DataGenerateTypeEnum.RandomInt },
                    new DataGenerateTypeSelectorVO() { Name = "随机浮点数", DataGenerateType = DataGenerateTypeEnum.RandomFloat },
                    new DataGenerateTypeSelectorVO() { Name = "条码", DataGenerateType = DataGenerateTypeEnum.Barcode },
                    new DataGenerateTypeSelectorVO() { Name = "唯一ID", DataGenerateType = DataGenerateTypeEnum.Guid },
                    new DataGenerateTypeSelectorVO() { Name = "时间", DataGenerateType = DataGenerateTypeEnum.Datetime },
                    new DataGenerateTypeSelectorVO() { Name = "固定字符串", DataGenerateType = DataGenerateTypeEnum.FixedString }
                }
            );

        private DataGenerateTypeSelectorVO? _selectedJsonPropertyDataGenerateType;

        /// <summary>
        /// 随机整数下限
        /// </summary>
        public int? IntValueMin { get => _intValueMin; set => SetProperty(ref _intValueMin, value); }

        /// <summary>
        /// 随机整数上限
        /// </summary>
        public int? IntValueMax { get => _intValueMax; set => SetProperty(ref _intValueMax, value); }

        /// <summary>
        /// 随机实数下限
        /// </summary>
        public double? RealValueMin { get => _realValueMin; set => SetProperty(ref _realValueMin, value); }

        /// <summary>
        /// 随机实数上限
        /// </summary>
        public double? RealValueMax { get => _realValueMax; set => SetProperty(ref _realValueMax, value); }

        /// <summary>
        /// JSON数组元素个数
        /// </summary>
        public int JsonArrayCount { get => _jsonArrayCount; set => SetProperty(ref _jsonArrayCount, value); }

        /// <summary>
        /// JSON对象属性列表
        /// </summary>
        public ObservableCollection<JsonPropertiesConfig> Properties { get => _properties; set => SetProperty(ref _properties, value); }

        /// <summary>
        /// 选中的JSON属性
        /// </summary>
        public JsonPropertiesConfig? SelectedJsonProperty
        {
            get => _selectedJsonProperty;
            set
            {
                SetProperty(ref _selectedJsonProperty, value);

                // 没有选中项时，隐藏输入区域
                if (_selectedJsonProperty == null)
                {
                    ShowJsonPropertyEditArea = false;
                }
                else
                {
                    ShowJsonPropertyEditArea = true;
                }

                // 更新编辑中的对象
                UpdateEditingJsonPropertyVO(_selectedJsonProperty);
            }
        }

        /// <summary>
        /// 条码前缀
        /// </summary>
        public string Prefix { get => _prefix; set => SetProperty(ref _prefix, value); }

        /// <summary>
        /// 条码后缀数字
        /// </summary>
        public int SuffixNum { get => _suffixNum; set => SetProperty(ref _suffixNum, value); }

        /// <summary>
        /// 固定时间
        /// </summary>
        public DateTime? FixedDatetimeVal { get => _fixedDatetimeVal; set => SetProperty(ref _fixedDatetimeVal, value); }

        /// <summary>
        /// 固定字符串
        /// </summary>
        public string StringVal { get => _stringVal; set => SetProperty(ref _stringVal, value); }

        /// <summary>
        /// 编辑中的Json对象
        /// </summary>
        public JsonPropertyVO? EditingJsonProperty
        {
            get => _editingJsonProperty;
            set => SetProperty(ref _editingJsonProperty, value);
        }

        /// <summary>
        /// Json属性可选的生成类型
        /// </summary>
        public ObservableCollection<DataGenerateTypeSelectorVO> JsonPropertyDataGenerateTypeList { get => _jsonPropertyDataGenerateTypeList; set => SetProperty(ref _jsonPropertyDataGenerateTypeList, value); }


        /// <summary>
        /// 当前编辑中的JSON属性的数据生成类型
        /// </summary>
        public DataGenerateTypeSelectorVO? SelectedJsonPropertyDataGenerateType { get => _selectedJsonPropertyDataGenerateType; set => SetProperty(ref _selectedJsonPropertyDataGenerateType, value); }

        /// <summary>
        /// 显示JSON属性编辑区域
        /// </summary>
        public bool ShowJsonPropertyEditArea { get => _showJsonPropertyEditArea; set => SetProperty(ref _showJsonPropertyEditArea, value); }

        /// <summary>
        /// 更新编辑中的Json属性信息
        /// </summary>
        /// <param name="selectedJsonProperty"></param>
        private void UpdateEditingJsonPropertyVO(JsonPropertiesConfig selectedJsonProperty)
        {
            if (selectedJsonProperty == null)
            {
                return;
            }

            JsonPropertyVO jsonPropertyVO = new JsonPropertyVO();

            // 选择的数据生成类型
            this.SelectedJsonPropertyDataGenerateType = this.JsonPropertyDataGenerateTypeList
                .FirstOrDefault(x => x.DataGenerateType == selectedJsonProperty.DataGenerateType);

            // 读取当前选中JSON项的值，显示在表格中
            switch (selectedJsonProperty.DataGenerateType)
            {
                case DataGenerateTypeEnum.RandomInt:
                    {
                        jsonPropertyVO.IntValueMin = ((RandomIntGenerateConfig)selectedJsonProperty.PropertyValueConfig).LowerLimit;
                        jsonPropertyVO.IntValueMax = ((RandomIntGenerateConfig)selectedJsonProperty.PropertyValueConfig).UpperLimit;
                    }
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        jsonPropertyVO.RealValueMin = ((RandomFloatGenerateConfig)selectedJsonProperty.PropertyValueConfig).LowerLimit;
                        jsonPropertyVO.RealValueMax = ((RandomFloatGenerateConfig)selectedJsonProperty.PropertyValueConfig).UpperLimit;
                    }
                    break;
                case DataGenerateTypeEnum.Barcode:
                    {
                        jsonPropertyVO.Prefix = ((BarcodeGenerateConfig)selectedJsonProperty.PropertyValueConfig).Prefix;
                        jsonPropertyVO.SuffixNum = ((BarcodeGenerateConfig)selectedJsonProperty.PropertyValueConfig).SuffixNum;
                    }
                    break;
                case DataGenerateTypeEnum.Datetime:
                    {
                        jsonPropertyVO.FixedDatetimeVal = ((DatetimeGenerateConfig)selectedJsonProperty.PropertyValueConfig).DateTimeVal;
                    }
                    break;
                case DataGenerateTypeEnum.FixedString:
                    {
                        jsonPropertyVO.StringVal = ((FixedStringGenerateConfig)selectedJsonProperty.PropertyValueConfig).StringVal;
                    }
                    break;
                default: break;
            }

            this.EditingJsonProperty = jsonPropertyVO;
        }
    }
}
