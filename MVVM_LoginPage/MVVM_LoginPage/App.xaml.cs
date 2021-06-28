using MVVM_LoginPage.Services;
using MVVM_LoginPage.View;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace MVVM_LoginPage
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<ILoginService, loginService>();
            DependencyService.Register<ILedService, ledService>();
            DependencyService.Register<IHistoryService, historyService>();
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
