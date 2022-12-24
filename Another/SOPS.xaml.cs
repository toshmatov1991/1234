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
    public partial class SOPS : Window
    {
        public SOPS(int temp)
        {
            InitializeComponent();
            GetMyReestrInfo(temp);
        }


        private void GetMyReestrInfo(int t)
        {
            using (RealContext db = new())
            {
                var Rees = from real in db.Realties.ToList()
                           join pr in db.Proofs.ToList() on real.Certificate equals pr.Id
                           where real.Id == t
                           select new
                           {
                               dat = pr.Dateof,
                               reg = pr.Registr,
                               ser = pr.Serial,
                               num = pr.Number
                           };
                foreach (var re in Rees)
                {
                    date.Text = re.dat;
                    registr.Text = re.reg;
                    serial.Text = re.ser;
                    number.Text = re.num;
                }
            }
        }



    }
}
