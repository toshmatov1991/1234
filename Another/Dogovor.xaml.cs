using iText.Kernel.Geom;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Dogovor : Window
    {
        int iddd = 0;
        bool gform= true;
        public Dogovor(int t, int realtoR)
        {
            InitializeComponent();
            iddd = t;
            ZapolnitDogovor();
            Pocupatel();
            Fire();
        }

        private async void Fire()
        {
            //Открывается окно выбора покупателя)
            await Task.Delay(1200);
            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }



        private void ZapolnitDogovor()
        {
            //File.Create("SalesMan.txt");
            //Подчеркнуть текст
            //Дата
            //Заполнить договор - данные продавца и недвижимости
            date.Text = DateTime.Now.DayOfYear.ToString();
            date.TextDecorations = TextDecorations.Underline;


            using (RealContext db = new())
            {
                var getmysalesman = from real in db.Realties.AsNoTracking().ToList()
                                    join clie in db.Clients.AsNoTracking().ToList() on real.Salesman equals clie.Id
                                    join pass in db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                                    join svidetelstvo in db.Proofs.AsNoTracking().ToList() on real.Certificate equals svidetelstvo.Id
                                    join type in db.ObjectTypes.AsNoTracking().ToList() on real.TypeObject equals type.Id
                                    where real.Id == iddd
                                    select new
                                    {
                                        clie.Id,
                                        fio = $"{clie.Firstname} {clie.Name} {clie.Lastname}",
                                        ps = $"{pass.Serial} №{pass.Number}, выдан {pass.Dateof} {pass.Isby}",
                                        summa = real.Price,
                                        kadastrnumber = svidetelstvo.Registr,
                                        ploshad = real.Square.ToString(),
                                        adrecc = real.Adress,
                                        obshee = $"{type.Name} серия: {svidetelstvo.Serial} №{svidetelstvo.Number} от {svidetelstvo.Dateof} регистрационный номер: {svidetelstvo.Registr}"
                                    };
                foreach (var item in getmysalesman)
                {
                   FIO.Text = item.fio;
                   passPort.Text = item.ps;
                   kadastr.Text = item.kadastrnumber;
                   kvadrat.Text = item.ploshad;
                   adress.Text = item.adrecc;
                   serianomer.Text = item.obshee;
                }
            }
        }

        #region Заполнение данных о покупателе
        private async void Pocupatel()
        {
            await GoVperde();
        }
        private async Task GoVperde()
        {
            //Заполнить данные о покупателе
            //Асинхронный метод, непрерывный цикл, если данные о покупателе пустые, то срабатывает условие и заполняются данные о покупателе
            try
            {
                await Task.Run(() =>
                {
                    
                    while (gform)
                    {
                        Dispatcher.Invoke(async () =>
                        {
                            if (pokupatel.Text == "" || pokupatel.Text == null)
                            {
                                string text = "";
                                using (StreamReader reader = new StreamReader("SalesMan.txt"))
                                    text = reader.ReadToEnd();
                                if (text == null || text == "")
                                    return;

                                else
                                {
                                    using (RealContext db = new())
                                    {
                                        var getmypoc = from poc in await db.Clients.AsNoTracking().ToListAsync()
                                                       join pas in await db.Passports.AsNoTracking().ToListAsync() on poc.PasswordFk equals pas.Id
                                                       where poc.Id == Convert.ToInt32(text)
                                                       select new
                                                       {
                                                           fiipoc = $"{poc.Firstname} {poc.Name} {poc.Lastname}",
                                                           paspoci = $"серии {pas.Serial} №{pas.Number}, выдан {pas.Dateof} {pas.Isby}"
                                                       };

                                        foreach (var item in getmypoc)
                                        {
                                            pokupatel.Text = item.fiipoc;
                                            paspoc.Text = item.paspoci;


                                        }
                                    }
                                }
                            }
                        });
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
            
        }
        #endregion

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить или изменить покупателя
            try
            {
                pokupatel.Text = "";
                paspoc.Text = "";
                using (StreamWriter writer = new StreamWriter("SalesMan.txt", false))
                {
                    await writer.WriteLineAsync("");
                }
                SalessMan salessMan = new(iddd);
                salessMan.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter("SalesMan.txt", false))
            {
                await writer.WriteLineAsync("");
            }
            Close();
        }
    }
}
