﻿using DBDataGenerator.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBDataGenerator.Views
{
    /// <summary>
    /// DataGenerateView.xaml 的交互逻辑
    /// </summary>
    public partial class DataGenerateView : UserControl
    {
        /// <summary>
        /// 数据上下文对象
        /// </summary>
        private readonly DataGenerateViewModel _dataGenerateViewModel;

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="tableName">表格名称</param>
        public DataGenerateView(string dbName, string tableName)
        {
            InitializeComponent();

            this._dataGenerateViewModel = (DataGenerateViewModel)this.DataContext;// 绑定数据上下文
            this._dataGenerateViewModel.TableName = tableName;
            this._dataGenerateViewModel.DbName = dbName;
            this._dataGenerateViewModel.InitColumnInfoView();// 初始化界面
        }
    }
}
