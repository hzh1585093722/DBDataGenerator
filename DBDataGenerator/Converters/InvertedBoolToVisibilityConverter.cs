using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace DBDataGenerator.Converters
{
    /// <summary>
    /// bool为False时，显示UI
    /// </summary>
    public class InvertedBoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// bool转Visibility，后台数据更新时用
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // 当 bool 为 false 时返回 Visible，为 true 时返回 Collapsed
                return boolValue ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible; // 默认返回值
        }


        /// <summary>
        /// Visibility转bool，前台视图更新，自动更新后台数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                // 将 Visibility 反向转换为 bool，Visible 转为 false，Collapsed 转为 true
                return visibility != Visibility.Visible;
            }

            return true;
        }
    }
}
