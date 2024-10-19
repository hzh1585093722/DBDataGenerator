using CommunityToolkit.Mvvm.ComponentModel;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.ViewObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBDataGenerator.DataModels.Enums;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DBDataGenerator.Viewmodels.DataGenerateConfigViewModels
{
    /// <summary>
    /// 视图模型：数值类型生成配置
    /// </summary>
    public class NumberGenerateConfigViewModel : ObservableObject
    {
        private ObservableCollection<DataGenerateTypeSelectorVO> _dataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>();
        private DataGenerateTypeSelectorVO? _selectedDataGenerateType;
        private int? _intValueMin = 0;
        private int? _intValueMax = 100;
        private double? _realValueMin = 0.0;
        private double? _realValueMax = 100.0;
        private bool _showIntValueForm = false;
        private bool _showRealValueForm = false;

        /// <summary>
        /// 用户点击保存按钮时的回调
        /// </summary>
        private Action<ColumnGenerateDataConfig> _onGenerateDataConfigSave;

        /// <summary>
        /// 列数据生成配置
        /// </summary>
        private ColumnGenerateDataConfig _generateDataConfig;

        /// <summary>
        /// 数据生成类型选项
        /// </summary>
        public ObservableCollection<DataGenerateTypeSelectorVO> DataGenerateTypeList { get => _dataGenerateTypeList; set => SetProperty(ref _dataGenerateTypeList, value); }

        /// <summary>
        /// 选中的数据生成选项
        /// </summary>
        public DataGenerateTypeSelectorVO? SelectedDataGenerateType
        {
            get => _selectedDataGenerateType;
            set
            {
                SetProperty(ref _selectedDataGenerateType, value);
                this.SwitchDataGenerateConfigForm(_selectedDataGenerateType);
            }
        }

        /// <summary>
        /// int最小值
        /// </summary>
        public int? IntValueMin { get => _intValueMin; set => SetProperty(ref _intValueMin, value); }

        /// <summary>
        /// int最大值
        /// </summary>
        public int? IntValueMax { get => _intValueMax; set => SetProperty(ref _intValueMax, value); }

        /// <summary>
        /// double最小值
        /// </summary>
        public double? RealValueMin { get => _realValueMin; set => SetProperty(ref _realValueMin, value); }

        /// <summary>
        /// double最大值
        /// </summary>
        public double? RealValueMax { get => _realValueMax; set => SetProperty(ref _realValueMax, value); }

        /// <summary>
        /// 显示int值设置界面
        /// </summary>
        public bool ShowIntValueForm { get => _showIntValueForm; set => SetProperty(ref _showIntValueForm, value); }

        /// <summary>
        /// 显示double值设置界面
        /// </summary>
        public bool ShowRealValueForm { get => _showRealValueForm; set => SetProperty(ref _showRealValueForm, value); }

        public NumberGenerateConfigViewModel()
        {

        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public RelayCommand SaveConfigCmd => new RelayCommand(() =>
        {
            try
            {
                // 拷贝原配置
                ColumnGenerateDataConfig? newConfig = JsonConvert.DeserializeObject<ColumnGenerateDataConfig>(
                    JsonConvert.SerializeObject(this._generateDataConfig));

                if (newConfig != null && this.SelectedDataGenerateType != null)
                {
                    newConfig.DataGenerateType = this.SelectedDataGenerateType.DataGenerateType;
                }

                switch (newConfig.DataGenerateType)
                {
                    case DataGenerateTypeEnum.RandomInt:
                        {
                            newConfig.DataGenerateConfig = new RandomIntGenerateConfig()
                            {
                                DatabaseName = newConfig.DatabaseName,
                                TableName = newConfig.TableName,
                                ColumnName = newConfig.ColumnName,
                                LowerLimit = this.IntValueMin.HasValue ? this.IntValueMin.Value : 0,
                                UpperLimit = this.IntValueMax.HasValue ? this.IntValueMax.Value : 100,
                            };
                        }
                        break;
                    case DataGenerateTypeEnum.RandomFloat:
                        {
                            newConfig.DataGenerateConfig = new RandomFloatGenerateConfig()
                            {
                                DatabaseName = newConfig.DatabaseName,
                                TableName = newConfig.TableName,
                                ColumnName = newConfig.ColumnName,
                                LowerLimit = this.RealValueMin.HasValue ? this.RealValueMin.Value : 0,
                                UpperLimit = this.RealValueMax.HasValue ? this.RealValueMax.Value : 100,
                            };
                        }
                        break;
                    default: break;
                }

                this._onGenerateDataConfigSave?.Invoke(newConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        /// <summary>
        /// 初始化配置页面
        /// </summary>
        /// <param name="columnSchema">列信息</param>
        /// <param name="generateDataConfig">数据生成配置</param>
        /// <param name="onGenerateDataConfigSave">数据生成配置保存时回调，交给调用者来处理储存相关的操作</param>
        public void InitConfigView(ColumnSchema columnSchema,
            ColumnGenerateDataConfig? generateDataConfig,
            Action<ColumnGenerateDataConfig> onGenerateDataConfigSave)
        {
            // 初始化下拉列表
            List<DataGenerateTypeSelectorVO> dataGenerateTypeSelectors = this.InitDataGenerateTypeSelector(generateDataConfig.DataGenerateType, generateDataConfig.MysqlDataTypeCategoryEnum);
            this.DataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>(dataGenerateTypeSelectors);
            this.SelectedDataGenerateType = this.DataGenerateTypeList.FirstOrDefault(x => x.DataGenerateType == generateDataConfig.DataGenerateType);

            // 初始化表单输入框
            this._generateDataConfig = generateDataConfig;
            this.InitForm(this._generateDataConfig.DataGenerateConfig);

            // 保存时回调
            this._onGenerateDataConfigSave = onGenerateDataConfigSave;
        }


        #region 私有方法

        /// <summary>
        /// 初始化生成器选项列表
        /// 自增ID类型的字段，无法选择随机整数、随机实数；
        /// 整数类型的字段，只能选择随机整数
        /// 实数类型字段，可以选择随机整数、随机实数
        /// </summary>
        /// <param name="dataGenerateType">数据生成类型</param>
        /// <param name="mysqlDataTypeCategory">Mysql数据类型分类</param>
        /// <returns></returns>
        private List<DataGenerateTypeSelectorVO> InitDataGenerateTypeSelector(DataGenerateTypeEnum dataGenerateType,
            MysqlDataTypeCategoryEnum mysqlDataTypeCategory)
        {
            List<DataGenerateTypeSelectorVO> list = new List<DataGenerateTypeSelectorVO>();

            // 自增ID
            if (dataGenerateType == DataGenerateTypeEnum.AutoIncrementID)
            {
                list.Add(new DataGenerateTypeSelectorVO() { Name = "自增ID", DataGenerateType = DataGenerateTypeEnum.AutoIncrementID });
                return list;
            }

            // 按照数据库大致分类，添加Combox的选项
            switch (mysqlDataTypeCategory)
            {
                case MysqlDataTypeCategoryEnum.Integer:
                    {
                        list.Add(new DataGenerateTypeSelectorVO() { Name = "随机整数", DataGenerateType = DataGenerateTypeEnum.RandomInt });
                    }
                    break;
                case MysqlDataTypeCategoryEnum.Real:
                    {
                        list.Add(new DataGenerateTypeSelectorVO() { Name = "随机整数", DataGenerateType = DataGenerateTypeEnum.RandomInt });
                        list.Add(new DataGenerateTypeSelectorVO() { Name = "随机浮点数", DataGenerateType = DataGenerateTypeEnum.RandomFloat });
                    }
                    break;
            }

            return list;
        }


        /// <summary>
        /// 初始化表单界面
        /// </summary>
        /// <param name="dataGenerateConfig">数据生成配置</param>
        private void InitForm(IDataGenerateConfig dataGenerateConfig)
        {
            if (dataGenerateConfig == null)
            {
                this.ShowIntValueForm = false;
                this.ShowRealValueForm = false;
                return;
            }

            // 整数配置初始化
            if (dataGenerateConfig is RandomIntGenerateConfig)
            {
                RandomIntGenerateConfig randomIntGenerateConfig = (RandomIntGenerateConfig)dataGenerateConfig;
                this.IntValueMax = randomIntGenerateConfig.UpperLimit;
                this.IntValueMin = randomIntGenerateConfig.LowerLimit;
                this.ShowIntValueForm = true;
                this.ShowRealValueForm = false;

                return;
            }

            // 实数配置初始化
            if (dataGenerateConfig is RandomFloatGenerateConfig)
            {
                RandomFloatGenerateConfig randomIntGenerateConfig = (RandomFloatGenerateConfig)dataGenerateConfig;
                this.RealValueMax = randomIntGenerateConfig.UpperLimit;
                this.RealValueMin = randomIntGenerateConfig.LowerLimit;
                this.ShowIntValueForm = false;
                this.ShowRealValueForm = true;

                return;
            }
        }


        /// <summary>
        /// 切换数据生成类型表单
        /// </summary>
        /// <param name="selectedDataGenerateType"></param>
        private void SwitchDataGenerateConfigForm(DataGenerateTypeSelectorVO selectedDataGenerateType)
        {
            if (selectedDataGenerateType == null)
            {
                return;
            }

            switch (selectedDataGenerateType.DataGenerateType)
            {
                case DataGenerateTypeEnum.RandomInt:
                    {
                        this.ShowIntValueForm = true;
                        this.ShowRealValueForm = false;
                    };
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        this.ShowIntValueForm = false;
                        this.ShowRealValueForm = true;
                    }
                    break;
                default:
                    {
                        this.ShowIntValueForm = false;
                        this.ShowRealValueForm = false;
                    }
                    break;
            };
        }
        #endregion
    }
}
