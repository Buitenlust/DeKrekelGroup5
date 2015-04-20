using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class InfoViewModel
    {
        public String Info { get; set; }
        public bool IsError { get; set; }
        public bool IsDialogBox { get; set; }
        public string CallBackAction { get; set; }

        public InfoViewModel()
        {
            
        }
    }
}