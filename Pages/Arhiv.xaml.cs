using iText.Forms.Xfdf;
using iText.StyledXmlParser.Jsoup.Nodes;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CP.Pages
{
    public partial class Arhiv : Page
    {
        public Arhiv()
        {
            InitializeComponent();
            new Thread(UpDateDeals).Start();
        }


        private void UpDateDeals()
        {
            using(RealContext db = new())
            {
                var getmysdelka = from sdel in db.Deals
                                  join riel in db.Realtors on sdel.Realtor equals riel.Id
                                  join real in db.Realties on sdel.OfferCode equals real.Id
                                  join clie in db.Clients on sdel.Buyer equals clie.Id
                                  select new
                                  {
                                      realt = $"{riel.Firstname} {riel.Name} {riel.Lastname}",
                                      nedvizh = real.NameReal,
                                      prodavec = $"{clie.Firstname} {clie.Name} {clie.Lastname}",
                                      dat = sdel.TransactionDate,
                                      commis = sdel.Commission,
                                      comagent = sdel.RealtorReward,
                                      typeDeals = sdel.Dealtype
                                  };
                Dispatcher.Invoke(() =>
                {
                    listviewCards.ItemsSource = getmysdelka.ToList();
                });
            }
        }
    }
}
