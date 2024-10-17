using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBDataGenerator.DataModels;
using DBDataGenerator.Services;
using DBDataGenerator.Views;
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
        public ColumnSchema SelectedColumnSchema { get => _selectedColumnSchema; set => SetProperty(ref _selectedColumnSchema, value); }

        /// <summary>
        /// 生成数据条数
        /// </summary>
        public int GenerateCount { get => _generateCount; set => SetProperty(ref _generateCount, value); }

        /// <summary>
        /// 数据配置编辑界面，可切换
        /// </summary>
        public ContentControl DisplayGenerateConfigView { get => _displayGenerateConfigView; set => SetProperty(ref _displayGenerateConfigView, value); }

        public DataGenerateViewModel(DataBaseService dataBaseService)
        {
            this._dataBaseService = dataBaseService;

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
        #endregion
    }
}
