using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.Services;
using DBDataGenerator.Views;
using DBDataGenerator.Views.DataGenerateConfigViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DBDataGenerator.Viewmodels
{
    /// <summary>
    /// 数据生成选项界面
    /// </summary>
    public class DataGenerateViewModel : ObservableObject
    {
        private string _dbName;
        private string _tableName;
        private readonly DataBaseService _dataBaseService;
        private ObservableCollection<ColumnSchema> _columnSchemas = new ObservableCollection<ColumnSchema>();
        private bool _isGenerateDataConfigShow = true;
        private ColumnSchema _selectedColumnSchema;
        private int _generateCount = 10000;
        private ContentControl _displayGenerateConfigView;
        private DataGenerateService _dataGenerateService;
        private DataGenerateConfigService _dataGenerateConfigService;
        private ObservableCollection<ColumnGenerateDataConfig> _dataGenerateConfigs;


        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get => _dbName; set => SetProperty(ref _dbName, value); }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string TableName { get => _tableName; set => SetProperty(ref _tableName, value); }

        /// <summary>
        /// 列字段信息
        /// </summary>
        public ObservableCollection<ColumnSchema> ColumnSchemas { get => _columnSchemas; set => SetProperty(ref _columnSchemas, value); }

        /// <summary>
        /// 是否显示数据生成配置界面，非系统数据库可显示
        /// </summary>
        public bool IsGenerateDataConfigShow { get => _isGenerateDataConfigShow; set => SetProperty(ref _isGenerateDataConfigShow, value); }

        /// <summary>
        /// 选中的列信息
        /// </summary>
        public ColumnSchema SelectedColumnSchema
        {
            get => _selectedColumnSchema;
            set
            {
                SetProperty(ref _selectedColumnSchema, value);
                this.HandleSelectedColumnSchemaChanged(SelectedColumnSchema);// 处理选中项
            }
        }

        /// <summary>
        /// 生成数据条数
        /// </summary>
        public int GenerateCount { get => _generateCount; set => SetProperty(ref _generateCount, value); }

        /// <summary>
        /// 数据表所有列的数据生成配置
        /// </summary>
        public ObservableCollection<ColumnGenerateDataConfig> DataGenerateConfigs { get => _dataGenerateConfigs; set => SetProperty(ref _dataGenerateConfigs, value); }

        /// <summary>
        /// 数据配置编辑界面，可切换
        /// </summary>
        public ContentControl DisplayGenerateConfigView { get => _displayGenerateConfigView; set => SetProperty(ref _displayGenerateConfigView, value); }


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataBaseService">数据库服务</param>
        /// <param name="dataGenerateService">数据生成服务</param>
        /// <param name="dataGenerateConfigService">数据生成配置服务</param>
        public DataGenerateViewModel(DataBaseService dataBaseService, 
            DataGenerateService dataGenerateService,
            DataGenerateConfigService dataGenerateConfigService)
        {
            this._dataBaseService = dataBaseService;
            this._dataGenerateService = dataGenerateService;
            _dataGenerateConfigService = dataGenerateConfigService;
        }


        /// <summary>
        ///  获取表格的列数据信息
        /// </summary>
        public void InitColumnInfoView()
        {
            try
            {
                List<ColumnSchema> columnSchemas = this._dataBaseService.GetDatabaseColumnSchemas(this.DbName, this.TableName);
                this.ColumnSchemas = new ObservableCollection<ColumnSchema>(columnSchemas);

                // 如果是数据库是系统数据库，禁止生成数据到数据库中
                if (IsMysqlSystemDatabase(this.DbName))
                {
                    this.IsGenerateDataConfigShow = false;
                    return;
                }
                else
                {
                    this.IsGenerateDataConfigShow = true;
                }

                // 初始化列数据生成配置
                List<ColumnGenerateDataConfig> list = this._dataGenerateConfigService.GetColumnGenerateDataConfigs(this.DbName, this.TableName, columnSchemas);
                this.DataGenerateConfigs = new ObservableCollection<ColumnGenerateDataConfig>(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 按钮：跳转登录页面
        /// </summary>
        /// <returns></returns>
        public RelayCommand SwitchToSelectTableViewCmd => new RelayCommand(() =>
        {
            try
            {
                ViewModelLocator.Instance.MainWindowViewModel.CurrentDisplayContent = new SelectTableView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        /// <summary>
        /// 按钮： 确认生成数据
        /// </summary>
        /// <returns></returns>
        public RelayCommand ConfirmDataGenerateCmd => new RelayCommand(() =>
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });


        #region 私有方法

        /// <summary>
        /// 是否为系统数据库。mysql，information_schema，performance_schema，这3张表是系统表，禁止修改
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private bool IsMysqlSystemDatabase(string databaseName)
        {
            if (databaseName == "mysql")
            {
                return true;
            }

            if (databaseName == "information_schema")
            {
                return true;
            }

            if (databaseName == "performance_schema")
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// 用户选中一个字段时的处理方法
        /// </summary>
        /// <param name="selectedColumnSchema"></param>
        private void HandleSelectedColumnSchemaChanged(ColumnSchema selectedColumnSchema)
        {
            try
            {
                if (selectedColumnSchema == null)
                {
                    return;
                }

                // 在生成配置列表获取选中项的数据生成配置
                ColumnGenerateDataConfig? generateDataConfig = this.DataGenerateConfigs.Where(x => x.ColumnName == selectedColumnSchema.COLUMN_NAME).FirstOrDefault();
                if (generateDataConfig == null)
                {
                    MessageBox.Show($"找不到列【{selectedColumnSchema.COLLATION_NAME}】的列生成配置信息，请退出并重新进入该界面后重试", "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 根据类型分类,加载相关配置界面
                switch (generateDataConfig.MysqlDataTypeCategoryEnum)
                {
                    case DataModels.Enums.MysqlDataTypeCategoryEnum.Integer:
                        this.DisplayGenerateConfigView = new NumberGenerateConfigView(selectedColumnSchema, generateDataConfig, (config) =>
                        {
                            // 更新配置
                            this._dataGenerateConfigService.UpdateColumnGenerateDataConfig(this.DbName,this.TableName, 
                                config,this.DataGenerateConfigs.ToList());
                        });
                        break;
                    case DataModels.Enums.MysqlDataTypeCategoryEnum.Real:
                        this.DisplayGenerateConfigView = new NumberGenerateConfigView(selectedColumnSchema, generateDataConfig, (config) =>
                        {
                            // 更新配置
                            this._dataGenerateConfigService.UpdateColumnGenerateDataConfig(this.DbName, this.TableName,
                                config, this.DataGenerateConfigs.ToList());
                        });
                        break;
                    case DataModels.Enums.MysqlDataTypeCategoryEnum.Text: this.DisplayGenerateConfigView = new TextGenerateConfigView(selectedColumnSchema, generateDataConfig, (config) =>
                        {
                            // 更新配置
                            this._dataGenerateConfigService.UpdateColumnGenerateDataConfig(this.DbName, this.TableName,
                                config, this.DataGenerateConfigs.ToList());
                        });
                        break;
                    case DataModels.Enums.MysqlDataTypeCategoryEnum.Datetime:
                        this.DisplayGenerateConfigView = new DatetimeGenerateConfigView(selectedColumnSchema, generateDataConfig, (config) =>
                        {
                            // 更新配置
                            this._dataGenerateConfigService.UpdateColumnGenerateDataConfig(this.DbName, this.TableName,
                                config, this.DataGenerateConfigs.ToList());
                        });
                        break;
                    default: this.DisplayGenerateConfigView = null; break;
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}
