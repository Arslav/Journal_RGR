using Journal_RGR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentEditorPage : ContentPage
    {
        private Student _student;

        public StudentEditorPage(Student student = null)
        {
            InitializeComponent();
            _student = student;
            if (student != null)
            {
                NameEntry.Text = student.Name;
                PhoneEntry.Text = student.Phone;
                EmailEntry.Text = student.Email;
            }
            else DeleteButton.IsVisible = false;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            Student student = _student;
            if (_student == null) student = new Student();
            student.Name = NameEntry.Text;
            student.Phone = PhoneEntry.Text;
            student.Email = EmailEntry.Text;
            if (_student == null) App.DB.Students.Add(student);
            App.DB.SaveChanges();
            MessagingCenter.Send(this, "StudentUpdate");
            App.Back();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            App.DB.Students.Remove(_student);
            App.DB.SaveChanges();
            MessagingCenter.Send(this, "StudentUpdate");
            App.Back();
        }
    }
}