using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CP.Another
{
    public partial class SalessMan : Window
    {
        public SalessMan(ref int pokupateli)
        {
            InitializeComponent();
            GetMyClients();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить в базу клиента

        }

        private async void GetMyClients()
        {
            poisk.Focus();
            using (RealContext db = new())
            {
                var getget = from clie in await db.Clients.AsNoTracking().ToListAsync()
                             join pass in await db.Passports.AsNoTracking().ToListAsync() on clie.PasswordFk equals pass.Id
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
            }
        }

        private async void PoissKK(object sender, KeyEventArgs e)
        {
            //Динамический поиск
            string str = poisk.Text.ToLower().Replace(" ", "");
            using (RealContext db = new())
            {
                var getget = from clie in await db.Clients.AsNoTracking().ToListAsync()
                             join pass in await db.Passports.AsNoTracking().ToListAsync() on clie.PasswordFk equals pass.Id
                             where    clie.Firstname.ToLower().Contains(str) 
                                   || clie.Name.ToLower().Contains(str)
                                   || clie.Lastname.ToLower().Contains(str)
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
            }
        }
    }
}
