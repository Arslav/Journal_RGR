using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Journal_RGR
{

    public class MenuItem
    {
        public MenuItem(string title, Page page, string imagePath)
        {
            Title = title;
            Page = page;
            ImagePath = imagePath;
        }

        public string Title { get; set; }
        public Page Page { get; set; }
        public string ImagePath { get; set; }
    }
}