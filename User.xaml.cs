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

namespace CP
{
    public partial class User : Window
    {
        public User(string str)
        {
            InitializeComponent();
            Title = str;
            MainFrame.Content = new Pages.Prodazha();
        }

        private void LeFt(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new Pages.Prodazha();
        }

        private void AdDRealse(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new Pages.AddReality();
        }

        private void SeeArhive(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Content = new Pages.Arhiv();
        }
    }
}
