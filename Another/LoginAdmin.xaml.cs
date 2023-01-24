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
    public partial class LoginAdmin : Window
    {
        public LoginAdmin()
        {
            InitializeComponent();
            passe.Focus();
        }

        private void GoAdm(object sender, KeyEventArgs e)
        {
            if (passe.Password == "qwerty123")
            {
                User.clo = 1;
                Close();
            }
        }

        private void df_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
