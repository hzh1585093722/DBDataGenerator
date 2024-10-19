using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels
{
    /// <summary>
    /// 列结构
    /// </summary>
    public class ColumnSchema
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string COLUMN_NAME { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public UInt32? ORDINAL_POSITION { get; set; } = 0;


        /// <summary>
        /// 数据类型
        /// </summary>
        public string COLUMN_TYPE { get; set; }


        /// <summary>
        /// 允许NULL
        /// </summary>
        public string IS_NULLABLE { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string COLUMN_KEY { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        public string COLUMN_COMMENT { get; set; }

        /// <summary>
        /// 字符集
        /// </summary>
        public string CHARACTER_SET_NAME { get; set; }

        /// <summary>
        /// 排序规则
        /// </summary>
        public string COLLATION_NAME { get; set; }


        /// <summary>
        /// 额外信息，如 "auto_increment"
        /// </summary>
        public string EXTRA { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string COLUMN_DEFAULT { get; set; }
    }
}
