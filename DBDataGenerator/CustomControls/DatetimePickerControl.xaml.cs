using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBDataGenerator.CustomControls
{
    /// <summary>
    /// DatetimePickerControl.xaml 的交互逻辑
    /// </summary>
    public partial class DatetimePickerControl : UserControl
    {

        /// <summary>
        /// 标识 用户选择的日期 的依赖属性
        /// </summary>
        public static readonly DependencyProperty SelectedDatetimeProperty =
            DependencyProperty.Register(
              name: "SelectedDatetime",
              propertyType: typeof(DateTime?),
              ownerType: typeof(DatetimePickerControl),
              typeMetadata: new FrameworkPropertyMetadata(
                  defaultValue: DateTime.Now,
                  propertyChangedCallback: new PropertyChangedCallback((d, e) =>
                  {
                      DatetimePickerControl datetimePickerControl = (DatetimePickerControl)d;
                      DateTime? newDatetimeVal = (DateTime?)e.NewValue;

                      // 年月日
                      datetimePickerControl.calendarControl.SelectedDate = new DateTime(
                        newDatetimeVal.Value.Year,
                        newDatetimeVal.Value.Month,
                        newDatetimeVal.Value.Day);

                      // 时分秒
                      datetimePickerControl.hourSelector.SelectedItem = newDatetimeVal.Value.Hour;
                      datetimePickerControl.minuteSelector.SelectedItem = newDatetimeVal.Value.Minute;
                      datetimePickerControl.secondSelector.SelectedItem = newDatetimeVal.Value.Second;

                      datetimePickerControl.selectedTimeText.Text = newDatetimeVal.Value.ToString("yyyy/MM/dd HH:mm:ss");
                  }))
            );


        /// <summary>
        /// 用户选择的日期
        /// </summary>
        public DateTime? SelectedDatetime
        {
            get => (DateTime?)GetValue(SelectedDatetimeProperty);
            set => SetValue(SelectedDatetimeProperty, value);
        }

        public DatetimePickerControl()
        {
            InitializeComponent();

            this.InitSelector(this.hourSelector, 0, 23);
            this.InitSelector(this.minuteSelector, 0, 59);
            this.InitSelector(this.secondSelector, 0, 59);

        }

        /// <summary>
        /// 控件加载时的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取父窗口并订阅事件
            if (Window.GetWindow(this) is Window parentWindow)
            {
                parentWindow.Activated += ParentWindow_Activated;
                parentWindow.Deactivated += ParentWindow_Deactivated;
            }
        }


        #region 私有方法

        /// <summary>
        /// 窗口被聚焦时的处理逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_Activated(object sender, EventArgs e)
        {
            if(this.selectedTimeText.IsFocused)
            {
                this.datetimePickerPopop.IsOpen = true;
            }
        }


        /// <summary>
        /// 窗口失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParentWindow_Deactivated(object sender, EventArgs e)
        {
            this.datetimePickerPopop.IsOpen = false;
        }

        /// <summary>
        /// 初始化时分秒选择器的itemSource
        /// </summary>
        /// <param name="listBox">下拉框空间</param>
        /// <param name="startVal">下拉框第一个选项的值</param>
        /// <param name="endVal">下拉框最后一个选项的值</param>
        private void InitSelector(ListBox listBox, int startVal, int endVal)
        {
            if (listBox == null)
            {
                return;
            }

            if (startVal > endVal)
            {
                return;
            }

            List<int> list = new List<int>();
            for (int i = startVal; i <= endVal; i++)
            {
                list.Add(i);
            }

            listBox.ItemsSource = list;
        }





        #endregion
        /// <summary>
        /// 文本框获得焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.datetimePickerPopop.IsOpen = true;
        }

        /// <summary>
        /// 文本框失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.datetimePickerPopop.IsOpen = false;
        }

        /// <summary>
        /// 日期控件的值发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calendarControl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // 处理日期变化
            Calendar? calendar = sender as Calendar;
            if (calendar != null)
            {
                DateTime? selectedDate = calendar.SelectedDate;

                if (selectedDate == null || this.SelectedDatetime == null)
                {
                    return;
                }

                this.SelectedDatetime = new DateTime(
                        selectedDate.Value.Year,
                        selectedDate.Value.Month,
                        selectedDate.Value.Day,
                        this.SelectedDatetime.Value.Hour,
                        this.SelectedDatetime.Value.Minute,
                        this.SelectedDatetime.Value.Second);
            }
        }

        private void hourSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null || this.SelectedDatetime == null) { return; }

            int value = (int)listBox.SelectedItem;
            this.SelectedDatetime = new DateTime(
                        this.SelectedDatetime.Value.Year,
                        this.SelectedDatetime.Value.Month,
                        this.SelectedDatetime.Value.Day,
                        value,
                        this.SelectedDatetime.Value.Minute,
                        this.SelectedDatetime.Value.Second);
        }


        private void minuteSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null || this.SelectedDatetime == null) { return; }

            int value = (int)listBox.SelectedItem;
            this.SelectedDatetime = new DateTime(
                        this.SelectedDatetime.Value.Year,
                        this.SelectedDatetime.Value.Month,
                        this.SelectedDatetime.Value.Day,
                        this.SelectedDatetime.Value.Hour,
                        value,
                        this.SelectedDatetime.Value.Second);
        }


        private void secondSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? listBox = sender as ListBox;
            if (listBox == null || listBox.SelectedItem == null || this.SelectedDatetime == null) { return; }

            int value = (int)listBox.SelectedItem;
            this.SelectedDatetime = new DateTime(
                        this.SelectedDatetime.Value.Year,
                        this.SelectedDatetime.Value.Month,
                        this.SelectedDatetime.Value.Day,
                        this.SelectedDatetime.Value.Hour,
                        this.SelectedDatetime.Value.Minute,
                        value);
        }


        /// <summary>
        /// 按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // 获取触发事件的按钮
            Button clickedButton = (Button)sender;
            if (clickedButton == null)
            {
                return;
            }

            switch (clickedButton.Content)
            {
                case "当前时间": this.SelectedDatetime = DateTime.Now; break;
                case "当天开始": this.SelectedDatetime = DateTime.Now.Date; break;
                case "当天结束": this.SelectedDatetime = DateTime.Now.Date.AddDays(1).AddSeconds(-1); break;
            };
        }
    }
}
