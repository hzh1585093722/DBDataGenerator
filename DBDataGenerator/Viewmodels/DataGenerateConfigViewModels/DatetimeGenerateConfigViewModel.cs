using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using DBDataGenerator.DataModels.ViewObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDataGenerator.Viewmodels.DataGenerateConfigViewModels
{
    /// <summary>
    /// 视图模型：日期类型生成配置
    /// </summary>
    public class DatetimeGenerateConfigViewModel : ObservableObject
    {
        private ObservableCollection<DataGenerateTypeSelectorVO> _dataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>();
        private DataGenerateTypeSelectorVO? _selectedDataGenerateType;
        private DateTime? _fixedDatetimeVal = DateTime.Now;

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
            }
        }

        /// <summary>
        /// 生成的固定时间值
        /// </summary>
        public DateTime? FixedDatetimeVal { get => _fixedDatetimeVal; set => SetProperty(ref _fixedDatetimeVal, value); }


        public DatetimeGenerateConfigViewModel()
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
                ColumnGenerateDataConfig? newConfig = new ColumnGenerateDataConfig()
                {
                    DatabaseName = $"{this._generateDataConfig.DatabaseName}",
                    TableName = $"{this._generateDataConfig.TableName}",
                    ColumnName = $"{this._generateDataConfig.ColumnName}",
                    DataGenerateType = this._generateDataConfig.DataGenerateType,
                    DataGenerateConfig = this._generateDataConfig.DataGenerateConfig,
                };

                if (newConfig != null && this.SelectedDataGenerateType != null)
                {
                    newConfig.DataGenerateType = this.SelectedDataGenerateType.DataGenerateType;
                }

                switch (newConfig.DataGenerateType)
                {
                    case DataGenerateTypeEnum.Datetime:
                        {
                            newConfig.DataGenerateConfig = new DatetimeGenerateConfig()
                            {
                                DatabaseName = newConfig.DatabaseName,
                                TableName = newConfig.TableName,
                                ColumnName = newConfig.ColumnName,
                                DateTimeVal = this.FixedDatetimeVal.Value
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
        /// </summary>
        /// <param name="dataGenerateType">数据生成类型</param>
        /// <param name="mysqlDataTypeCategory">Mysql数据类型分类</param>
        /// <returns></returns>
        private List<DataGenerateTypeSelectorVO> InitDataGenerateTypeSelector(DataGenerateTypeEnum dataGenerateType,
            MysqlDataTypeCategoryEnum mysqlDataTypeCategory)
        {
            List<DataGenerateTypeSelectorVO> list = new List<DataGenerateTypeSelectorVO>();

            // 按照数据库大致分类，添加Combox的选项
            switch (mysqlDataTypeCategory)
            {
                case MysqlDataTypeCategoryEnum.Datetime:
                    {
                        list.Add(new DataGenerateTypeSelectorVO() { Name = "日期", DataGenerateType = DataGenerateTypeEnum.Datetime });
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
                return;
            }

            // 整数配置初始化
            if (dataGenerateConfig is DatetimeGenerateConfig)
            {
                DatetimeGenerateConfig datetimeGenerateConfig = (DatetimeGenerateConfig)dataGenerateConfig;
                this.FixedDatetimeVal = datetimeGenerateConfig.DateTimeVal;

                return;
            }

        }

        #endregion
    }
}
