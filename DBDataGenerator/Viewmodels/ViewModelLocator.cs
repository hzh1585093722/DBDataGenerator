using Microsoft.Extensions.DependencyInjection;
using DBDataGenerator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DBDataGenerator.Viewmodels.DataGenerateConfigViewModels;
using DBDataGenerator.Views.DataGenerateConfigViews;

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
            services.AddSingleton<DataGenerateService>();
            services.AddSingleton<NumberGenerateConfigViewModel>();
            services.AddSingleton<DatetimeGenerateConfigViewModel>();
            services.AddSingleton<TextGenerateConfigViewModel>();
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

        /// <summary>
        /// 数据生成服务
        /// </summary>
        public DataGenerateService DataGenerateService => _serviceProvider.GetRequiredService<DataGenerateService>();

        /// <summary>
        /// 视图模型：数值类型生成配置
        /// </summary>
        public NumberGenerateConfigViewModel NumberGenerateConfigViewModel => _serviceProvider.GetRequiredService<NumberGenerateConfigViewModel>();

        /// <summary>
        /// 视图模型：日期类型生成配置
        /// </summary>
        public DatetimeGenerateConfigViewModel DatetimeGenerateConfigViewModel => _serviceProvider.GetRequiredService<DatetimeGenerateConfigViewModel>();

        /// <summary>
        /// 视图模型：文本类型生成配置
        /// </summary>
        public TextGenerateConfigViewModel TextGenerateConfigViewModel => _serviceProvider.GetRequiredService<TextGenerateConfigViewModel>();
    }
}
