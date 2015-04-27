using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class Helper
    {
        public string CallBack { get; set; }

        public Helper(string callBack)
        {
            CallBack = callBack;
        }

        public Helper()
        {
            
        }
    }
}