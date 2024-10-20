using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.Enums
{
    /// <summary>
    /// 数据生成类型枚举
    /// </summary>
    public enum DataGenerateTypeEnum
    {
        /// <summary>
        /// 自增ID主键
        /// </summary>
        AutoIncrementID = 0,

        /// <summary>
        /// 随机整数
        /// </summary>
        RandomInt = 1,

        /// <summary>
        /// 随机浮点数
        /// </summary>
        RandomFloat = 2,

        /// <summary>
        /// JSON对象
        /// </summary>
        JsonObject = 3,

        /// <summary>
        /// JSON数组
        /// </summary>
        JsonArray = 4,

        /// <summary>
        /// 条码
        /// </summary>
        Barcode = 5,

        /// <summary>
        /// 唯一ID
        /// </summary>
        Guid = 6,

        /// <summary>
        /// 时间
        /// </summary>
        Datetime = 7,

        /// <summary>
        /// 固定字符串
        /// </summary>
        FixedString = 8,

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        UserData = 9
    }
}
