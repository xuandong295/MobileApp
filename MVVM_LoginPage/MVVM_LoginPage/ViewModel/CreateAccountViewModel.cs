using MVVM_LoginPage.Model;
using MVVM_LoginPage.Services;
using MVVM_LoginPage.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MVVM_LoginPage.ViewModel
{
    public class CreateAccountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ILoginService ilog = DependencyService.Get<ILoginService>();
        public Command cmdCreate { get; set; }
        public CreateAccountViewModel()
        {
            cmdCreate = new Command(gotoLogin);
        }

        private void gotoLogin(object obj)
        {
            if ((cre_Password!="")&&(cre_Username!="")) //Please enter a valid user name and password
            {
                if (ilog.cmpPassRepass(cre_Password, cre_RePassword))
                {
                    if (ilog.checkDuplicateUsername(cre_Username)) //Please enter another username
                    {
                        ilog.addData(cre_Username, cre_Password);
                        //ilog.addData("xuan2", "xuan2");
                        ilog.LoadData();
                        App.Current.MainPage.Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        CreateMessage = "Please enter another username.";
                        TurnCreateMessage = true;
                    }
                }
                else
                {
                    CreateMessage = "Those passwords didn’t match. Try again.";
                    TurnCreateMessage = true;
                }
            }
            else
            {
                CreateMessage = "Please enter a valid user name and password.";
                TurnCreateMessage = true;
            }

        }
        private string cre_username;
        public string cre_Username
        {
            get { return cre_username; }
            set
            {
                cre_username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("cre_UserName"));
            }
        }
        private string cre_password;
        public string cre_Password
        {
            get { return cre_password; }
            set
            {
                cre_password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("cre_Password"));
            }
        }
        private string cre_rePassword;
        public string cre_RePassword
        {
            get { return cre_rePassword; }
            set
            {
                cre_rePassword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("cre_RePassword"));
            }
        }
        private string createMessage;
        public string CreateMessage
        {
            get
            {
                return createMessage;
            }
            set
            {
                createMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CreateMessage"));
            }
        }
        private bool turnCreateMessage = false;
        public bool TurnCreateMessage
        {
            get
            {
                return turnCreateMessage;
            }
            set
            {
                turnCreateMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TurnCreateMessage"));
            }
        }
    }
}
