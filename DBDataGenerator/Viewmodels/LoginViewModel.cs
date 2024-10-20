using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBDataGenerator.Common;
using DBDataGenerator.Services;
using DBDataGenerator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDataGenerator.Viewmodels
{
    public class LoginViewModel : ObservableObject
    {

        private string _hostName;
        private string _port;
        private string _username;
        private string _password;
        private DataBaseService _dataBaseService;

        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName
        {
            get => _hostName;
            set => SetProperty(ref _hostName, value);
        }

        /// <summary>
        /// 端口
        /// </summary>
        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// 登录数据按钮
        /// </summary>
        public RelayCommand LoginCmd => new RelayCommand(() =>
        {
            try
            {
                _dataBaseService.CreateDataBaseConnection(HostName,Port,Username,Password);
                ViewModelLocator.Instance.MainWindowViewModel.CurrentDisplayContent = new SelectTableView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        public LoginViewModel(DataBaseService dataBaseService)
        {
            HostName = GlobalData.HostName;
            Port = GlobalData.Port;
            Username = GlobalData.Username;
            Password = GlobalData.Password;
            _dataBaseService = dataBaseService;
        }
    }
}
