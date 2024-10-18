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

namespace DBDataGenerator.Viewmodels.DataGenerateConfigViewModels
{
    /// <summary>
    /// 视图模型：数值类型生成配置
    /// </summary>
    public class NumberGenerateConfigViewModel : ObservableObject
    {
        private ObservableCollection<DataGenerateTypeSelectorVO> _dataGenerateTypeList = new ObservableCollection<DataGenerateTypeSelectorVO>();
        private DataGenerateTypeSelectorVO? _selectedDataGenerateType;

        /// <summary>
        /// 数据生成类型选项
        /// </summary>
        public ObservableCollection<DataGenerateTypeSelectorVO> DataGenerateTypeList { get => _dataGenerateTypeList; set => SetProperty(ref _dataGenerateTypeList, value); }

        /// <summary>
        /// 选中的数据生成选项
        /// </summary>
        public DataGenerateTypeSelectorVO? SelectedDataGenerateType { get => _selectedDataGenerateType; set => SetProperty(ref _selectedDataGenerateType, value); }

        public NumberGenerateConfigViewModel()
        {

        }


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

        #endregion
    }
}
