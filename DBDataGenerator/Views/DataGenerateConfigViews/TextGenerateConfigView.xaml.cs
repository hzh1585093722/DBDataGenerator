﻿using DBDataGenerator.DataModels.DataGenerateConfigModels;
using DBDataGenerator.DataModels;
using DBDataGenerator.Viewmodels.DataGenerateConfigViewModels;
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

namespace DBDataGenerator.Views.DataGenerateConfigViews
{
    /// <summary>
    /// TextGenerateConfigView.xaml 的交互逻辑
    /// </summary>
    public partial class TextGenerateConfigView : UserControl
    {
        private readonly TextGenerateConfigViewModel _textGenerateConfigViewModel;


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="columnSchema">列信息</param>
        /// <param name="generateDataConfig">数据生成配置</param>
        /// <param name="onGenerateDataConfigSave">数据生成配置保存时回调，交给调用者来处理储存相关的操作</param>
        public TextGenerateConfigView(ColumnSchema columnSchema,
            ColumnGenerateDataConfig? generateDataConfig,
            Action<ColumnGenerateDataConfig> onGenerateDataConfigSave)
        {
            InitializeComponent();

            this._textGenerateConfigViewModel = (TextGenerateConfigViewModel)this.DataContext;
            // 初始化配置界面
            this._textGenerateConfigViewModel.InitConfigView(columnSchema, generateDataConfig, onGenerateDataConfigSave);
        }
    }
}
