﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeKrekelGroup5.ViewModel
{
    public class GebruikerViewModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Tes vandoen")]
        [MaxLength(10, ErrorMessage = " max 10 tekens")]
        [MinLength(6, ErrorMessage = " min 6 tekens")]
        public string Paswoord { get; set; }

        public bool IsBeheerder { get; set; }

        public bool IsBibliothecaris { get; set; }


        public GebruikerViewModel()
        {

        }
        public GebruikerViewModel(string username, string paswoord)
        {
            Username = username;
            Paswoord = paswoord;
        }


    }
}


