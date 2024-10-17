using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.DataGenerateConfigModels
{
    /// <summary>
    /// 数据生成配置抽象类
    /// </summary>
    public interface IDataGenerateConfig
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>
        /// 数据表名
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        string ColumnName { get; set; }
    }
}
