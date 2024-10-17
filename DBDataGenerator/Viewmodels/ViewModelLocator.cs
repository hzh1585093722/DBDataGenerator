using Microsoft.Extensions.DependencyInjection;
using DBDataGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DBDataGenerator.Viewmodels
{
    public class ViewModelLocator
    {
        private IServiceProvider _serviceProvider;

        public static ViewModelLocator Instance => new Lazy<ViewModelLocator>(() =>Application.Current.TryFindResource("Locator") as ViewModelLocator)?.Value;

        public ViewModelLocator() {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        // 注册你的服务
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<DataBaseService>();
            services.AddSingleton<SelectTableService>();
            services.AddSingleton<SelectTableViewModel>();
            services.AddSingleton<DataGenerateViewModel>();
        }


        /// <summary>
        /// 登录页面ViewModel
        /// </summary>
        public LoginViewModel LoginViewModel => _serviceProvider.GetRequiredService<LoginViewModel>();

        /// <summary>
        /// 主页面ViewModel
        /// </summary>
        public MainWindowViewModel MainWindowViewModel => _serviceProvider.GetRequiredService<MainWindowViewModel>();

        /// <summary>
        /// 数据库表格选择界面
        /// </summary>
        public SelectTableViewModel SelectTableViewModel => _serviceProvider.GetRequiredService<SelectTableViewModel>();

        /// <summary>
        /// 数据生成选项ViewModel
        /// </summary>
        public DataGenerateViewModel DataGenerateViewModel => _serviceProvider.GetRequiredService<DataGenerateViewModel>();    
    }
}
