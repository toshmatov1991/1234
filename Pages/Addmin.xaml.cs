using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
            //pass.Focus();
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
            string str = "";
            for (int i = 0; i < listviewCards.SelectedItem.ToString().Length; i++)
            {
                if (char.IsDigit(listviewCards.SelectedItem.ToString()[i]))
                    str += listviewCards.SelectedItem.ToString()[i];
                if (listviewCards.SelectedItem.ToString()[i] == ',')
                    break;
            }
            using(RealContext db = new())
            {
                var get = db.Realtors.Where(u => u.Id == Convert.ToInt64(str)).ToList().FirstOrDefault();
                fam.Text = get.Firstname;
                nam.Text = get.Name;
                las.Text = get.Lastname;
                log.Text = get.Login;
                pas.Text = get.Password;
                con.Text = get.Numberphone;
            }



            /*
            fam
            nam
            las
            log
            pas
            con
             */





        }
    }
}
