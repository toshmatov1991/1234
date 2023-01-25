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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CP.Pages
{
    public partial class Addmin : Page
    {
        public Addmin()
        {
            InitializeComponent();
            pass.Focus();
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if (pass.Password == "123")
            {
                qwe.Visibility = Visibility.Hidden;
                MessageBox.Show("Добро пожаловать, ваше администратейшество!)");
                tec.Visibility = Visibility.Visible;
                listviewCards.Visibility = Visibility.Visible;
                using (RealContext db = new())
                {
                    var df = from g in db.Realtors
                             select new
                             {
                                 g.Id,
                                 fir = g.Firstname,
                                 nam = g.Name,
                                 las = g.Lastname,
                                 tel = g.Numberphone,
                                 log = g.Login,
                                 pas = g.Password
                             };
                    listviewCards.ItemsSource = df.ToList();
                }
                   
            }
               
        }

        private void Fer(object sender, MouseButtonEventArgs e)
        {
            //Двойной клик
            MessageBox.Show(listviewCards.SelectedItem.ToString());
        }
    }
}
