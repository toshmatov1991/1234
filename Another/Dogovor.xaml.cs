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
        static int actual = 0;
        public Dogovor(int t, int realtoR)
        {
            InitializeComponent();
            iddd = t;
            ZapolnitDogovor();
            Pocupatel();
        }

        #region Заполнить данные о продавце (без нареканий)
        private void ZapolnitDogovor()
        {
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
        #endregion

        #region Заполнение данных о покупателе (решение идет)
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
                    
                    while (true)
                    {
                        Dispatcher.Invoke(async () =>
                        {
                            if (pokupatel.Text == "")
                            {
                                
                                if (MainWindow.salesmanhik == 0)
                                    return;

                                else
                                {
                                    actual = MainWindow.salesmanhik;
                                    using (RealContext db = new())
                                    {
                                        var getmypoc = from poc in await db.Clients.AsNoTracking().ToListAsync()
                                                       join pas in await db.Passports.AsNoTracking().ToListAsync() on poc.PasswordFk equals pas.Id
                                                       where poc.Id == Convert.ToInt32(MainWindow.salesmanhik)
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
                MessageBox.Show(e.Message);
            }
            
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить или изменить покупателя
            pokupatel.Text = "";
            paspoc.Text = "";
            
            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Закрыть окно
            Close();
        }

        private async void Update()
        {
            while (true)
            {
                if(actual != MainWindow.salesmanhik && MainWindow.salesmanhik > 0)
                {
                    using (RealContext db = new())
                    {
                        var getmypoc = from poc in await db.Clients.AsNoTracking().ToListAsync()
                                       join pas in await db.Passports.AsNoTracking().ToListAsync() on poc.PasswordFk equals pas.Id
                                       where poc.Id == Convert.ToInt32(MainWindow.salesmanhik)
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



    }
}
