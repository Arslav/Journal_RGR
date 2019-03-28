using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMaster : ContentPage
    {
        public ListView ListView;

        public MainMaster()
        {
            InitializeComponent();

            BindingContext = new MainMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuItem> MenuItems { get; set; }

            public MainMasterViewModel()
            {
                MenuItems = new ObservableCollection<MenuItem>(new[]
                {
                    new MenuItem("Дисциплины", new DisciplineListPage(), "BooksIcon" ),
                    new MenuItem("Студенты", new StudentListPage(), "PeopleIcon"),
                    new MenuItem("Добавить запись", new CheckListPage(), "JournalIcon"),
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}