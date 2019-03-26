using Journal_RGR.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Journal_RGR.Views
{
    public class JournalDateTimeLabel : Label
    {
        private DateTime dateTime;

        public DateTime DateTime {
            get => dateTime;
            set
            {
                dateTime = value;
                Text = value.ToString("dd.MM");
            }
        }

        public Discipline Discipline { get; set; }

        public JournalDateTimeLabel(DateTime dateTime, Discipline discipline)
        {
            DateTime = dateTime;
            Discipline = discipline;
            HorizontalOptions = LayoutOptions.Center;
            TextDecorations = TextDecorations.Underline;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += TapGesture_Tapped;
            GestureRecognizers.Add(tapGesture);
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            App.Navigate(new CheckListPage(DateTime, Discipline));
        }
    }
}
