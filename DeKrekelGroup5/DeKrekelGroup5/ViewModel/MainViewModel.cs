using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class MainViewModel
    {
        public LoginViewModel loginViewModel { get; set; }
        public BoekViewModel boekViewModel { get; set; }
        public SpelViewModel spelViewModel { get; set; }
        public UitlenerViewModel uitlenerViewModel { get; set; }
    }
}