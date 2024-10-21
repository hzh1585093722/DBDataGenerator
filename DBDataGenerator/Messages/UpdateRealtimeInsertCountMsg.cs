using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.Messages
{
    /// <summary>
    /// 当前插入数据库的记录数
    /// </summary>
    public class UpdateRealtimeInsertCountMsg
    {
        public int _count { get; }

        public UpdateRealtimeInsertCountMsg(int count)
        {
            _count = count;
        }
    }
}
