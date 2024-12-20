﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels.Enums;
using DBDataGenerator.DataModels.ViewObjects;
using DBDataGenerator.Views.DataGenerateConfigViews;
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
    /// 视图模型：文本类型生成配置
    /// </summary>
    public class TextGenerateConfigViewModel : ObservableObject
    {
        private ObservableCollection<DataGenerateTypeSelectorVO> _dataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>();
        private DataGenerateTypeSelectorVO? _selectedDataGenerateType;
        private TextGenerateFormVO? _textGenerateFormVO = new TextGenerateFormVO();
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
        /// 用户输入内容
        /// </summary>
        public TextGenerateFormVO? TextGenerateFormVO { get => _textGenerateFormVO; set => SetProperty(ref _textGenerateFormVO, value); }

        #region 命令

        /// <summary>
        /// 保存JSON属性
        /// </summary>
        public RelayCommand SaveJsonPropertyCmd => new RelayCommand(() =>
        {
            try
            {
                if (this.TextGenerateFormVO == null || this.TextGenerateFormVO.SelectedJsonProperty == null)
                {
                    MessageBox.Show("请选中1个JSON属性进行操作", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // 更新数据生成类型
                if (this.TextGenerateFormVO.SelectedJsonPropertyDataGenerateType != null)
                {
                    this.TextGenerateFormVO.SelectedJsonProperty.DataGenerateType = this.TextGenerateFormVO.SelectedJsonPropertyDataGenerateType.DataGenerateType;
                }

                // 更新生成配置
                if (this.TextGenerateFormVO.EditingJsonProperty != null)
                {
                    this.TextGenerateFormVO.SelectedJsonProperty.PropertyValueConfig = this.UpdateJsonPropertyDataGenerateConfig(this.TextGenerateFormVO.SelectedJsonProperty.DataGenerateType,
                    this.TextGenerateFormVO.EditingJsonProperty);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        /// <summary>
        /// 添加JSON属性
        /// </summary>
        public RelayCommand AddJsonPropertyCmd => new RelayCommand(() =>
        {
            try
            {
                if (this.TextGenerateFormVO == null)
                {
                    MessageBox.Show("初始化JSON信息出错，请退出后重新进入该界面", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                JsonPropertyFormWindow jsonPropertyFormWindow = new JsonPropertyFormWindow(new JsonPropertiesConfig(), (newName) =>
                {
                    this.TextGenerateFormVO.Properties.Add(new JsonPropertiesConfig() { PropertyName = newName });
                    return true;
                });
                jsonPropertyFormWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        /// <summary>
        /// 删除JSON属性
        /// </summary>
        public RelayCommand DeleteJsonPropertyCmd => new RelayCommand(() =>
        {
            try
            {
                if (this.TextGenerateFormVO == null || this.TextGenerateFormVO.SelectedJsonProperty == null)
                {
                    MessageBox.Show("请选中1个JSON属性进行操作", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                JsonPropertiesConfig? deleteItem = this.TextGenerateFormVO.Properties.FirstOrDefault(x => x.PropertyName == this.TextGenerateFormVO.SelectedJsonProperty.PropertyName);

                if (deleteItem != null)
                {
                    this.TextGenerateFormVO.Properties.Remove(deleteItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        /// <summary>
        /// 重命名JSON属性
        /// </summary>
        public RelayCommand ModifyJsonPropertyCmd => new RelayCommand(() =>
        {
            try
            {
                if (this.TextGenerateFormVO == null || this.TextGenerateFormVO.SelectedJsonProperty == null)
                {
                    MessageBox.Show("请选中1个JSON属性进行操作", "消息", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                JsonPropertyFormWindow jsonPropertyFormWindow = new JsonPropertyFormWindow(this.TextGenerateFormVO.SelectedJsonProperty,
                    (newName) =>
                    {
                        JsonPropertiesConfig? modifyItem = this.TextGenerateFormVO.Properties.FirstOrDefault(x => x.PropertyName == this.TextGenerateFormVO.SelectedJsonProperty.PropertyName);

                        if (modifyItem != null)
                        {
                            modifyItem.PropertyName = newName;
                        }

                        return true;
                    }
                    );
                jsonPropertyFormWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

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

                // 更新数据生成类型
                if (newConfig != null && this.SelectedDataGenerateType != null)
                {
                    newConfig.DataGenerateType = this.SelectedDataGenerateType.DataGenerateType;
                }

                // 更新数据生成配置
                if (this.TextGenerateFormVO != null)
                {
                    newConfig.DataGenerateConfig = this.UpdateDataGenerateConfig(newConfig, this.TextGenerateFormVO);
                }

                this._onGenerateDataConfigSave?.Invoke(newConfig);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });
        #endregion




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

            // 暂时先支持：随机整数、随机浮点数、JSON对象、JSON数组、条码、唯一ID、时间、固定字符串
            list.Add(new DataGenerateTypeSelectorVO() { Name = "随机整数", DataGenerateType = DataGenerateTypeEnum.RandomInt });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "随机浮点数", DataGenerateType = DataGenerateTypeEnum.RandomFloat });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "JSON对象", DataGenerateType = DataGenerateTypeEnum.JsonObject });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "JSON数组", DataGenerateType = DataGenerateTypeEnum.JsonArray });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "条码", DataGenerateType = DataGenerateTypeEnum.Barcode });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "唯一ID", DataGenerateType = DataGenerateTypeEnum.Guid });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "时间", DataGenerateType = DataGenerateTypeEnum.Datetime });
            list.Add(new DataGenerateTypeSelectorVO() { Name = "固定字符串", DataGenerateType = DataGenerateTypeEnum.FixedString });

            return list;
        }


        /// <summary>
        /// 初始化表单界面
        /// </summary>
        /// <param name="dataGenerateConfig">数据生成配置</param>
        private void InitForm(IDataGenerateConfig dataGenerateConfig)
        {
            // 清空表单的选中的JSON属性对象
            if (this.TextGenerateFormVO != null)
            {
                this.TextGenerateFormVO.SelectedJsonProperty = null;
            }

            if (dataGenerateConfig == null || this.TextGenerateFormVO == null)
            {
                return;
            }

            // 整数配置初始化
            if (dataGenerateConfig is RandomIntGenerateConfig)
            {
                RandomIntGenerateConfig randomIntGenerateConfig = (RandomIntGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.IntValueMax = randomIntGenerateConfig.UpperLimit;
                this.TextGenerateFormVO.IntValueMin = randomIntGenerateConfig.LowerLimit;

                return;
            }

            // 实数配置初始化
            if (dataGenerateConfig is RandomFloatGenerateConfig)
            {
                RandomFloatGenerateConfig randomIntGenerateConfig = (RandomFloatGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.RealValueMax = randomIntGenerateConfig.UpperLimit;
                this.TextGenerateFormVO.RealValueMin = randomIntGenerateConfig.LowerLimit;

                return;
            }

            // JSON对象配置初始化
            if (dataGenerateConfig is JsonObjectGenerateConfig)
            {
                JsonObjectGenerateConfig jsonObjectGenerateConfig = (JsonObjectGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.Properties = new ObservableCollection<JsonPropertiesConfig>(jsonObjectGenerateConfig.Properties);

                return;
            }

            // JSON数组配置初始化
            if (dataGenerateConfig is JsonArrayGenerateConfig)
            {
                JsonArrayGenerateConfig jsonArrayGenerateConfig = (JsonArrayGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.Properties = new ObservableCollection<JsonPropertiesConfig>(jsonArrayGenerateConfig.JsonObjectConfig.Properties);
                this.TextGenerateFormVO.JsonArrayCount = jsonArrayGenerateConfig.JsonArrayCount;

                return;
            }

            // 条码配置初始化
            if (dataGenerateConfig is BarcodeGenerateConfig)
            {
                BarcodeGenerateConfig barcodeGenerateConfig = (BarcodeGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.Prefix = barcodeGenerateConfig.Prefix;
                this.TextGenerateFormVO.SuffixNum = barcodeGenerateConfig.SuffixNum;

                return;
            }

            // 时间配置初始化
            if (dataGenerateConfig is DatetimeGenerateConfig)
            {
                DatetimeGenerateConfig datetimeGenerateConfig = (DatetimeGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.FixedDatetimeVal = datetimeGenerateConfig.DateTimeVal;

                return;
            }

            // 固定字符串配置初始化
            if (dataGenerateConfig is FixedStringGenerateConfig)
            {
                FixedStringGenerateConfig fixedStringGenerateConfig = (FixedStringGenerateConfig)dataGenerateConfig;
                this.TextGenerateFormVO.StringVal = fixedStringGenerateConfig.StringVal;

                return;
            }
        }


        /// <summary>
        /// 更新列数据生成配置
        /// </summary>
        /// <returns></returns>
        private IDataGenerateConfig UpdateDataGenerateConfig(
            ColumnGenerateDataConfig newConfig,
            TextGenerateFormVO editingVal)
        {
            IDataGenerateConfig dataGenerateConfig = null;

            if (editingVal == null || newConfig == null)
            {
                return dataGenerateConfig;
            }

            switch (newConfig.DataGenerateType)
            {
                case DataGenerateTypeEnum.RandomInt:
                    {
                        dataGenerateConfig = new RandomIntGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            LowerLimit = editingVal.IntValueMin.HasValue ? editingVal.IntValueMin.Value : 0,
                            UpperLimit = editingVal.IntValueMax.HasValue ? editingVal.IntValueMax.Value : 100,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        dataGenerateConfig = new RandomFloatGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            LowerLimit = editingVal.RealValueMin.HasValue ? editingVal.RealValueMin.Value : 0,
                            UpperLimit = editingVal.RealValueMax.HasValue ? editingVal.RealValueMax.Value : 100,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.JsonObject:
                    {
                        dataGenerateConfig = new JsonObjectGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            Properties = editingVal.Properties.ToList(),
                        };
                    }
                    break;
                case DataGenerateTypeEnum.JsonArray:
                    {
                        dataGenerateConfig = new JsonArrayGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            JsonArrayCount = editingVal.JsonArrayCount,
                            JsonObjectConfig = new JsonObjectGenerateConfig() { Properties = editingVal.Properties.ToList() },
                        };
                    }
                    break;
                case DataGenerateTypeEnum.Barcode:
                    {
                        dataGenerateConfig = new BarcodeGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            Prefix = editingVal.Prefix,
                            SuffixNum = editingVal.SuffixNum,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.Guid: break;
                case DataGenerateTypeEnum.Datetime:
                    {
                        dataGenerateConfig = new DatetimeGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            DateTimeVal = editingVal.FixedDatetimeVal.HasValue ? editingVal.FixedDatetimeVal.Value : DateTime.Now,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.FixedString:
                    {
                        dataGenerateConfig = new FixedStringGenerateConfig()
                        {
                            DatabaseName = newConfig.DatabaseName,
                            TableName = newConfig.TableName,
                            ColumnName = newConfig.ColumnName,
                            StringVal = editingVal.StringVal
                        };
                    }
                    break;
                default: break;
            };

            return dataGenerateConfig;
        }


        /// <summary>
        /// 更新JSON属性的数据生成配置
        /// </summary>
        /// <param name="dataGenerateType"></param>
        /// <param name="editingVal"></param>
        /// <returns>更新后的配置</returns>
        private IDataGenerateConfig UpdateJsonPropertyDataGenerateConfig(
            DataGenerateTypeEnum dataGenerateType,
            JsonPropertyVO editingVal)
        {
            IDataGenerateConfig dataGenerateConfig = null;

            if (editingVal == null)
            {
                return dataGenerateConfig;
            }

            switch (dataGenerateType)
            {
                case DataGenerateTypeEnum.RandomInt:
                    {
                        dataGenerateConfig = new RandomIntGenerateConfig()
                        {
                            LowerLimit = editingVal.IntValueMin.HasValue ? editingVal.IntValueMin.Value : 0,
                            UpperLimit = editingVal.IntValueMax.HasValue ? editingVal.IntValueMax.Value : 100,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.RandomFloat:
                    {
                        dataGenerateConfig = new RandomFloatGenerateConfig()
                        {
                            LowerLimit = editingVal.RealValueMin.HasValue ? editingVal.RealValueMin.Value : 0,
                            UpperLimit = editingVal.RealValueMax.HasValue ? editingVal.RealValueMax.Value : 100,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.Barcode:
                    {
                        dataGenerateConfig = new BarcodeGenerateConfig()
                        {
                            Prefix = editingVal.Prefix,
                            SuffixNum = editingVal.SuffixNum,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.Guid: break;
                case DataGenerateTypeEnum.Datetime:
                    {
                        dataGenerateConfig = new DatetimeGenerateConfig()
                        {
                            DateTimeVal = editingVal.FixedDatetimeVal.HasValue ? editingVal.FixedDatetimeVal.Value : DateTime.Now,
                        };
                    }
                    break;
                case DataGenerateTypeEnum.FixedString:
                    {
                        dataGenerateConfig = new FixedStringGenerateConfig() { StringVal = editingVal.StringVal };
                    }
                    break;
                default: break;
            };

            return dataGenerateConfig;
        }
        #endregion
    }
}
