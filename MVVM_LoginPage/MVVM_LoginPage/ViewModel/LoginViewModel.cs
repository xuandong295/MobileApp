using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using MVVM_LoginPage.View;
using MVVM_LoginPage.Services;

namespace MVVM_LoginPage.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Command cmdLogin { get; set; }
        public Command cmdCreateAccount { get; set; }
        public Command cmdForgotPassword { get; set; }
        public Command cmdSetting { get; set; }
        ILoginService ilog = DependencyService.Get<ILoginService>();
        public LoginViewModel()
        {
            cmdLogin = new Command(gotoMainPage);
            cmdCreateAccount = new Command(gotoCreateAccount);
            cmdForgotPassword = new Command(gotoForgotPassword);
            cmdSetting = new Command(gotoSetting);
        }

        private void gotoSetting(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new SettingPage());
        }

        private void gotoForgotPassword(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private void gotoCreateAccount(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new CreateAccountPage());

        }

        private void gotoMainPage(object obj)
        {
            if (ilog.login(UserName, Password))
            {
                App.Current.MainPage.Navigation.PushAsync(new MainPage());
            }
            else
            {
                LoginMessage = "Please enter a valid user name and password.";
                TurnLoginMessage = true;
            }
        }
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }
        private string loginMessage;
        public string LoginMessage
        {
            get
            {
                return loginMessage;
            }
            set
            {
                loginMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LoginMessage"));
            }
        }
        private bool turnLoginMessage = false;
        public bool TurnLoginMessage
        {
            get
            {
                return turnLoginMessage;
            }
            set
            {
                turnLoginMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TurnLoginMessage"));
            }
        }
    }

}
