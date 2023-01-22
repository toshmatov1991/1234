using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CP.Pages
{
    public partial class AddReality : Page
    {
        public AddReality()
        {
            InitializeComponent();
            new Thread(UpDateData).Start();
        }


        //Заполнить Comboboxes данными из связанных таблиц
        private void UpDateData()
        {
            using(RealContext db = new())
            {
                //Типы объектов
                var typeObj = db.ObjectTypes.FromSqlRaw("SELECT * FROM ObjectType").ToList();
                List<string> list = new();
                foreach (var type in typeObj){list.Add(type.Name);}

                //Районы
                var distr = db.Districts.FromSqlRaw("SELECT * FROM Districts").ToList();
                List<string> list1 = new();
                foreach (var item in distr) {list1.Add(item.Name);}
               
                //Санузел
                var sunuz = db.BathroomTypes.FromSqlRaw("SELECT * FROM BathroomType").ToList();
                List<string> list2 = new();
                foreach (var item in sunuz) { list2.Add(item.Type); }
                

                Dispatcher.Invoke(() =>
                {
                    category.ItemsSource = list;
                    rayon.ItemsSource = list1;
                    sunuzel.ItemsSource = list2;
                });
            }
        }












    }
}
