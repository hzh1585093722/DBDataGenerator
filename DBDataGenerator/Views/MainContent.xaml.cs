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
    /// MainContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainContent : UserControl
    {

        /// <summary>
        /// 显示的内容
        /// </summary>
        public object DisplayContent
        {
            get { return GetValue(DisplayContentProperty); }
            set { SetValue(DisplayContentProperty, value); }
        }


        /// <summary>
        /// 依赖属性：标识DisplayContent的依赖属性
        /// </summary>
        public static readonly DependencyProperty DisplayContentProperty =
        DependencyProperty.Register(nameof(DisplayContent), typeof(object), typeof(MainContent),
            new FrameworkPropertyMetadata(null, (d, e) =>
            {
                
                MainContent mainContentView = (MainContent)d;
                mainContentView.PART_MainContent.Content = e.NewValue;
            })
        );

        public MainContent()
        {
            InitializeComponent();
        }
    }
}
