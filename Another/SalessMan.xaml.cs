using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        int i = 0;
        int r = 0;

        public SalessMan(int reald)
        {
            //Здесь просто задаю id покупателя
            InitializeComponent();
            GetMyClients();
            r = reald;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить в базу клиента
            




        }



        //Общая загрузка списка клиентов агенства
        private void GetMyClients()
        {
            
            poisk.Focus();
            using (RealContext db = new())
            {
                var getget = from clie in  db.Clients.AsNoTracking().ToList()
                             join pass in  db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                             where clie.Id != r
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

        //Динамический поиск без нареканий
        private void PoissKK(object sender, KeyEventArgs e)
        {
            //Динамический поиск
            string str = poisk.Text.ToLower().Replace(" ", "");
            using (RealContext db = new())
            {
                var getget = from clie in db.Clients.AsNoTracking().ToList()
                             join pass in db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                             where    clie.Firstname.ToLower().Contains(str) 
                                   || clie.Name.ToLower().Contains(str)
                                   || clie.Lastname.ToLower().Contains(str)
                                   && clie.Id != r
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

        //Получение id покупателя при двойном клике на любую запись в таблице
        private async void GetSalesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var str = GetMyId(listviewCards.SelectedItem.ToString());
            i = Convert.ToInt32(str);
            using(RealContext db = new())
            {
                var ddd = db.Clients.AsNoTracking().Where(u => u.Id == i).ToList();
                foreach (var item in ddd)
                {
                    str = $"{item.Firstname} {item.Name} {item.Lastname}";
                }
            }
            MessageBoxResult dialog = MessageBox.Show($"Добавить покупателя: {str}?", "Вы уверены в своем выборе?!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialog == MessageBoxResult.Yes)
            {
                using (StreamWriter writer = new StreamWriter("SalesMan.txt", false))
                {
                    await writer.WriteLineAsync(i.ToString());
                }
                Close();
            }
            else if(dialog == MessageBoxResult.No)
            {
                return;
            }

            static string GetMyId(string dsa)
            {
                string s = "";
                for (int i = 0; i < dsa.Length; i++)
                {
                    if (char.IsDigit(dsa[i]))
                        s += dsa[i];
                    else if (dsa[i] == ',')
                        break;
                }
                return s;
            }
        }
    }
}
