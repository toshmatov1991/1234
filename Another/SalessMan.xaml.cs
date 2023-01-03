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
        public SalessMan()
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
            using (RealContext db = new())
            {
                var getget = from clie in await db.Clients.ToListAsync()
                             join pass in await db.Passports.ToListAsync() on clie.PasswordFk equals pass.Id
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
