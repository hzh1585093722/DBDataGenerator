using DBDataGenerator.DataModels.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DBDataGenerator.Converters
{
    /// <summary>
    /// 根据生成类型枚举，转换为显示隐藏
    /// </summary>
    public class DataGenerateTypeEnumToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return Visibility.Collapsed;
            }

            DataGenerateTypeEnum dataGenerateType = (DataGenerateTypeEnum)value;
            string conditionStr = (string)parameter;

            switch (conditionStr)
            {
                case "VisibleIfRandomInt": return dataGenerateType == DataGenerateTypeEnum.RandomInt ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfRandomFloat": return dataGenerateType == DataGenerateTypeEnum.RandomFloat ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfJsonObject": return dataGenerateType == DataGenerateTypeEnum.JsonObject ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfJsonArray": return dataGenerateType == DataGenerateTypeEnum.JsonArray ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfBarcode": return dataGenerateType == DataGenerateTypeEnum.Barcode ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfGuid": return dataGenerateType == DataGenerateTypeEnum.Guid ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfDatetime": return dataGenerateType == DataGenerateTypeEnum.Datetime ? Visibility.Visible : Visibility.Collapsed;
                case "VisibleIfFixedString": return dataGenerateType == DataGenerateTypeEnum.FixedString ? Visibility.Visible : Visibility.Collapsed;

                default: return Visibility.Collapsed;
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
