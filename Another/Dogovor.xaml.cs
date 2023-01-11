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
    public partial class Dogovor : Window
    {
        int iddd = 0;
       
        public Dogovor(int t, int realtoR)
        {
            InitializeComponent();
            iddd = t;
            Fire();
            ZapolnitDogovor();
            Pocupatel();
        }

        private async void Fire()
        {
            //Открывается окно выбора покупателя)
            await Task.Delay(2000);
            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }


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
                                    where real.Id == iddd
                                    select new
                                    {
                                        clie.Id,
                                        fio = $"{clie.Firstname} {clie.Name} {clie.Lastname}",
                                        ps = $"{pass.Serial} №{pass.Number}, выдан {pass.Dateof} {pass.Isby}",

                                    };
            }



            //FIO.Text; //ФИО
            //passPort.Text;//Паспортные данные продавца
            //kadastr.Text; //Кадастровый номер
            //kvadrat.Text; //Площадь
            //adress.Text; //Адрес
            //serianomer.Text//Свидетельство о праве собственности
            //datadogovora.Text//Дата заключения договора

        }


        private async void Pocupatel()
        {
            await GoVperde();
        }


        private async Task GoVperde()
        {
            //Заполнить данные о покупателе
            //Асинхронный метод, непрерывный цикл, если данные о покупателе пустые, то срабатывает условие и заполняются данные о покупателе
            await Task.Run(() =>
            {
                while (true)
                {
                    Dispatcher.Invoke(() =>
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
                                    var getmypoc = from poc in db.Clients.AsNoTracking().ToList()
                                                   join pas in db.Passports.AsNoTracking().ToList() on poc.PasswordFk equals pas.Id
                                                   where poc.Id == Convert.ToInt32(text)
                                                   select new
                                                   {
                                                       fiipoc = $"{poc.Firstname} {poc.Name} {poc.Lastname}",
                                                       paspoci = $"серии {pas.Serial} №{pas.Number}, выдан {pas.Dateof} {pas.Isby}"
                                                   };
                                    //pokupatel покупатель
                                    //paspoc его поспортные данные
                                    foreach (var item in getmypoc)
                                    {
                                        Dispatcher.Invoke(() =>
                                        {
                                            pokupatel.Text = item.fiipoc;
                                            paspoc.Text = item.paspoci;
                                        });

                                    }
                                }
                            }
                        }

                    });
                }
            });




        }






    }
}
