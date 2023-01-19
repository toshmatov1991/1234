using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CP.Another
{
    public partial class Dogovor : Window
    {
        int iddd = 0;
        public static int actual = 0;
        bool potok = true;
        public Dogovor(int t, int realtoR)
        {
            InitializeComponent();
            iddd = t;
            ZapolnitDogovor();
            new Thread(Update).Start();
        }

        #region Заполнить данные о продавце (без нареканий)
        private void ZapolnitDogovor()
        {
            //Подчеркнуть текст
            //Дата
            //Заполнить договор - данные продавца и недвижимости
            date.Text = DateTime.Now.ToString().Substring(0, 10);
            datadogovora.Text = DateTime.Now.ToString().Substring(0, 10);
            datadogovora.TextDecorations = TextDecorations.Underline;
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
                                        obshee = $"{type.Name} серия: {svidetelstvo.Serial} №{svidetelstvo.Number} от {svidetelstvo.Dateof} \nрегистрационный номер: {svidetelstvo.Registr}"
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

        #region Заполнение данных о покупателе (решено)
        private async void Update()
        {
            while (potok)
            {
                if (actual != MainWindow.salesmanhik && MainWindow.salesmanhik > 0)
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
                            Dispatcher.Invoke(() =>
                            {
                                pokupatel.Text = item.fiipoc;
                                paspoc.Text = item.paspoci;
                            });
                        }
                        actual = MainWindow.salesmanhik;
                    }
                }
            }
        }
        #endregion

        //Добавить или изменить покупателя(без нареканий)
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить или изменить покупателя
            pokupatel.Text = "";
            paspoc.Text = "";

            SalessMan salessMan = new(iddd);
            salessMan.ShowDialog();
        }

        //Закрыть окно(вроде без нареканий)
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Закрыть окно
            //Прервать поток(типо)
            potok = false;
            Close();
        }

        //Заключить договор(в работе)
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Заключить договор
            //Сверстать ПДФ + изменения в БД + сохранить пдф
            //Продавец FIO - passPort
            //Покупатель pokupatel - paspoc
            if (pokupatel.Text == "" || paspoc.Text == "" || pokupatel.Text == "" && paspoc.Text == "")
                MessageBox.Show("Не заполнены данные о покупателе", "Внимательней", MessageBoxButton.OK, MessageBoxImage.None);
            else
            {

                return;
            }


        }

        //Верстка PDF документа
        private static void PDFReaders()
        {
            //Сохранить документ //Задаем фильтр
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.FileName = "Договор купли-продажи квартиры";
            dialog.DefaultExt = "pdf";
            dialog.Filter = "PDF document (*.pdf)|*.pdf";

            //Открываем окно виндовс для сохранения документа
            if (dialog.ShowDialog() == true)
            {
                string str = dialog.FileName;

                //Формат документа = pdf
                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(str));

                //Создание документа + задаем формат pdf и A4 
                Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4);

                //Задаем стиль
                iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));


                //Чтоб поддерживал кириллицу в pdf документе
                PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");

                //Cell - Просто строка //SetFont(f2 - задаем русский шрифт, иначе не видит русские буквы)
                Cell cell2 = new Cell().Add(new iText.Layout.Element.Paragraph($"Гражданину:  Лазареву Святославу Андреевичу{""}")).SetFont(f2).SetMarginLeft(-20);

                //Вторая строка
                Cell cell3 = new Cell().Add(new Paragraph($"проживающему:   Пушкина 71б{""}")).SetFont(f2);

                Paragraph paragraph = new("Повестка");
                paragraph.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER).SetFont(f2).SetMarginLeft(220).SetFontSize(25);

                Cell cell5 = new Cell().Add(new Paragraph(""));
                //Cell cell6 = new Cell().Add(new Paragraph("Серия: ")).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER).SetFont(f2).SetMarginLeft(320);
                Paragraph paragraph2 = new Paragraph("Серия: 777").SetFont(f2).SetMarginLeft(390);
                Cell cell6 = new Cell().Add(new Paragraph(""));

                Cell cell7 = new Cell().Add(new Paragraph($"В соответствии с Федеральным законом от 28 марта 1998 г. N53-ФЗ \"О Воинской обязанности и военной службе\" Вы обязаны" +
                    $"{"Такого то числа"} к {"таком"} часам явиться в военный комиссариат:")).SetFont(f2);
                Cell cell8 = new Cell().Add(new Paragraph($"адрес военкомата")).SetFont(f2);
                Cell cell9 = new Cell().Add(new Paragraph($"для  {" Причина"}")).SetFont(f2);
                Cell cell10 = new Cell().Add(new Paragraph("\n")).SetFont(f2);
                Cell cell11 = new Cell().Add(new Paragraph($"При себе иметь паспорт, а также: {"ЧТО НУЖНО СПИСОК"}")).SetFont(f2);
                Cell cell12 = new Cell().Add(new Paragraph("\n")).SetFont(f2);
                Cell cell13 = new Cell().Add(new Paragraph("Военный комиссар области, города, района")).SetFont(f2);
                Cell cell14 = new Cell().Add(new Paragraph("\n")).SetFont(f2);
                Cell cell15 = new Cell().Add(new Paragraph($"М.П. {""}")).SetFont(f2);
                //Добавляем в документ
                doc.Add(cell2);
                doc.Add(cell3);
                doc.Add(paragraph);
                doc.Add(cell5);
                doc.Add(paragraph2);
                doc.Add(cell6);
                doc.Add(cell7);
                doc.Add(cell8);
                doc.Add(cell9);
                doc.Add(cell10);
                doc.Add(cell11);
                doc.Add(cell12);
                doc.Add(cell13);
                doc.Add(cell14);
                doc.Add(cell15);

                //Закрываем документ
                doc.Close();
            }
            
        }



    }
}
