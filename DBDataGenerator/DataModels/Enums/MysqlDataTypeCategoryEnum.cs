using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.Enums
{
    /// <summary>
    /// Mysql数据类型分类，将类型大致分为：数值、文本、日期类型，减少逻辑判断
    /// </summary>
    public enum MysqlDataTypeCategoryEnum
    {
        /// <summary>
        /// 整数类型
        /// </summary>
        Integer = 0,

        /// <summary>
        /// 实数类型
        /// </summary>
        Real = 1,

        /// <summary>
        /// 文本类型
        /// </summary>
        Text = 2,

        /// <summary>
        /// 日期类型
        /// </summary>
        Datetime = 3
    }
}
