using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class LogoutViewModel
    {
        public string Username { get; set; }


        public LogoutViewModel()
        {

        }
        public LogoutViewModel(string username)
        {
            Username = username;
        }
    }
}