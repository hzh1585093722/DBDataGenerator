using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels
{
    /// <summary>
    /// 数据库表描述
    /// </summary>
    public class TableSchema
    {
        public string TABLE_NAME { get; set; }
        public string ENGINE { get; set; }
        public UInt64? AUTO_INCREMENT { get; set; } = 0;
        public UInt64? DATA_LENGTH { get; set; } = 0;

        public string TABLE_COMMENT { get; set; }
    }
}
