using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.ViewObjects
{
    /// <summary>
    /// 编辑中的Json属性
    /// </summary>
    public class JsonPropertyVO : ObservableObject
    {
        private int? _intValueMin = 0;
        private int? _intValueMax = 100;
        private double? _realValueMin = 0.0;
        private double? _realValueMax = 100.0;
        private string _prefix = "TEST";
        private int _suffixNum = 10000;
        private DateTime? _fixedDatetimeVal = DateTime.Now;
        private string _stringVal = "";

        /// <summary>
        /// 随机整数下限
        /// </summary>
        public int? IntValueMin { get => _intValueMin; set => SetProperty(ref _intValueMin, value); }

        /// <summary>
        /// 随机整数上限
        /// </summary>
        public int? IntValueMax { get => _intValueMax; set => SetProperty(ref _intValueMax, value); }

        /// <summary>
        /// 随机实数下限
        /// </summary>
        public double? RealValueMin { get => _realValueMin; set => SetProperty(ref _realValueMin, value); }

        /// <summary>
        /// 随机实数上限
        /// </summary>
        public double? RealValueMax { get => _realValueMax; set => SetProperty(ref _realValueMax, value); }

        /// <summary>
        /// 条码前缀
        /// </summary>
        public string Prefix { get => _prefix; set => SetProperty(ref _prefix, value); }

        /// <summary>
        /// 条码后缀数字
        /// </summary>
        public int SuffixNum { get => _suffixNum; set => SetProperty(ref _suffixNum, value); }

        /// <summary>
        /// 固定时间
        /// </summary>
        public DateTime? FixedDatetimeVal { get => _fixedDatetimeVal; set => SetProperty(ref _fixedDatetimeVal, value); }

        /// <summary>
        /// 固定字符串
        /// </summary>
        public string StringVal { get => _stringVal; set => SetProperty(ref _stringVal, value); }
    }
}
