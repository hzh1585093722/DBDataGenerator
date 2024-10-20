using DBDataGenerator.DataModels.DataGenerateConfigModels;
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
using System.Windows.Shapes;

namespace DBDataGenerator.Views.DataGenerateConfigViews
{
    /// <summary>
    /// JsonPropertyFormWindow.xaml 的交互逻辑
    /// </summary>
    public partial class JsonPropertyFormWindow : Window
    {
        private readonly Func<string, bool> _saveButtonCallback;

        public JsonPropertyFormWindow(JsonPropertiesConfig jsonPropertiesConfig, Func<string, bool> saveButtonCallback)
        {
            InitializeComponent();

            this.inputPropertyName.Text = jsonPropertiesConfig.PropertyName;
            this._saveButtonCallback = saveButtonCallback;
        }

        /// <summary>
        /// 按下保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string inputText = this.inputPropertyName.Text;// 获取用户输入的内容
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("请输入属性名", "消息", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 执行用户的回调，获取结果，结果为true时关闭窗口
            bool? result = this._saveButtonCallback?.Invoke(inputText.Trim());
            if (result.HasValue && result == true)
            {
                this.Close();
                return;
            }
        }
    }
}
