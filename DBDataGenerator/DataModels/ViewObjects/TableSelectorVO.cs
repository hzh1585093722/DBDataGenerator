using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDataGenerator.DataModels.ViewObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class TableSelectorVO : ObservableObject
    {
        private bool? _isChecked;
        public string _name;


        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool? IsChecked { get => _isChecked; set => SetProperty(ref _isChecked, value); }

        /// <summary>
        /// 表格名称
        /// </summary>
        public string Name { get => _name; set => SetProperty(ref _name, value); }


        /// <summary>
        /// 将表格实体转为视图对象
        /// </summary>
        /// <param name="tableEntities"></param>
        /// <returns></returns>
        public static ObservableCollection<TableSelectorVO> GetVOsFromEntity(List<TableEntity> tableEntities)
        {
            if (tableEntities == null || tableEntities.Count() <= 0)
            {
                return new ObservableCollection<TableSelectorVO>();
            }

            List<TableSelectorVO> tableSelectorVOs = new List<TableSelectorVO>();
            foreach (TableEntity entity in tableEntities)
            {
                TableSelectorVO vo = new TableSelectorVO() { IsChecked = false, Name = entity.Name };
                tableSelectorVOs.Add(vo);
            }

            return new ObservableCollection<TableSelectorVO>(tableSelectorVOs);
        }
    }
}
