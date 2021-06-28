using MVVM_LoginPage.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_LoginPage.Services
{
    public class loginService : ILoginService
    {
        private DataHandler dataAccess;
        List<UserModel> userList = new List<UserModel>();
        public loginService()
        {

            this.dataAccess = new DataHandler();
            /*        userList.Add(new UserModel { userName = "admin", password = "admin" });
                    userList.Add(new UserModel { userName = "xuandong", password = "xuandong" });*/
            LoadData();
        }
        public bool login(string username, string password)
        {
            foreach (var user in userList)
            {
                if (username == user.userName && password == user.password)
                {
                    return true;
                }
            }
            return false;
        }
        public void LoadData()
        {
            foreach (var user in dataAccess.GetAllAccountToList())
            {
                userList.Add(new UserModel { userName = user.userName, password = user.password });
            }
        }

        public void addData(string username, string password)
        {
            dataAccess.AddAccount(new UserModel { userName = username, password = password });
        }

        public bool cmpPassRepass(string password, string rePassword)
        {
            if (password == rePassword) return true;
            else return false;
        }

        public bool checkDuplicateUsername(string username)
        {
            foreach (var user in dataAccess.GetAllAccountToList())
            {
                if (user.userName == username)
                {
                    return false;
                }
            }
            return true;
        }


        public string checkUsernameForPassword(string username)
        {
            foreach (var user in dataAccess.GetAllAccountToList())
            {
                if (user.userName == username) return user.password;
            }
            return "Not found user name";
        }
    }
}

