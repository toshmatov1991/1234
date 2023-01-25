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
            bace.Visibility = Visibility.Hidden;
            pass.Focus();
        }

        private void Go(object sender, KeyEventArgs e)
        {
            if (pass.Password == "123")
            {
                qwe.Visibility = Visibility.Hidden;
                MessageBox.Show("Добро пожаловать, ваше администратейшество!)");
            }
               
        }
    }
}
