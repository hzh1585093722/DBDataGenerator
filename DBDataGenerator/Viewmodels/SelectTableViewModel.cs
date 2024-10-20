using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using NPOI.HSSF.UserModel;
using DBDataGenerator.DataModels;
using DBDataGenerator.DataModels.ViewObjects;
using DBDataGenerator.Services;
using DBDataGenerator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDataGenerator.Viewmodels
{
    public class SelectTableViewModel : ObservableObject
    {
        private DataBaseService _dataBaseService;
        private ObservableCollection<DatabaseEntity> _dbList;
        private ObservableCollection<TableSelectorVO> _tables;
        private DatabaseEntity _selectedDatabase;
        private TableSelectorVO _selectedTable;

        /// <summary>
        /// 数据库列表
        /// </summary>
        public ObservableCollection<DatabaseEntity> DbList
        {
            get => _dbList;
            set => SetProperty(ref _dbList, value);
        }

        /// <summary>
        /// 选中的数据库
        /// </summary>
        public DatabaseEntity SelectedDatabase
        {
            get => _selectedDatabase;
            set => SetProperty(ref _selectedDatabase, value);
        }

        /// <summary>
        /// 表列表
        /// </summary>
        public ObservableCollection<TableSelectorVO> Tables
        {
            get => _tables;
            set => SetProperty(ref _tables, value);
        }

        /// <summary>
        /// 选中的数据库
        /// </summary>
        public TableSelectorVO SelectedTable
        {
            get => _selectedTable;
            set => SetProperty(ref _selectedTable, value);
        }

        /// <summary>
        /// 按钮：跳转登录页面
        /// </summary>
        /// <returns></returns>
        public RelayCommand SwitchToLoginViewCmd => new RelayCommand(() =>
        {
            try
            {
                _dataBaseService.DisconnectDataBase();
                ViewModelLocator.Instance.MainWindowViewModel.CurrentDisplayContent = new LoginView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        /// <summary>
        /// 生成表格数据界面
        /// </summary>
        public RelayCommand GenerateTableDataCmd => new RelayCommand(() =>
        {
            try
            {
                if (SelectedTable == null)
                {
                    MessageBox.Show("请选择要生成数据的表格", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 跳转到界面
                DataGenerateView dataGenerateView = new DataGenerateView(SelectedDatabase.SCHEMA_NAME, SelectedTable.Name);
                ViewModelLocator.Instance.MainWindowViewModel.CurrentDisplayContent = dataGenerateView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });



        /// <summary>
        /// 加载数据库列表
        /// </summary>
        public void ReloadDataBaseList()
        {
            DbList = new ObservableCollection<DatabaseEntity>(_dataBaseService.GetDatabases());
        }


        /// <summary>
        /// 加载数据库列表
        /// </summary>
        public void ReloadTables(DatabaseEntity databaseEntity)
        {
            Tables = TableSelectorVO.GetVOsFromEntity(_dataBaseService.GetTables(databaseEntity));
        }


        public SelectTableViewModel(DataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
    }
}
