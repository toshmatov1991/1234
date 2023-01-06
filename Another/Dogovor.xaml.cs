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
        }

        private async void Fire()
        {

            await Task.Delay(2000);
            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }


        private void ZapolnitDogovor()
        {
            //Подчеркнуть текст
            //Дата
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

        private async Task GoVperde()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    if(pokupatel.Text == "" || pokupatel.Text == null)
                    {
                        using (StreamReader reader = new StreamReader("SalesMan.txt"))
                        {
                            string text = await reader.ReadToEndAsync();
                            //Запрос на заполнение данных покупателя

                        }
                    }
                   
                }
            });




        }






    }
}
