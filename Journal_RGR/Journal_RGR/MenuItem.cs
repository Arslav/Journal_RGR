﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Journal_RGR
{

    public class MenuItem
    {
       /* public MenuItem() {
            //TargetType = typeof(MainDetail);
        }*/
        public string Title { get; set; }
        //public Type TargetType {get;set;}
        public Page Page { get; set; }
    }
}