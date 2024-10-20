using DBDataGenerator.DataModels;
using DBDataGenerator.Viewmodels;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// SelectTableView.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTableView : UserControl
    {
        private readonly SelectTableViewModel _SelectTableViewModel;
        public SelectTableView()
        {
            InitializeComponent();
            _SelectTableViewModel = (SelectTableViewModel)this.DataContext;

            ReloadDataBaseList();
        }

        /// <summary>
        /// 加载数据库列表
        /// </summary>
        private void ReloadDataBaseList() {
            _SelectTableViewModel.ReloadDataBaseList();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var a =(ListBox)sender;
            var selectedDatabase = (DatabaseEntity)a.SelectedItem;
            _SelectTableViewModel.SelectedDatabase = selectedDatabase;
            _SelectTableViewModel.ReloadTables(selectedDatabase);
        }
    }
}
